using B3.CDB.WebApi.Controllers;
using B3.CDB.WebApi.DTOs;
using B3.CDB.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace B3.CDB.WebApi.Test.Controllers
{
    public class CDBControllerTests
    {
        private readonly CDBCalculator _cdbCalculator;
        private readonly CDBService _cdbService;
        private readonly CDBController _controller;

        public CDBControllerTests()
        {
            _cdbCalculator = new CDBCalculator();
            _cdbService = new CDBService(_cdbCalculator);
            _controller = new CDBController(_cdbService);
        }

        #region Testes do Construtor

        [Fact]
        public void Constructor_ComCDBServiceValido_DeveInicializarComSucesso()
        {
            // Arrange
            var cdbCalculator = new CDBCalculator();
            var cdbService = new CDBService(cdbCalculator);

            // Act
            var controller = new CDBController(cdbService);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void Constructor_ComCDBServiceNulo_DeveLancarArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CDBController(null));
        }

        #endregion

        #region Testes Calcular (Consolidado)

        [Fact]
        public void Calcular_ComDadosValidos_DeveRetornarCDBResultadoResponseCompleto()
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = 10000, Meses = 12 };

            // Act
            var resultado = _controller.Calcular(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var response = Assert.IsType<CDBResultadoResponse>(okResult.Value);

            Assert.Equal(10000, response.ValorInicial);
            Assert.Equal(12, response.Meses);
            Assert.Equal(20, response.Aliquota);
            Assert.NotNull(response.ResultadoBruto);
            Assert.NotNull(response.ResultadoLiquido);
            Assert.True(response.ResultadoBruto.Valor > 10000);
            Assert.True(response.ResultadoLiquido.Valor > 10000);
            Assert.True(response.Imposto > 0);
        }

        [Fact]
        public void Calcular_ComRequestNulo_DeveRetornarBadRequest()
        {
            // Arrange
            CalculoCDBRequest request = null;

            // Act
            var resultado = _controller.Calcular(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void Calcular_ComValorInicialNegativo_DeveRetornarBadRequest()
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = -5000, Meses = 12 };

            // Act
            var resultado = _controller.Calcular(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void Calcular_ComMesesInvalido_DeveRetornarBadRequest()
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = 10000, Meses = 0 };

            // Act
            var resultado = _controller.Calcular(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Theory]
        [InlineData(1000, 2, 22.5)]
        [InlineData(5000, 6, 22.5)]
        [InlineData(10000, 12, 20)]
        [InlineData(25000, 24, 17.5)]
        [InlineData(50000, 36, 15)]
        public void Calcular_ComVariosValores_DeveRetornarAliquotasCorretas(decimal valorInicial, int meses, double aliquotaEsperada)
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = valorInicial, Meses = meses };

            // Act
            var resultado = _controller.Calcular(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var response = Assert.IsType<CDBResultadoResponse>(okResult.Value);
            Assert.Equal((decimal)aliquotaEsperada, response.Aliquota);
        }

        [Fact]
        public void Calcular_VerificaCoerenciaEntreBrutoELiquido_DeveVerificarQueLiquidoEMenorQueBruto()
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = 10000, Meses = 12 };

            // Act
            var resultado = _controller.Calcular(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var response = Assert.IsType<CDBResultadoResponse>(okResult.Value);

            Assert.True(response.ResultadoLiquido.Valor < response.ResultadoBruto.Valor);
            Assert.True(response.ResultadoLiquido.Rendimento < response.ResultadoBruto.Rendimento);

            var diferenca = response.ResultadoBruto.Rendimento - response.ResultadoLiquido.Rendimento;
            Assert.Equal(response.Imposto, diferenca, precision: 2);
        }

        #endregion
    }
}
