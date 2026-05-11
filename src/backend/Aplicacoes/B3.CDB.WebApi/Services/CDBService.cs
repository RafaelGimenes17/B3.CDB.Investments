using B3.CDB.WebApi.DTOs;

namespace B3.CDB.WebApi.Services
{
    /// <summary>
    /// Implementação do serviço de cálculo de CDB.
    /// Orquestra as chamadas ao <see cref="CDBCalculator"/>, aplica arredondamentos
    /// financeiros e monta o DTO de resposta completo.
    /// </summary>
    public class CDBService : ICDBService
    {
        private readonly CDBCalculator _cdbCalculator;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="CDBService"/>.
        /// </summary>
        /// <param name="cdbCalculator">Calculadora de CDB responsável pelos cálculos matemáticos.</param>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="cdbCalculator"/> é nulo.</exception>
        public CDBService(CDBCalculator cdbCalculator)
        {
            _cdbCalculator = cdbCalculator ?? throw new ArgumentNullException(nameof(cdbCalculator));
        }

        /// <inheritdoc/>
        public CDBResultadoResponse Calcular(decimal valorInicial, int meses)
        {
            // Calcular o valor final uma única vez e derivar os demais valores a partir dele,
            // evitando chamadas redundantes ao loop de composição mensal.
            decimal valorFinal = _cdbCalculator.CalcularValorFinal(valorInicial, meses);
            decimal aliquota = _cdbCalculator.ObterAliquotaImposto(meses);
            decimal rendimento = valorFinal - valorInicial;
            decimal imposto = rendimento * aliquota;
            decimal valorLiquido = valorFinal - imposto;

            return new CDBResultadoResponse
            {
                ValorInicial = Math.Round(valorInicial, 2),
                Meses = meses,
                Aliquota = Math.Round(aliquota * 100, 2),
                ResultadoBruto = new ResultadoDetalhadoResponse
                {
                    Valor = Math.Round(valorFinal, 2),
                    Rendimento = Math.Round(rendimento, 2)
                },
                ResultadoLiquido = new ResultadoDetalhadoResponse
                {
                    Valor = Math.Round(valorLiquido, 2),
                    Rendimento = Math.Round(rendimento - imposto, 2)
                },
                Imposto = Math.Round(imposto, 2)
            };
        }
    }
}
