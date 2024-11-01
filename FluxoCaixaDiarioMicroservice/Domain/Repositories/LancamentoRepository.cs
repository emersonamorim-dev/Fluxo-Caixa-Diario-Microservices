using FluxoCaixaDiarioMicroservice.Domain.Entities;
using FluxoCaixaDiarioMicroservice.Domain.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluxoCaixaDiarioMicroservice.Domain.Repositories
{
    public class LancamentoRepository : ILancamentoRepository
    {
        private readonly IMongoCollection<Lancamento> _lancamentosCollection;

        public LancamentoRepository(IMongoDatabase database)
        {
            _lancamentosCollection = database.GetCollection<Lancamento>("lancamentos");
        }

        public async Task AdicionarLancamentoAsync(Lancamento lancamento)
        {
            if (lancamento.Id == Guid.Empty)
            {
                lancamento.Id = Guid.NewGuid();
            }
            await _lancamentosCollection.InsertOneAsync(lancamento);
        }

        public async Task<Lancamento> ObterLancamentoPorIdAsync(Guid id)
        {
            return await _lancamentosCollection.Find(l => l.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Lancamento>> ObterLancamentosPorDataAsync(DateTime data)
        {
            var start = data.Date;
            var end = start.AddDays(1);
            var filter = Builders<Lancamento>.Filter.Gte(l => l.Data, start) & Builders<Lancamento>.Filter.Lt(l => l.Data, end);
            return await _lancamentosCollection.Find(filter).ToListAsync();
        }

        // obtém todos os lançamentos
        public async Task<IEnumerable<Lancamento>> ObterTodosLancamentosAsync()
        {
            return await _lancamentosCollection.Find(_ => true).ToListAsync();
        }
    }
}
