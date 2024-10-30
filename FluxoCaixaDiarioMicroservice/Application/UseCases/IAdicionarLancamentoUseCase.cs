using FluxoCaixaDiarioMicroservice.Domain.Entities;

namespace FluxoCaixaDiarioMicroservice.Application.UseCases
{
    public interface IAdicionarLancamentoUseCase
    {
        Task ExecuteAsync(Lancamento lancamento);
    }

}
