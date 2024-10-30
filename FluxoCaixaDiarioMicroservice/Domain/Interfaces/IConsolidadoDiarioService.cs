using FluxoCaixaDiarioMicroservice.Domain.Entities;

namespace FluxoCaixaDiarioMicroservice.Domain.Interfaces
{
    public interface IConsolidadoDiarioService
    {
        Task ConsolidarSaldoDiarioAsync(DateTime data);
        Task<SaldoConsolidado> ObterSaldoConsolidadoAsync(DateTime data);

        
    }
}
