using FluxoCaixaDiarioMicroservice.Domain.Entities;

namespace FluxoCaixaDiarioMicroservice.Domain.Repositories
{
    public interface IConsolidadoDiarioRepository
    {
        Task AdicionarConsolidadoAsync(SaldoConsolidado consolidado);
        Task<SaldoConsolidado> ObterConsolidadoPorDataAsync(DateTime data);
    }
}
