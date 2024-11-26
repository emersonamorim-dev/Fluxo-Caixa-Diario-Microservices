using System;

namespace FluxoCaixaDiarioMicroservice.Presentation.Exceptions
{
    public class LancamentoNotFoundException : Exception
    {
        public LancamentoNotFoundException() : base("Lançamento não encontrado.")
        {
        }

        public LancamentoNotFoundException(string message) : base(message)
        {
        }

        public LancamentoNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public LancamentoNotFoundException(Guid id) : base($"Lançamento com ID {id} não foi encontrado.")
        {
        }
    }
}
