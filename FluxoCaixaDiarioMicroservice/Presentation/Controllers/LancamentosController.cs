using FluentValidation;
using FluxoCaixaDiarioMicroservice.Domain.Entities;
using FluxoCaixaDiarioMicroservice.Domain.Interfaces;
using FluxoCaixaDiarioMicroservice.Presentation.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FluxoCaixaDiarioMicroservice.Presentation.Controllers
{
    [ApiController]
    [Route("api/lancamentos")]
    [Produces("application/json")]
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AdicionarLancamento([FromBody] Lancamento lancamento)
        {
            if (lancamento == null)
            {
                throw new ArgumentNullException(nameof(lancamento), "Dados do lançamento não podem estar vazios.");
            }

            _logger.LogInformation("Recebendo solicitação para adicionar um novo lançamento.");

            var validationResult = await _lancamentoValidator.ValidateAsync(lancamento);
            if (!validationResult.IsValid)
            {
                throw new InvalidOperationException("Dados do lançamento inválidos: " + string.Join(", ", validationResult.Errors));
            }

            await _lancamentoService.AdicionarLancamentoAsync(lancamento);
            _logger.LogInformation("Lançamento {Id} adicionado com sucesso.", lancamento.Id);
            
            return CreatedAtAction(nameof(ObterLancamentoPorId), new { id = lancamento.Id }, lancamento);
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterLancamentos()
        {
            _logger.LogInformation("Recebendo solicitação para listar lançamentos.");
            var lancamentos = await _lancamentoService.ObterTodosLancamentosAsync();
            return Ok(lancamentos);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterLancamentoPorId(Guid id)
        {
            _logger.LogInformation("Recebendo solicitação para obter lançamento por ID: {Id}", id);

            var lancamento = await _lancamentoService.ObterLancamentoPorIdAsync(id);
            if (lancamento == null)
            {
                throw new LancamentoNotFoundException(id);
            }

            return Ok(lancamento);
        }
    }
}

