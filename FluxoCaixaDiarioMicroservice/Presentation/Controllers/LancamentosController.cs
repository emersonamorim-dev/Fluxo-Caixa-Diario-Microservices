using FluentValidation;
using FluxoCaixaDiarioMicroservice.Domain.Entities;
using FluxoCaixaDiarioMicroservice.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FluxoCaixaDiarioMicroservice.Presentation.Controllers
{
    [ApiController]
    [Route("api/lancamentos")]
    public class LancamentosController : ControllerBase
    {
        private readonly ILancamentoService _lancamentoService;
        private readonly IValidator<Lancamento> _lancamentoValidator;
        private readonly ILogger<LancamentosController> _logger;

        public LancamentosController(ILancamentoService lancamentoService, IValidator<Lancamento> lancamentoValidator, ILogger<LancamentosController> logger)
        {
            _lancamentoService = lancamentoService ?? throw new ArgumentNullException(nameof(lancamentoService));
            _lancamentoValidator = lancamentoValidator ?? throw new ArgumentNullException(nameof(lancamentoValidator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarLancamento([FromBody] Lancamento lancamento)
        {
            if (lancamento == null)
            {
                _logger.LogWarning("O lançamento enviado está nulo.");
                return BadRequest("Dados do lançamento inválidos.");
            }

            _logger.LogInformation("Recebendo solicitação para adicionar um novo lançamento.");

            var validationResult = await _lancamentoValidator.ValidateAsync(lancamento);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validação falhou para o lançamento: {Errors}", validationResult.Errors);
                return BadRequest(validationResult.Errors);
            }

            try
            {
                await _lancamentoService.AdicionarLancamentoAsync(lancamento);
                _logger.LogInformation("Lançamento adicionado com sucesso.");
                return CreatedAtAction(nameof(ObterLancamentoPorId), new { id = lancamento.Id }, lancamento);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar lançamento.");
                return StatusCode(500, "Erro interno do servidor. Chave ID duplicada.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObterLancamentos()
        {
            _logger.LogInformation("Recebendo solicitação para listar lançamentos.");

            try
            {
                var lancamentos = await _lancamentoService.ObterTodosLancamentosAsync();
                return Ok(lancamentos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar lançamentos.");
                return StatusCode(500, "Erro interno do servidor. Por favor, tente novamente mais tarde.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterLancamentoPorId(Guid id)
        {
            _logger.LogInformation("Recebendo solicitação para obter lançamento por ID: {Id}", id);

            try
            {
                var lancamento = await _lancamentoService.ObterLancamentoPorIdAsync(id);
                if (lancamento == null)
                {
                    return NotFound("Lançamento não encontrado.");
                }
                return Ok(lancamento);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lançamento por ID.");
                return StatusCode(500, "Erro interno do servidor. Por favor, tente novamente mais tarde.");
            }
        }
    }
}
