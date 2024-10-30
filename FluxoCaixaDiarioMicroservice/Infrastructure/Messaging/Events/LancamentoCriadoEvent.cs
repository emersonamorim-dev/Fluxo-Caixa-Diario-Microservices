using MediatR;

namespace FluxoCaixaDiarioMicroservice.Infrastructure.Messaging.Events
{
    public class LancamentoCriadoEvent : INotification
    {
        public Guid LancamentoId { get; }
        public decimal Valor { get; }
        public string Tipo { get; }
        public DateTime Data { get; }

        public LancamentoCriadoEvent(Guid lancamentoId, decimal valor, string tipo, DateTime data)
        {
            LancamentoId = lancamentoId;
            Valor = valor;
            Tipo = tipo;
            Data = data;
        }
    }
}