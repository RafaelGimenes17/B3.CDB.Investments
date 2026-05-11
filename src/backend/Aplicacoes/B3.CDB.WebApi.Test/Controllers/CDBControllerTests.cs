using B3.CDB.WebApi.Controllers;
using B3.CDB.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace B3.CDB.WebApi.Test.Controllers
{
    public class CDBControllerTests
    {
        private readonly CDBCalculator _cdbCalculator;
        private readonly CDBController _controller;

        public CDBControllerTests()
        {
            _cdbCalculator = new CDBCalculator();
            _controller = new CDBController(_cdbCalculator);
        }

        #region Testes do Construtor

        [Fact]
        public void Constructor_ComCDBCalculatorValido_DeveInicializarComSucesso()
        {
            // Arrange
            var cdbCalculator = new CDBCalculator();

            // Act
            var controller = new CDBController(cdbCalculator);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void Constructor_ComCDBCalculatorNulo_DeveLancarArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CDBController(null));
        }

        #endregion

        #region Testes CalcularValorFinal

        [Fact]
        public void CalcularValorFinal_ComDadosValidos_DeveRetornarOk()
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = 10000, Meses = 12 };

            // Act
            var resultado = _controller.CalcularValorFinal(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            Assert.NotNull(okResult.Value);
            var valor = Assert.IsType<decimal>(okResult.Value);
            Assert.True(valor > 10000);
        }

        [Fact]
        public void CalcularValorFinal_ComRequestNulo_DeveRetornarBadRequest()
        {
            // Arrange
            CalculoCDBRequest request = null;

            // Act
            var resultado = _controller.CalcularValorFinal(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void CalcularValorFinal_ComValorInicialNegativo_DeveRetornarBadRequest()
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = -1000, Meses = 12 };

            // Act
            var resultado = _controller.CalcularValorFinal(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void CalcularValorFinal_ComValorInicialZero_DeveRetornarBadRequest()
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = 0, Meses = 12 };

            // Act
            var resultado = _controller.CalcularValorFinal(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void CalcularValorFinal_ComMesesMenorOuIgualAUm_DeveRetornarBadRequest()
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = 10000, Meses = 1 };

            // Act
            var resultado = _controller.CalcularValorFinal(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void CalcularValorFinal_ComMesesZero_DeveRetornarBadRequest()
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = 10000, Meses = 0 };

            // Act
            var resultado = _controller.CalcularValorFinal(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Theory]
        [InlineData(1000, 2)]
        [InlineData(5000, 6)]
        [InlineData(10000, 12)]
        [InlineData(50000, 24)]
        public void CalcularValorFinal_ComVariosValores_DeveRetornarValoresCorretos(decimal valorInicial, int meses)
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = valorInicial, Meses = meses };

            // Act
            var resultado = _controller.CalcularValorFinal(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var valor = Assert.IsType<decimal>(okResult.Value);
            Assert.True(valor > valorInicial);
        }

        #endregion

        #region Testes CalcularRendimento

        [Fact]
        public void CalcularRendimento_ComDadosValidos_DeveRetornarOk()
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = 10000, Meses = 12 };

            // Act
            var resultado = _controller.CalcularRendimento(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            Assert.NotNull(okResult.Value);
            var rendimento = Assert.IsType<decimal>(okResult.Value);
            Assert.True(rendimento > 0);
        }

        [Fact]
        public void CalcularRendimento_ComRequestNulo_DeveRetornarBadRequest()
        {
            // Arrange
            CalculoCDBRequest request = null;

            // Act
            var resultado = _controller.CalcularRendimento(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void CalcularRendimento_ComValorInicialNegativo_DeveRetornarBadRequest()
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = -5000, Meses = 12 };

            // Act
            var resultado = _controller.CalcularRendimento(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void CalcularRendimento_ComMesesInvalido_DeveRetornarBadRequest()
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = 10000, Meses = 0 };

            // Act
            var resultado = _controller.CalcularRendimento(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Theory]
        [InlineData(1000, 2)]
        [InlineData(10000, 12)]
        public void CalcularRendimento_ComVariosValores_DeveRetornarRendimentoPositivo(decimal valorInicial, int meses)
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = valorInicial, Meses = meses };

            // Act
            var resultado = _controller.CalcularRendimento(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var rendimento = Assert.IsType<decimal>(okResult.Value);
            Assert.True(rendimento > 0);
        }

        #endregion

        #region Testes CalcularImposto

        [Fact]
        public void CalcularImposto_ComDadosValidos_DeveRetornarOk()
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = 10000, Meses = 12 };

            // Act
            var resultado = _controller.CalcularImposto(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            Assert.NotNull(okResult.Value);
            var imposto = Assert.IsType<decimal>(okResult.Value);
            Assert.True(imposto >= 0);
        }

        [Fact]
        public void CalcularImposto_ComRequestNulo_DeveRetornarBadRequest()
        {
            // Arrange
            CalculoCDBRequest request = null;

            // Act
            var resultado = _controller.CalcularImposto(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void CalcularImposto_ComValorInicialNegativo_DeveRetornarBadRequest()
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = -10000, Meses = 12 };

            // Act
            var resultado = _controller.CalcularImposto(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Theory]
        [InlineData(1000, 2)]
        [InlineData(10000, 6)]
        [InlineData(10000, 12)]
        public void CalcularImposto_ComVariosValores_DeveRetornarImpostoPositivo(decimal valorInicial, int meses)
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = valorInicial, Meses = meses };

            // Act
            var resultado = _controller.CalcularImposto(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var imposto = Assert.IsType<decimal>(okResult.Value);
            Assert.True(imposto >= 0);
        }

        #endregion

        #region Testes CalcularValorLiquido

        [Fact]
        public void CalcularValorLiquido_ComDadosValidos_DeveRetornarOk()
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = 10000, Meses = 12 };

            // Act
            var resultado = _controller.CalcularValorLiquido(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            Assert.NotNull(okResult.Value);
            var valorLiquido = Assert.IsType<decimal>(okResult.Value);
            Assert.True(valorLiquido > 10000);
            Assert.True(valorLiquido < 12000); // Valor esperado razoável
        }

        [Fact]
        public void CalcularValorLiquido_ComRequestNulo_DeveRetornarBadRequest()
        {
            // Arrange
            CalculoCDBRequest request = null;

            // Act
            var resultado = _controller.CalcularValorLiquido(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void CalcularValorLiquido_ComValorInicialNegativo_DeveRetornarBadRequest()
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = -10000, Meses = 12 };

            // Act
            var resultado = _controller.CalcularValorLiquido(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void CalcularValorLiquido_ComMesesInvalido_DeveRetornarBadRequest()
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = 10000, Meses = 1 };

            // Act
            var resultado = _controller.CalcularValorLiquido(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Theory]
        [InlineData(1000, 2)]
        [InlineData(5000, 12)]
        public void CalcularValorLiquido_ComVariosValores_DeveRetornarValorLiquidoMaiorQueInicial(decimal valorInicial, int meses)
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = valorInicial, Meses = meses };

            // Act
            var resultado = _controller.CalcularValorLiquido(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var valorLiquido = Assert.IsType<decimal>(okResult.Value);
            Assert.True(valorLiquido > valorInicial);
        }

        #endregion

        #region Testes ObtenerAliquotaImposto

        [Theory]
        [InlineData(1, 0.225)]
        [InlineData(3, 0.225)]
        [InlineData(6, 0.225)]
        [InlineData(7, 0.20)]
        [InlineData(12, 0.20)]
        [InlineData(13, 0.175)]
        [InlineData(24, 0.175)]
        [InlineData(25, 0.15)]
        [InlineData(36, 0.15)]
        public void ObtenerAliquotaImposto_ComVariosPeríodos_DeveRetornarAliquotaCorreta(int meses, decimal aliquotaEsperada)
        {
            // Act
            var resultado = _controller.ObtenerAliquotaImposto(meses);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var aliquota = Assert.IsType<decimal>(okResult.Value);
            Assert.Equal(aliquotaEsperada, aliquota);
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
            Assert.Equal(20, response.Aliquota); // 20% para 12 meses
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
        [InlineData(1000, 2, 0.225)]
        [InlineData(5000, 6, 0.225)]
        [InlineData(10000, 12, 0.20)]
        [InlineData(25000, 24, 0.175)]
        [InlineData(50000, 36, 0.15)]
        public void Calcular_ComVariosValores_DeveRetornarAliquotasCorretas(decimal valorInicial, int meses, decimal aliquotaEsperada)
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = valorInicial, Meses = meses };

            // Act
            var resultado = _controller.Calcular(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var response = Assert.IsType<CDBResultadoResponse>(okResult.Value);
            Assert.Equal(aliquotaEsperada * 100, response.Aliquota);
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

            // O valor líquido deve ser menor que o bruto (pois há desconto de imposto)
            Assert.True(response.ResultadoLiquido.Valor < response.ResultadoBruto.Valor);

            // O rendimento líquido deve ser menor que o bruto
            Assert.True(response.ResultadoLiquido.Rendimento < response.ResultadoBruto.Rendimento);

            // A diferença deve ser igual ao imposto
            var diferenca = response.ResultadoBruto.Rendimento - response.ResultadoLiquido.Rendimento;
            Assert.Equal(response.Imposto, diferenca, precision: 2);
        }

        [Fact]
        public void Calcular_ComPeriodosCurtos_DeveAplicarAliquotaMaiorDe22Porcento()
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = 10000, Meses = 3 };

            // Act
            var resultado = _controller.Calcular(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var response = Assert.IsType<CDBResultadoResponse>(okResult.Value);
            Assert.Equal(22.5m, response.Aliquota);
        }

        [Fact]
        public void Calcular_ComPeriodosLongos_DeveAplicarAliquotaMenorDe15Porcento()
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = 10000, Meses = 36 };

            // Act
            var resultado = _controller.Calcular(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var response = Assert.IsType<CDBResultadoResponse>(okResult.Value);
            Assert.Equal(15m, response.Aliquota);
        }

        #endregion

        #region Testes de Integração - Fluxo Completo

        [Fact]
        public void FluxoCompleto_CalcularEVerificarConsistenciaDados()
        {
            // Arrange
            var request = new CalculoCDBRequest { ValorInicial = 10000, Meses = 12 };

            // Act - Calcular todos os valores
            var resultadoValorFinal = _controller.CalcularValorFinal(request);
            var resultadoRendimento = _controller.CalcularRendimento(request);
            var resultadoImposto = _controller.CalcularImposto(request);
            var resultadoValorLiquido = _controller.CalcularValorLiquido(request);
            var resultadoAliquota = _controller.ObtenerAliquotaImposto(12);
            var resultadoConsolidado = _controller.Calcular(request);

            // Assert - Verificar consistência
            var valorFinal = (decimal)((OkObjectResult)resultadoValorFinal.Result).Value;
            var rendimento = (decimal)((OkObjectResult)resultadoRendimento.Result).Value;
            var imposto = (decimal)((OkObjectResult)resultadoImposto.Result).Value;
            var valorLiquido = (decimal)((OkObjectResult)resultadoValorLiquido.Result).Value;
            var aliquota = (decimal)((OkObjectResult)resultadoAliquota.Result).Value;
            var consolidado = (CDBResultadoResponse)((OkObjectResult)resultadoConsolidado.Result).Value;

            // Verificar coerência entre valores
            Assert.Equal(valorFinal, consolidado.ResultadoBruto.Valor, precision: 2);
            Assert.Equal(rendimento, consolidado.ResultadoBruto.Rendimento, precision: 2);
            Assert.Equal(imposto, consolidado.Imposto, precision: 2);
            Assert.Equal(valorLiquido, consolidado.ResultadoLiquido.Valor, precision: 2);
            Assert.Equal(aliquota * 100, consolidado.Aliquota, precision: 2);
        }

        #endregion
    }
}
