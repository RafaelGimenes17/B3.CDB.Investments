using B3.CDB.WebApi.Services;
using B3.CDB.WebApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace B3.CDB.WebApi.Controllers
{
    /// <summary>
    /// Controller responsável pelo cálculo de investimentos em CDB.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CDBController : ControllerBase
    {
        private readonly ICDBService _cdbService;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="CDBController"/>.
        /// </summary>
        /// <param name="cdbService">Serviço de cálculo de CDB.</param>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="cdbService"/> é nulo.</exception>
        public CDBController(ICDBService cdbService)
        {
            _cdbService = cdbService ?? throw new ArgumentNullException(nameof(cdbService));
        }

        /// <summary>
        /// Calcula o investimento em CDB retornando resultado bruto e líquido
        /// </summary>
        /// <param name="request">Dados do investimento com valor inicial e número de meses</param>
        /// <returns>Resultado detalhado do investimento com valores bruto e líquido</returns>
        [HttpPost("calcular")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CDBResultadoResponse> Calcular([FromBody] CalculoCDBRequest request)
        {
            if (!ModelState.IsValid || request == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var resultado = _cdbService.Calcular(request.ValorInicial, request.Meses);
                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}
