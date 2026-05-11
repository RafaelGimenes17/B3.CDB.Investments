using B3.CDB.WebApi.DTOs;

namespace B3.CDB.WebApi.Services
{
    /// <summary>
    /// Define o contrato do serviço de cálculo de investimentos em CDB.
    /// </summary>
    public interface ICDBService
    {
        /// <summary>
        /// Calcula o investimento em CDB e retorna o resultado completo com valores bruto, líquido, imposto e alíquota.
        /// </summary>
        /// <param name="valorInicial">Valor inicial do investimento.</param>
        /// <param name="meses">Número de meses do investimento.</param>
        /// <returns>Resultado detalhado do investimento.</returns>
        CDBResultadoResponse Calcular(decimal valorInicial, int meses);
    }
}
