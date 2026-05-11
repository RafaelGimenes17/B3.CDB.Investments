using Microsoft.AspNetCore.Mvc;
using B3.CDB.WebApi.Services;
using System.ComponentModel.DataAnnotations;

namespace B3.CDB.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CDBController : ControllerBase
    {
        private readonly CDBCalculator _cdbCalculator;

        public CDBController(CDBCalculator cdbCalculator)
        {
            _cdbCalculator = cdbCalculator ?? throw new ArgumentNullException(nameof(cdbCalculator));
        }

        /// <summary>
        /// Calcula o valor final de um investimento em CDB
        /// </summary>
        /// <param name="request">Dados do investimento com valor inicial e número de meses</param>
        /// <returns>Valor final do investimento</returns>
        [HttpPost("calcular-valor-final")]
        public ActionResult<decimal> CalcularValorFinal([FromBody] CalculoCDBRequest request)
        {
            return ExecutarComValidacao(request, () => 
                _cdbCalculator.CalcularValorFinal(request.ValorInicial, request.Meses));
        }

        /// <summary>
        /// Calcula o rendimento de um investimento em CDB
        /// </summary>
        /// <param name="request">Dados do investimento com valor inicial e número de meses</param>
        /// <returns>Rendimento total</returns>
        [HttpPost("calcular-rendimento")]
        public ActionResult<decimal> CalcularRendimento([FromBody] CalculoCDBRequest request)
        {
            return ExecutarComValidacao(request, () => 
                _cdbCalculator.CalcularRendimento(request.ValorInicial, request.Meses));
        }

        /// <summary>
        /// Calcula o imposto sobre o rendimento de um investimento em CDB
        /// </summary>
        /// <param name="request">Dados do investimento com valor inicial e número de meses</param>
        /// <returns>Valor do imposto</returns>
        [HttpPost("calcular-imposto")]
        public ActionResult<decimal> CalcularImposto([FromBody] CalculoCDBRequest request)
        {
            return ExecutarComValidacao(request, () => 
                _cdbCalculator.CalcularImposto(request.ValorInicial, request.Meses));
        }

        /// <summary>
        /// Calcula o valor líquido final de um investimento em CDB
        /// </summary>
        /// <param name="request">Dados do investimento com valor inicial e número de meses</param>
        /// <returns>Valor final líquido após dedução do imposto</returns>
        [HttpPost("calcular-valor-liquido")]
        public ActionResult<decimal> CalcularValorLiquido([FromBody] CalculoCDBRequest request)
        {
            return ExecutarComValidacao(request, () => 
                _cdbCalculator.CalcularValorLiquido(request.ValorInicial, request.Meses));
        }

        /// <summary>
        /// Obtém a alíquota de imposto para um período específico
        /// </summary>
        /// <param name="meses">Número de meses do investimento</param>
        /// <returns>Alíquota de imposto em decimal</returns>
        [HttpGet("aliquota-imposto/{meses}")]
        public ActionResult<decimal> ObtenerAliquotaImposto(int meses)
        {
            try
            {
                decimal resultado = _cdbCalculator.ObtenerAliquotaImposto(meses);
                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        private ActionResult<T> ExecutarComValidacao<T>(CalculoCDBRequest request, Func<T> operacao)
        {
            if (request is null)
            {
                return BadRequest(new { mensagem = "Dados do investimento são obrigatórios" });
            }

            try
            {
                var resultado = operacao();
                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Calcula o investimento em CDB retornando resultado bruto e líquido
        /// </summary>
        /// <param name="request">Dados do investimento com valor inicial e número de meses</param>
        /// <returns>Resultado detalhado do investimento com valores bruto e líquido</returns>
        [HttpPost("calcular")]
        public ActionResult<CDBResultadoResponse> Calcular([FromBody] CalculoCDBRequest request)
        {
            return ExecutarComValidacao(request, () => 
            {
                decimal valorFinal = _cdbCalculator.CalcularValorFinal(request.ValorInicial, request.Meses);
                decimal rendimento = _cdbCalculator.CalcularRendimento(request.ValorInicial, request.Meses);
                decimal imposto = _cdbCalculator.CalcularImposto(request.ValorInicial, request.Meses);
                decimal valorLiquido = _cdbCalculator.CalcularValorLiquido(request.ValorInicial, request.Meses);
                decimal aliquota = _cdbCalculator.ObtenerAliquotaImposto(request.Meses);

                return new CDBResultadoResponse
                {
                    ValorInicial = request.ValorInicial,
                    Meses = request.Meses,
                    Aliquota = aliquota * 100,
                    ResultadoBruto = new ResultadoDetalhadoResponse
                    {
                        Valor = valorFinal,
                        Rendimento = rendimento
                    },
                    ResultadoLiquido = new ResultadoDetalhadoResponse
                    {
                        Valor = valorLiquido,
                        Rendimento = rendimento - imposto
                    },
                    Imposto = imposto
                };
            });
        }
    }

    public class CalculoCDBRequest
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor inicial deve ser maior que zero")]
        public decimal ValorInicial { get; set; }

        [Range(2, int.MaxValue, ErrorMessage = "Prazo deve ser maior que 1 mês")]
        public int Meses { get; set; }
    }

    public class ResultadoDetalhadoResponse
    {
        public decimal Valor { get; set; }
        public decimal Rendimento { get; set; }
    }

    public class CDBResultadoResponse
    {
        public decimal ValorInicial { get; set; }
        public int Meses { get; set; }
        public decimal Aliquota { get; set; }
        public ResultadoDetalhadoResponse ResultadoBruto { get; set; } = new();
        public ResultadoDetalhadoResponse ResultadoLiquido { get; set; } = new();
        public decimal Imposto { get; set; }
    }
}
