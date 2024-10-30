using FluxoCaixaDiarioMicroservice.Domain.Entities;

namespace FluxoCaixaDiarioMicroservice.Application.UseCases
{
    public interface IConsolidarSaldoDiarioUseCase
    {
        Task<SaldoConsolidado> ExecuteAsync(DateTime data);
    }
}
