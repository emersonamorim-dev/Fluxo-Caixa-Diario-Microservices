using FluxoCaixaDiarioMicroservice.Domain.Entities;
using MongoDB.Driver;

namespace FluxoCaixaDiarioMicroservice.Domain.Repositories
{
    public class ConsolidadoDiarioRepository : IConsolidadoDiarioRepository
    {
        private readonly IMongoCollection<SaldoConsolidado> _consolidadoCollection;

        public ConsolidadoDiarioRepository(IMongoDatabase database)
        {
            _consolidadoCollection = database.GetCollection<SaldoConsolidado>("ConsolidadosDiarios");
        }

        public async Task AdicionarConsolidadoAsync(SaldoConsolidado consolidado)
        {
            await _consolidadoCollection.InsertOneAsync(consolidado);
        }

        public async Task<SaldoConsolidado> ObterConsolidadoPorDataAsync(DateTime data)
        {
            var filter = Builders<SaldoConsolidado>.Filter.Eq(c => c.Data.Date, data.Date);
            return await _consolidadoCollection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
