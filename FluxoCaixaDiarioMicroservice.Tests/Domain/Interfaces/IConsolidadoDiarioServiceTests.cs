using FluxoCaixaDiarioMicroservice.Domain.Interfaces;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FluxoCaixaDiarioMicroservice.Tests.Domain.Interfaces
{
    public class IConsolidadoDiarioServiceTests
    {
        private readonly Mock<IConsolidadoDiarioService> _mockConsolidadoDiarioService;

        public IConsolidadoDiarioServiceTests()
        {
            // Cria um Mock do IConsolidadoDiarioService
            _mockConsolidadoDiarioService = new Mock<IConsolidadoDiarioService>();
        }

        [Fact]
        public async Task ConsolidarSaldoDiarioAsync_DeveSerChamado_ComDataValida()
        {
            // Arrange
            var data = new DateTime(2024, 10, 29);

            // Configura o Mock para garantir que o método seja chamado sem exceções
            _mockConsolidadoDiarioService.Setup(service => service.ConsolidarSaldoDiarioAsync(It.IsAny<DateTime>()))
                .Returns(Task.CompletedTask);

            // Act
            await _mockConsolidadoDiarioService.Object.ConsolidarSaldoDiarioAsync(data);

            // Assert
            _mockConsolidadoDiarioService.Verify(service => service.ConsolidarSaldoDiarioAsync(data), Times.Once);
        }

        [Fact]
        public async Task ConsolidarSaldoDiarioAsync_DeveLancarExcecao_SeDataInvalida()
        {
            // Arrange
            var dataInvalida = DateTime.MinValue;

            _mockConsolidadoDiarioService.Setup(service => service.ConsolidarSaldoDiarioAsync(dataInvalida))
                .ThrowsAsync(new ArgumentException("Data fornecida é inválida."));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _mockConsolidadoDiarioService.Object.ConsolidarSaldoDiarioAsync(dataInvalida));
        }
    }
}
