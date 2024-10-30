using FluxoCaixaDiarioMicroservice.Infrastructure.Db;
using FluxoCaixaDiarioMicroservice.Domain.Entities;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace FluxoCaixaDiarioMicroservice.Tests.Infrastructure.Db
{
    public class MongoDbContextTests
    {
        private readonly Mock<IMongoClient> _mockClient;
        private readonly Mock<IMongoDatabase> _mockDatabase;
        private readonly Mock<IMongoCollection<Lancamento>> _mockLancamentoCollection;
        private readonly Mock<IMongoCollection<ConsolidadoDiario>> _mockConsolidadoCollection;
        private readonly MongoDbContext _dbContext;

        public MongoDbContextTests()
        {
            // Arrange
            _mockClient = new Mock<IMongoClient>();
            _mockDatabase = new Mock<IMongoDatabase>();
            _mockLancamentoCollection = new Mock<IMongoCollection<Lancamento>>();
            _mockConsolidadoCollection = new Mock<IMongoCollection<ConsolidadoDiario>>();

            _mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), null))
                       .Returns(_mockDatabase.Object);

            _mockDatabase.Setup(d => d.GetCollection<Lancamento>("lancamentos", null))
                         .Returns(_mockLancamentoCollection.Object);

            _mockDatabase.Setup(d => d.GetCollection<ConsolidadoDiario>("consolidados", null))
                         .Returns(_mockConsolidadoCollection.Object);

            _dbContext = new MongoDbContext("mongodb://localhost:27017", "TestDatabase");
        }

        [Fact]
        public void Lancamentos_ShouldReturnLancamentoCollection()
        {
            // Act
            var collection = _dbContext.Lancamentos;

            // Assert
            Assert.NotNull(collection);
            Assert.IsAssignableFrom<IMongoCollection<Lancamento>>(collection);
        }

        [Fact]
        public void ConsolidadosDiarios_ShouldReturnConsolidadoDiarioCollection()
        {
            // Act
            var collection = _dbContext.ConsolidadosDiarios;

            // Assert
            Assert.NotNull(collection);
            Assert.IsAssignableFrom<IMongoCollection<ConsolidadoDiario>>(collection);
        }
    }
}
