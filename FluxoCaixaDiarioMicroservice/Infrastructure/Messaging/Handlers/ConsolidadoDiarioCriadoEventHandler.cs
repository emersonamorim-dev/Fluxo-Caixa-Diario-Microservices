using FluxoCaixaDiarioMicroservice.Infrastructure.Messaging.Events;
using MediatR;

namespace FluxoCaixaDiarioMicroservice.Infrastructure.Messaging.Handlers
{
    public class ConsolidadoDiarioCriadoEventHandler : INotificationHandler<ConsolidadoDiarioCriadoEvent>
    {
        private readonly ILogger<ConsolidadoDiarioCriadoEventHandler> _logger;

        public ConsolidadoDiarioCriadoEventHandler(ILogger<ConsolidadoDiarioCriadoEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(ConsolidadoDiarioCriadoEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Saldo consolidado criado para a data: {Data}, Saldo: {SaldoConsolidado}",
                notification.Data, notification.SaldoConsolidado);

            // Simula alguma operação assíncrona que precise ser feita
            await Task.CompletedTask;
        }
    }
}
