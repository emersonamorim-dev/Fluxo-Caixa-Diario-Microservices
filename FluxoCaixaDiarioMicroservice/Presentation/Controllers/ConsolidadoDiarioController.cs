using FluxoCaixaDiarioMicroservice.Application.Requests;
using FluxoCaixaDiarioMicroservice.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FluxoCaixaDiarioMicroservice.Presentation.Controllers
{
    [ApiController]
    [Route("api/consolidados/")]
    public class ConsolidadoDiarioController : ControllerBase
    {
        private readonly IConsolidadoDiarioService _consolidadoDiarioService;

        public ConsolidadoDiarioController(IConsolidadoDiarioService consolidadoDiarioService)
        {
            _consolidadoDiarioService = consolidadoDiarioService ?? throw new ArgumentNullException(nameof(consolidadoDiarioService));
        }

        [HttpPost("consolidar")]
        public async Task<IActionResult> ConsolidarSaldoDiario([FromQuery] DateTime? data, [FromBody] ConsolidarSaldoRequest? request = null)
        {
            try
            {
                var dataProcessada = request?.Data ?? data ?? throw new ArgumentException("A data fornecida é inválida. Certifique-se de enviar uma data válida.");

                await _consolidadoDiarioService.ConsolidarSaldoDiarioAsync(dataProcessada);
                return Ok($"Saldo diário consolidado com sucesso para a data: {dataProcessada:yyyy-MM-dd}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao consolidar saldo diário: {ex.Message}");
            }
        }

        [HttpPost("consolidado")]
        public async Task<IActionResult> ConsolidadoSaldoDiario([FromQuery] DateTime? data, [FromBody] ConsolidarSaldoRequest? request = null)
        {
            try
            {
                var dataProcessada = request?.Data ?? data ?? throw new ArgumentException("A data fornecida é inválida. Certifique-se de enviar uma data válida.");

                var saldoConsolidado = await _consolidadoDiarioService.ObterSaldoConsolidadoAsync(dataProcessada);

                if (saldoConsolidado == null)
                {
                    return NotFound($"Nenhum saldo consolidado encontrado para a data: {dataProcessada:yyyy-MM-dd}");
                }

                return Ok(new
                {
                    data = dataProcessada.ToString("yyyy-MM-dd"),
                    total_creditos = saldoConsolidado.TotalCreditos,
                    total_debitos = saldoConsolidado.TotalDebitos,
                    saldo_consolidado = saldoConsolidado.SaldoFinal
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter saldo consolidado: {ex.Message}");
            }
        }
    }
}

