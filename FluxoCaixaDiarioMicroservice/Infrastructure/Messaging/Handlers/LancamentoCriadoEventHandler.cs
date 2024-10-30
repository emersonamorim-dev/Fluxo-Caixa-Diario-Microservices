using FluxoCaixaDiarioMicroservice.Infrastructure.Messaging.Events;
using MediatR;

namespace FluxoCaixaDiarioMicroservice.Infrastructure.Messaging.Handlers
{
    public class LancamentoCriadoEventHandler : INotificationHandler<LancamentoCriadoEvent>
    {
        private readonly ILogger<LancamentoCriadoEventHandler> _logger;

        public LancamentoCriadoEventHandler(ILogger<LancamentoCriadoEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(LancamentoCriadoEvent notification, CancellationToken cancellationToken)
        {
            // Log para indicar que o evento foi tratado
            _logger.LogInformation("Lançamento criado: Id={LancamentoId}, Valor={Valor}, Data={Data}",
                notification.LancamentoId, notification.Valor, notification.Data);

            return Task.CompletedTask;
        }
    }
}
