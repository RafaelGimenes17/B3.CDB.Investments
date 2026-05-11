namespace B3.CDB.WebApi.Services
{
    public class CDBCalculator
    {
        private const decimal TB = 1.08m;  // 108%
        private const decimal CDI = 0.009m; // 0,9%

        /// <summary>
        /// Calcula o valor final de um investimento em CDB com base no valor inicial e número de meses.
        /// Utiliza a fórmula: VF = VI × [1 + (CDI × TB)]
        /// O cálculo é realizado mês a mês, onde os rendimentos de cada mês são utilizados para calcular o mês seguinte.
        /// </summary>
        /// <param name="valorInicial">Valor inicial do investimento</param>
        /// <param name="meses">Número de meses para aplicar o cálculo</param>
        /// <returns>Valor final do investimento após os meses especificados</returns>
        public decimal CalcularValorFinal(decimal valorInicial, int meses)
        {
            if (valorInicial <= 0)
                throw new ArgumentException("Valor inicial deve ser positivo", nameof(valorInicial));

            if (meses <= 1)
                throw new ArgumentException("Prazo deve ser maior que 1 mês", nameof(meses));

            decimal fatorMensal = 1 + (CDI * TB);
            decimal resultado = valorInicial;

            for (int i = 0; i < meses; i++)
            {
                resultado *= fatorMensal;
            }

            return resultado;
        }

        /// <summary>
        /// Calcula o rendimento total obtido no investimento em CDB.
        /// </summary>
        /// <param name="valorInicial">Valor inicial do investimento</param>
        /// <param name="meses">Número de meses para aplicar o cálculo</param>
        /// <returns>Rendimento total (Valor Final - Valor Inicial)</returns>
        public decimal CalcularRendimento(decimal valorInicial, int meses)
        {
            decimal valorFinal = CalcularValorFinal(valorInicial, meses);
            return valorFinal - valorInicial;
        }

        /// <summary>
        /// Obtém a alíquota de imposto (IR) baseada no número de meses.
        /// Tabela progressiva:
        /// - Até 6 meses: 22,5%
        /// - Até 12 meses: 20%
        /// - Até 24 meses: 17,5%
        /// - Acima de 24 meses: 15%
        /// </summary>
        /// <param name="meses">Número de meses do investimento</param>
        /// <returns>Alíquota de imposto em decimal (ex: 0.225 para 22,5%)</returns>
        public decimal ObtenerAliquotaImposto(int meses)
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
        /// Calcula o valor do imposto (IR) sobre o rendimento do CDB.
        /// </summary>
        /// <param name="valorInicial">Valor inicial do investimento</param>
        /// <param name="meses">Número de meses para aplicar o cálculo</param>
        /// <returns>Valor do imposto em reais</returns>
        public decimal CalcularImposto(decimal valorInicial, int meses)
        {
            decimal rendimento = CalcularRendimento(valorInicial, meses);
            decimal aliquota = ObtenerAliquotaImposto(meses);
            return rendimento * aliquota;
        }

        /// <summary>
        /// Calcula o valor líquido final (após dedução do imposto) do investimento em CDB.
        /// </summary>
        /// <param name="valorInicial">Valor inicial do investimento</param>
        /// <param name="meses">Número de meses para aplicar o cálculo</param>
        /// <returns>Valor final líquido (Valor Final - Imposto)</returns>
        public decimal CalcularValorLiquido(decimal valorInicial, int meses)
        {
            decimal valorFinal = CalcularValorFinal(valorInicial, meses);
            decimal imposto = CalcularImposto(valorInicial, meses);
            return valorFinal - imposto;
        }
    }
}
