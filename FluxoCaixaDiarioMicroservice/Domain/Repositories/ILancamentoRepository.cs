using FluxoCaixaDiarioMicroservice.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluxoCaixaDiarioMicroservice.Domain.Interfaces
{
    public interface ILancamentoRepository
    {
        Task AdicionarLancamentoAsync(Lancamento lancamento);
        Task<Lancamento> ObterLancamentoPorIdAsync(Guid id);
        Task<IEnumerable<Lancamento>> ObterLancamentosPorDataAsync(DateTime data);
        Task<IEnumerable<Lancamento>> ObterTodosLancamentosAsync();
    }
}
