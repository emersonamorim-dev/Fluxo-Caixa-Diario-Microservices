using FluxoCaixaDiarioMicroservice.Domain.Entities;

namespace FluxoCaixaDiarioMicroservice.Domain.Interfaces
{
    public interface ILancamentoService
    {
        Task AdicionarLancamentoAsync(Lancamento lancamento);
        Task<IEnumerable<Lancamento>> ObterTodosLancamentosAsync();
        Task<IEnumerable<Lancamento>> ObterLancamentosPorDataAsync(DateTime data);
        Task<Lancamento> ObterLancamentoPorIdAsync(Guid lancamentoId); 
    }
}
