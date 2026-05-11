using System.ComponentModel.DataAnnotations;

namespace B3.CDB.WebApi.DTOs
{
    /// <summary>
    /// Dados de entrada para o cálculo de investimento em CDB.
    /// </summary>
    public class CalculoCDBRequest
    {
        /// <summary>
        /// Valor inicial do investimento em reais. Deve ser maior que zero.
        /// </summary>
        [Required(ErrorMessage = "O corpo da requisição não pode ser nulo.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor inicial deve ser maior que zero.")]
        public decimal ValorInicial { get; set; }

        /// <summary>
        /// Prazo do investimento em meses. Deve ser maior que 1.
        /// </summary>
        [Range(2, int.MaxValue, ErrorMessage = "O prazo deve ser maior que 1 mês.")]
        public int Meses { get; set; }
    }
}
