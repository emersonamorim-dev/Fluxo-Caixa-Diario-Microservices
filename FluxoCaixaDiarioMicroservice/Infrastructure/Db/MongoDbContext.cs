using FluxoCaixaDiarioMicroservice.Domain.Entities;
using MongoDB.Driver;

namespace FluxoCaixaDiarioMicroservice.Infrastructure.Db
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Lancamento> Lancamentos => _database.GetCollection<Lancamento>("lancamentos");
        public IMongoCollection<ConsolidadoDiario> ConsolidadosDiarios => _database.GetCollection<ConsolidadoDiario>("consolidados");
    }
}
