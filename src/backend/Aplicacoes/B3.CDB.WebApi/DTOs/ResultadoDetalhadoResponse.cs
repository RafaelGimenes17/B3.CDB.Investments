namespace B3.CDB.WebApi.DTOs
{
    /// <summary>
    /// Detalhe de um resultado de investimento, contendo o valor total e o rendimento.
    /// </summary>
    public class ResultadoDetalhadoResponse
    {
        /// <summary>
        /// Valor total do investimento ao final do período em reais.
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Rendimento obtido no período em reais (Valor Final - Valor Inicial).
        /// </summary>
        public decimal Rendimento { get; set; }
    }
}
