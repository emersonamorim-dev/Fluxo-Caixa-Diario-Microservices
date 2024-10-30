using FluxoCaixaDiarioMicroservice.Domain.Entities;
using FluxoCaixaDiarioMicroservice.Domain.Interfaces;
using FluxoCaixaDiarioMicroservice.Domain.Repositories;
using FluxoCaixaDiarioMicroservice.Infrastructure.Messaging.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FluxoCaixaDiarioMicroservice.Application.UseCases
{
    public class AdicionarLancamentoUseCase : IAdicionarLancamentoUseCase
    {
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly IMediator _mediator;
        private readonly ILogger<AdicionarLancamentoUseCase> _logger;

        public AdicionarLancamentoUseCase(ILancamentoRepository lancamentoRepository, IMediator mediator, ILogger<AdicionarLancamentoUseCase> logger)
        {
            _lancamentoRepository = lancamentoRepository ?? throw new ArgumentNullException(nameof(lancamentoRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ExecuteAsync(Lancamento lancamento)
        {
            try
            {
                if (lancamento == null)
                {
                    throw new ArgumentNullException(nameof(lancamento), "O lançamento não pode ser nulo.");
                }

                if (lancamento.Valor <= 0)
                {
                    throw new ArgumentException("O valor do lançamento deve ser maior que zero.", nameof(lancamento));
                }

                _logger.LogInformation("Adicionando lançamento no banco de dados.");
                await _lancamentoRepository.AdicionarLancamentoAsync(lancamento);

                // Publica o evento de lançamento criado
                _logger.LogInformation("Publicando evento de lançamento criado.");
                var evento = new LancamentoCriadoEvent(lancamento.Id, lancamento.Valor, lancamento.Tipo, lancamento.Data);
                await _mediator.Publish(evento);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Erro ao adicionar lançamento: {Mensagem}", ex.Message);
                throw;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Erro ao adicionar lançamento: {Mensagem}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao adicionar lançamento: {Mensagem}", ex.Message);
                throw;
            }
        }
    }
}
