namespace B3.CDB.WebApi.Services
{
    /// <summary>
    /// Responsável pelos cálculos matemáticos de investimentos em CDB,
    /// incluindo valor final, rendimento, alíquota de IR e valor líquido.
    /// </summary>
    public class CDBCalculator
    {
        private const decimal TaxaBanco = 1.08m;  // 108%
        private const decimal TaxaCdi = 0.009m;   // 0,9%

        /// <summary>
        /// Fator de crescimento mensal pré-calculado: 1 + (CDI × TB).
        /// Calculado uma única vez como campo estático para evitar recálculo a cada chamada.
        /// </summary>
        private static readonly decimal FatorMensal = 1 + (TaxaCdi * TaxaBanco);

        /// <summary>
        /// Calcula o valor final de um investimento em CDB com base no valor inicial e número de meses.
        /// Utiliza a fórmula: VF = VI × [1 + (CDI × TB)]^n
        /// O cálculo é realizado mês a mês, onde os rendimentos de cada mês são utilizados para calcular o mês seguinte.
        /// </summary>
        /// <param name="valorInicial">Valor inicial do investimento.</param>
        /// <param name="meses">Número de meses para aplicar o cálculo.</param>
        /// <returns>Valor final do investimento após os meses especificados.</returns>
        /// <exception cref="ArgumentException">
        /// Lançada quando <paramref name="valorInicial"/> é menor ou igual a zero,
        /// ou quando <paramref name="meses"/> é menor ou igual a 1.
        /// </exception>
        public decimal CalcularValorFinal(decimal valorInicial, int meses)
        {
            if (valorInicial <= 0)
                throw new ArgumentException("Valor inicial deve ser positivo", nameof(valorInicial));

            if (meses <= 1)
                throw new ArgumentException("Prazo deve ser maior que 1 mês", nameof(meses));

            decimal resultado = valorInicial;

            for (int i = 0; i < meses; i++)
            {
                resultado *= FatorMensal;
            }

            return resultado;
        }

        /// <summary>
        /// Calcula o rendimento total obtido no investimento em CDB.
        /// </summary>
        /// <param name="valorInicial">Valor inicial do investimento.</param>
        /// <param name="meses">Número de meses para aplicar o cálculo.</param>
        /// <returns>Rendimento total (Valor Final - Valor Inicial).</returns>
        public decimal CalcularRendimento(decimal valorInicial, int meses)
        {
            decimal valorFinal = CalcularValorFinal(valorInicial, meses);
            return valorFinal - valorInicial;
        }

        /// <summary>
        /// Obtém a alíquota de Imposto de Renda (IR) baseada no número de meses.
        /// Tabela progressiva:
        /// <list type="bullet">
        ///   <item><description>Até 6 meses: 22,5%</description></item>
        ///   <item><description>Até 12 meses: 20%</description></item>
        ///   <item><description>Até 24 meses: 17,5%</description></item>
        ///   <item><description>Acima de 24 meses: 15%</description></item>
        /// </list>
        /// </summary>
        /// <param name="meses">Número de meses do investimento.</param>
        /// <returns>Alíquota de imposto em decimal (ex: 0.225 para 22,5%).</returns>
        public decimal ObterAliquotaImposto(int meses)
        {
            return meses switch
            {
                <= 6 => 0.225m,   // 22,5%
                <= 12 => 0.20m,   // 20%
                <= 24 => 0.175m,  // 17,5%
                _ => 0.15m        // 15%
            };
        }

        /// <summary>
        /// Calcula o valor do Imposto de Renda (IR) sobre o rendimento do CDB.
        /// </summary>
        /// <param name="valorInicial">Valor inicial do investimento.</param>
        /// <param name="meses">Número de meses para aplicar o cálculo.</param>
        /// <returns>Valor do imposto em reais.</returns>
        public decimal CalcularImposto(decimal valorInicial, int meses)
        {
            decimal rendimento = CalcularRendimento(valorInicial, meses);
            decimal aliquota = ObterAliquotaImposto(meses);
            return rendimento * aliquota;
        }

        /// <summary>
        /// Calcula o valor líquido final (após dedução do imposto) do investimento em CDB.
        /// </summary>
        /// <param name="valorInicial">Valor inicial do investimento.</param>
        /// <param name="meses">Número de meses para aplicar o cálculo.</param>
        /// <returns>Valor final líquido (Valor Final - Imposto).</returns>
        public decimal CalcularValorLiquido(decimal valorInicial, int meses)
        {
            decimal valorFinal = CalcularValorFinal(valorInicial, meses);
            decimal imposto = CalcularImposto(valorInicial, meses);
            return valorFinal - imposto;
        }
    }
}
