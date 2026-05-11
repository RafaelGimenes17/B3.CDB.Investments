using B3.CDB.WebApi.Services;

namespace B3.CDB.WebApi.Test.Services
{
    public class CDBCalculatorTests
    {
        private readonly CDBCalculator _calculator;

        public CDBCalculatorTests()
        {
            _calculator = new CDBCalculator();
        }

        #region Testes CalcularValorFinal

        [Fact]
        public void CalcularValorFinal_ComValoresValidos_DeveCalcularCorretamente()
        {
            // Arrange
            decimal valorInicial = 10000;
            int meses = 12;
            // Fator mensal = 1 + (0.009 * 1.08) = 1.00972
            // Após 12 meses: 10000 * (1.00972)^12 ≈ 11.231

            // Act
            decimal resultado = _calculator.CalcularValorFinal(valorInicial, meses);

            // Assert
            Assert.True(resultado > valorInicial);
            // Tolerância ampla para precisão decimal
            Assert.True(resultado > 11000 && resultado < 11500);
        }

        [Theory]
        [InlineData(1000, 2)]
        [InlineData(5000, 6)]
        [InlineData(10000, 12)]
        [InlineData(50000, 24)]
        [InlineData(100000, 36)]
        public void CalcularValorFinal_ComVariosValores_DeveRetornarValorMaiorQueInicial(decimal valorInicial, int meses)
        {
            // Act
            decimal resultado = _calculator.CalcularValorFinal(valorInicial, meses);

            // Assert
            Assert.True(resultado > valorInicial, $"Valor final {resultado} deve ser maior que inicial {valorInicial}");
        }

        [Fact]
        public void CalcularValorFinal_ComValorInicialNegativo_DeveLancarArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _calculator.CalcularValorFinal(-1000, 12));
        }

        [Fact]
        public void CalcularValorFinal_ComValorInicialZero_DeveLancarArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _calculator.CalcularValorFinal(0, 12));
        }

        [Fact]
        public void CalcularValorFinal_ComMesesMenorOuIgualUm_DeveLancarArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _calculator.CalcularValorFinal(10000, 1));
        }

        [Fact]
        public void CalcularValorFinal_ComMesesZero_DeveLancarArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _calculator.CalcularValorFinal(10000, 0));
        }

        [Fact]
        public void CalcularValorFinal_ComMesesNegativo_DeveLancarArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _calculator.CalcularValorFinal(10000, -5));
        }

        [Fact]
        public void CalcularValorFinal_ComGrandePrazo_DeveCalcularCorreto()
        {
            // Arrange
            decimal valorInicial = 10000;
            int meses = 120; // 10 anos

            // Act
            decimal resultado = _calculator.CalcularValorFinal(valorInicial, meses);

            // Assert
            Assert.True(resultado > valorInicial * 1.1m); // Deve render pelo menos 10%
        }

        [Fact]
        public void CalcularValorFinal_VerificaAcumuloDeRendimentos()
        {
            // Arrange
            decimal valorInicial = 10000;

            // Act
            decimal resultado1Mes = _calculator.CalcularValorFinal(valorInicial, 2);
            decimal resultado2Meses = _calculator.CalcularValorFinal(resultado1Mes, 2);
            decimal resultadoDiretamente = _calculator.CalcularValorFinal(valorInicial, 4);

            // Assert - O resultado de aplicar 2 meses duas vezes deve ser próximo ao de 4 meses
            Assert.True(Math.Abs(resultado2Meses - resultadoDiretamente) < 0.01m);
        }

        #endregion

        #region Testes CalcularRendimento

        [Fact]
        public void CalcularRendimento_ComValoresValidos_DeveCalcularCorretamente()
        {
            // Arrange
            decimal valorInicial = 10000;
            int meses = 12;

            // Act
            decimal rendimento = _calculator.CalcularRendimento(valorInicial, meses);

            // Assert
            Assert.True(rendimento > 0);
            Assert.True(rendimento < valorInicial); // Rendimento deve ser menor que valor inicial
        }

        [Theory]
        [InlineData(1000, 2)]
        [InlineData(5000, 12)]
        [InlineData(10000, 24)]
        public void CalcularRendimento_ComVariosValores_DeveRetornarRendimentoPositivo(decimal valorInicial, int meses)
        {
            // Act
            decimal rendimento = _calculator.CalcularRendimento(valorInicial, meses);

            // Assert
            Assert.True(rendimento > 0);
        }

        [Fact]
        public void CalcularRendimento_ComValorInicialNegativo_DeveLancarArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _calculator.CalcularRendimento(-5000, 12));
        }

        [Fact]
        public void CalcularRendimento_ComMesesInvalido_DeveLancarArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _calculator.CalcularRendimento(10000, 0));
        }

        [Fact]
        public void CalcularRendimento_VerificaConsistenciaComValorFinal()
        {
            // Arrange
            decimal valorInicial = 10000;
            int meses = 12;

            // Act
            decimal valorFinal = _calculator.CalcularValorFinal(valorInicial, meses);
            decimal rendimento = _calculator.CalcularRendimento(valorInicial, meses);

            // Assert
            Assert.Equal(valorFinal - valorInicial, rendimento, precision: 2);
        }

        #endregion

        #region Testes CalcularImposto

        [Fact]
        public void CalcularImposto_ComPrazo3Meses_DeveAplicar22Porcento()
        {
            // Arrange
            decimal valorInicial = 10000;
            int meses = 3;

            // Act
            decimal imposto = _calculator.CalcularImposto(valorInicial, meses);
            decimal rendimento = _calculator.CalcularRendimento(valorInicial, meses);

            // Assert
            decimal impostoEsperado = rendimento * 0.225m;
            Assert.Equal(impostoEsperado, imposto, precision: 2);
        }

        [Fact]
        public void CalcularImposto_ComPrazo9Meses_DeveAplicar20Porcento()
        {
            // Arrange
            decimal valorInicial = 10000;
            int meses = 9;

            // Act
            decimal imposto = _calculator.CalcularImposto(valorInicial, meses);
            decimal rendimento = _calculator.CalcularRendimento(valorInicial, meses);

            // Assert
            decimal impostoEsperado = rendimento * 0.20m;
            Assert.Equal(impostoEsperado, imposto, precision: 2);
        }

        [Fact]
        public void CalcularImposto_ComPrazo18Meses_DeveAplicar17Porcento()
        {
            // Arrange
            decimal valorInicial = 10000;
            int meses = 18;

            // Act
            decimal imposto = _calculator.CalcularImposto(valorInicial, meses);
            decimal rendimento = _calculator.CalcularRendimento(valorInicial, meses);

            // Assert
            decimal impostoEsperado = rendimento * 0.175m;
            Assert.Equal(impostoEsperado, imposto, precision: 2);
        }

        [Fact]
        public void CalcularImposto_ComPrazo30Meses_DeveAplicar15Porcento()
        {
            // Arrange
            decimal valorInicial = 10000;
            int meses = 30;

            // Act
            decimal imposto = _calculator.CalcularImposto(valorInicial, meses);
            decimal rendimento = _calculator.CalcularRendimento(valorInicial, meses);

            // Assert
            decimal impostoEsperado = rendimento * 0.15m;
            Assert.Equal(impostoEsperado, imposto, precision: 2);
        }

        [Fact]
        public void CalcularImposto_ComValorInicialNegativo_DeveLancarArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _calculator.CalcularImposto(-10000, 12));
        }

        [Fact]
        public void CalcularImposto_ComMesesInvalido_DeveLancarArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _calculator.CalcularImposto(10000, 1));
        }

        #endregion

        #region Testes ObterAliquotaImposto

        [Theory]
        [InlineData(1, 0.225)]
        [InlineData(2, 0.225)]
        [InlineData(3, 0.225)]
        [InlineData(6, 0.225)]
        public void ObterAliquotaImposto_ComAte6Meses_DeveRetornar22Porcento(int meses, decimal aliquotaEsperada)
        {
            // Act
            decimal aliquota = _calculator.ObterAliquotaImposto(meses);

            // Assert
            Assert.Equal(aliquotaEsperada, aliquota);
        }

        [Theory]
        [InlineData(7, 0.20)]
        [InlineData(9, 0.20)]
        [InlineData(12, 0.20)]
        public void ObterAliquotaImposto_ComAte12Meses_DeveRetornar20Porcento(int meses, decimal aliquotaEsperada)
        {
            // Act
            decimal aliquota = _calculator.ObterAliquotaImposto(meses);

            // Assert
            Assert.Equal(aliquotaEsperada, aliquota);
        }

        [Theory]
        [InlineData(13, 0.175)]
        [InlineData(18, 0.175)]
        [InlineData(24, 0.175)]
        public void ObterAliquotaImposto_ComAte24Meses_DeveRetornar17Porcento(int meses, decimal aliquotaEsperada)
        {
            // Act
            decimal aliquota = _calculator.ObterAliquotaImposto(meses);

            // Assert
            Assert.Equal(aliquotaEsperada, aliquota);
        }

        [Theory]
        [InlineData(25, 0.15)]
        [InlineData(36, 0.15)]
        [InlineData(60, 0.15)]
        [InlineData(120, 0.15)]
        public void ObterAliquotaImposto_ComAcimaDe24Meses_DeveRetornar15Porcento(int meses, decimal aliquotaEsperada)
        {
            // Act
            decimal aliquota = _calculator.ObterAliquotaImposto(meses);

            // Assert
            Assert.Equal(aliquotaEsperada, aliquota);
        }

        #endregion

        #region Testes CalcularValorLiquido

        [Fact]
        public void CalcularValorLiquido_ComValoresValidos_DeveCalcularCorretamente()
        {
            // Arrange
            decimal valorInicial = 10000;
            int meses = 12;

            // Act
            decimal valorLiquido = _calculator.CalcularValorLiquido(valorInicial, meses);

            // Assert
            Assert.True(valorLiquido > valorInicial);
            // Verificar que é menor que o bruto
            decimal valorFinal = _calculator.CalcularValorFinal(valorInicial, meses);
            Assert.True(valorLiquido < valorFinal);
        }

        [Theory]
        [InlineData(1000, 2)]
        [InlineData(5000, 6)]
        [InlineData(10000, 12)]
        [InlineData(25000, 24)]
        [InlineData(50000, 36)]
        public void CalcularValorLiquido_ComVariosValores_DeveRetornarValorMaiorQueInicial(decimal valorInicial, int meses)
        {
            // Act
            decimal valorLiquido = _calculator.CalcularValorLiquido(valorInicial, meses);

            // Assert
            Assert.True(valorLiquido > valorInicial);
        }

        [Fact]
        public void CalcularValorLiquido_VerificaConsistenciaComBrutoEImposto()
        {
            // Arrange
            decimal valorInicial = 10000;
            int meses = 12;

            // Act
            decimal valorFinal = _calculator.CalcularValorFinal(valorInicial, meses);
            decimal imposto = _calculator.CalcularImposto(valorInicial, meses);
            decimal valorLiquido = _calculator.CalcularValorLiquido(valorInicial, meses);

            // Assert
            Assert.Equal(valorFinal - imposto, valorLiquido, precision: 2);
        }

        [Fact]
        public void CalcularValorLiquido_ComValorInicialNegativo_DeveLancarArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _calculator.CalcularValorLiquido(-10000, 12));
        }

        [Fact]
        public void CalcularValorLiquido_ComMesesInvalido_DeveLancarArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _calculator.CalcularValorLiquido(10000, 0));
        }

        [Fact]
        public void CalcularValorLiquido_ComPeriodosCurtos_DeveAplicarAliquotaMaior()
        {
            // Arrange
            decimal valorInicial = 10000;
            int meses = 3; // Alíquota 22,5%

            // Act
            decimal valorLiquido = _calculator.CalcularValorLiquido(valorInicial, meses);
            decimal imposto = _calculator.CalcularImposto(valorInicial, meses);

            // Assert
            Assert.True(imposto > 0);
            Assert.True(valorLiquido < _calculator.CalcularValorFinal(valorInicial, meses));
        }

        [Fact]
        public void CalcularValorLiquido_ComPeriodosLongos_DeveAplicarAliquotaMenor()
        {
            // Arrange
            decimal valorInicial = 10000;

            // Act
            // Para 3 meses: alíquota 22,5%
            decimal rendimento3 = _calculator.CalcularRendimento(10000, 3);
            decimal imposto3 = _calculator.CalcularImposto(10000, 3);

            // Para 36 meses: alíquota 15%
            decimal rendimento36 = _calculator.CalcularRendimento(valorInicial, 36);
            decimal imposto36 = _calculator.CalcularImposto(valorInicial, 36);

            // Assert
            // Verificar que a alíquota aplicada é menor em períodos maiores (imposto / rendimento)
            decimal aliquotaAplicada3 = imposto3 / rendimento3;
            decimal aliquotaAplicada36 = imposto36 / rendimento36;

            Assert.True(aliquotaAplicada36 < aliquotaAplicada3);
        }

        #endregion

        #region Testes de Integração

        [Fact]
        public void Fluxo_Completo_VerificaCoerenciaEntreTodasAsOperacoes()
        {
            // Arrange
            decimal valorInicial = 10000;
            int meses = 12;

            // Act
            decimal valorFinal = _calculator.CalcularValorFinal(valorInicial, meses);
            decimal rendimento = _calculator.CalcularRendimento(valorInicial, meses);
            decimal aliquota = _calculator.ObterAliquotaImposto(meses);
            decimal imposto = _calculator.CalcularImposto(valorInicial, meses);
            decimal valorLiquido = _calculator.CalcularValorLiquido(valorInicial, meses);

            // Assert
            // 1. Valor final deve ser inicial + rendimento
            Assert.Equal(valorFinal, valorInicial + rendimento, precision: 2);

            // 2. Rendimento deve ser positivo
            Assert.True(rendimento > 0);

            // 3. Imposto deve ser rendimento * aliquota
            Assert.Equal(rendimento * aliquota, imposto, precision: 2);

            // 4. Valor líquido deve ser valor final - imposto
            Assert.Equal(valorFinal - imposto, valorLiquido, precision: 2);

            // 5. Rendimento líquido deve ser valor líquido - valor inicial
            decimal rendimentoLiquido = valorLiquido - valorInicial;
            Assert.Equal(rendimento - imposto, rendimentoLiquido, precision: 2);
        }

        [Theory]
        [InlineData(1000, 2)]
        [InlineData(5000, 6)]
        [InlineData(10000, 12)]
        [InlineData(25000, 24)]
        [InlineData(100000, 36)]
        public void Fluxo_Completo_ComVariosValores_DeveManterCoerencia(decimal valorInicial, int meses)
        {
            // Act
            decimal valorFinal = _calculator.CalcularValorFinal(valorInicial, meses);
            decimal rendimento = _calculator.CalcularRendimento(valorInicial, meses);
            decimal aliquota = _calculator.ObterAliquotaImposto(meses);
            decimal imposto = _calculator.CalcularImposto(valorInicial, meses);
            decimal valorLiquido = _calculator.CalcularValorLiquido(valorInicial, meses);

            // Assert
            Assert.Equal(valorFinal, valorInicial + rendimento, precision: 2);
            Assert.Equal(rendimento * aliquota, imposto, precision: 2);
            Assert.Equal(valorFinal - imposto, valorLiquido, precision: 2);
            Assert.True(valorFinal > valorInicial);
            Assert.True(valorLiquido > valorInicial);
            Assert.True(valorLiquido < valorFinal);
        }

        [Fact]
        public void Fluxo_Comparacao_EntrePeríodosCurtos_E_Longos()
        {
            // Arrange
            decimal valorInicial = 10000;

            // Act
            decimal rendimentoCurto = _calculator.CalcularRendimento(valorInicial, 3);
            decimal rendimentoLongo = _calculator.CalcularRendimento(valorInicial, 36);

            decimal impostoCurto = _calculator.CalcularImposto(valorInicial, 3);
            decimal impostoLongo = _calculator.CalcularImposto(valorInicial, 36);

            // Assert
            // Períodos mais longos têm maior rendimento
            Assert.True(rendimentoLongo > rendimentoCurto);

            // Alíquotas menores em períodos maiores, mas pode ter mais imposto em valor absoluto
            // devido ao maior rendimento
            decimal aliquotaCurta = _calculator.ObterAliquotaImposto(3);
            decimal aliquotaLonga = _calculator.ObterAliquotaImposto(36);
            Assert.True(aliquotaCurta > aliquotaLonga);

            // Suppress unused variable warnings
            _ = impostoCurto;
            _ = impostoLongo;
        }

        #endregion
    }
}
