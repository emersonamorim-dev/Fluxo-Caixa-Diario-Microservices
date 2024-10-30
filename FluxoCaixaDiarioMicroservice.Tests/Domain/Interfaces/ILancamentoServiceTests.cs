using FluxoCaixaDiarioMicroservice.Domain.Entities;
using FluxoCaixaDiarioMicroservice.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FluxoCaixaDiarioMicroservice.Tests.Domain.Interfaces
{
    public class ILancamentoServiceTests
    {
        private readonly Mock<ILancamentoService> _mockLancamentoService;

        public ILancamentoServiceTests()
        {
            _mockLancamentoService = new Mock<ILancamentoService>();
        }

        [Fact]
        public async Task ObterLancamentoPorIdAsync_DeveRetornarLancamento_CasoExista()
        {
            // Arrange
            var lancamentoId = Guid.NewGuid();
            var expectedLancamento = new Lancamento
            {
                Id = lancamentoId,
                Valor = 1000,
                Tipo = "Crédito",
                Data = DateTime.UtcNow
            };

            _mockLancamentoService.Setup(service => service.ObterLancamentoPorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedLancamento);

            // Act
            var lancamento = await _mockLancamentoService.Object.ObterLancamentoPorIdAsync(lancamentoId);

            // Assert
            Assert.NotNull(lancamento);
            Assert.Equal(expectedLancamento.Id, lancamento.Id);
            Assert.Equal(expectedLancamento.Valor, lancamento.Valor);
            Assert.Equal(expectedLancamento.Tipo, lancamento.Tipo);
            Assert.Equal(expectedLancamento.Data, lancamento.Data);
        }
    }
}
