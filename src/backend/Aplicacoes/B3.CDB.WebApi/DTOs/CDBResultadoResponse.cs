namespace B3.CDB.WebApi.DTOs
{
    /// <summary>
    /// Resposta completa do cálculo de investimento em CDB, contendo resultados bruto e líquido.
    /// </summary>
    public class CDBResultadoResponse
    {
        /// <summary>
        /// Valor inicial do investimento em reais.
        /// </summary>
        public decimal ValorInicial { get; set; }

        /// <summary>
        /// Prazo do investimento em meses.
        /// </summary>
        public int Meses { get; set; }

        /// <summary>
        /// Alíquota de Imposto de Renda aplicada, em percentual (ex: 20 para 20%).
        /// </summary>
        public decimal Aliquota { get; set; }

        /// <summary>
        /// Resultado bruto do investimento (antes do desconto do IR).
        /// </summary>
        public ResultadoDetalhadoResponse ResultadoBruto { get; set; } = new();

        /// <summary>
        /// Resultado líquido do investimento (após o desconto do IR).
        /// </summary>
        public ResultadoDetalhadoResponse ResultadoLiquido { get; set; } = new();

        /// <summary>
        /// Valor do Imposto de Renda descontado em reais.
        /// </summary>
        public decimal Imposto { get; set; }
    }
}
