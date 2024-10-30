using MediatR;

namespace FluxoCaixaDiarioMicroservice.Infrastructure.Messaging.Events
{
    public class ConsolidadoDiarioCriadoEvent : INotification
    {
        public DateTime Data { get; }
        public decimal SaldoConsolidado { get; }

        public ConsolidadoDiarioCriadoEvent(DateTime data, decimal saldoConsolidado)
        {
            Data = data;
            SaldoConsolidado = saldoConsolidado;
        }
    }
}
