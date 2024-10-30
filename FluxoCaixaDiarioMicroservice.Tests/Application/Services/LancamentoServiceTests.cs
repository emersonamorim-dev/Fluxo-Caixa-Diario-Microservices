using FluxoCaixaDiarioMicroservice.Application.Services;
using FluxoCaixaDiarioMicroservice.Application.UseCases;
using FluxoCaixaDiarioMicroservice.Domain.Entities;
using FluxoCaixaDiarioMicroservice.Domain.Interfaces;
using FluxoCaixaDiarioMicroservice.Domain.Repositories;
using FluxoCaixaDiarioMicroservice.Infrastructure.Messaging.Events;
using FluxoCaixaDiarioMicroservice.Infrastructure.Redis;
using MediatR;
using Moq;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FluxoCaixaDiarioMicroservice.Tests.Application.Services
{
    public class LancamentoServiceTests
    {
        private readonly Mock<IAdicionarLancamentoUseCase> _mockAdicionarLancamentoUseCase;
        private readonly Mock<IRedisCacheService> _mockRedisCacheService;
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<IConnection> _mockRabbitMqConnection;
        private readonly Mock<ILancamentoRepository> _mockLancamentoRepository;
        private readonly LancamentoService _lancamentoService;

        public LancamentoServiceTests()
        {
            _mockAdicionarLancamentoUseCase = new Mock<IAdicionarLancamentoUseCase>();
            _mockRedisCacheService = new Mock<IRedisCacheService>();
            _mockMediator = new Mock<IMediator>();
            _mockRabbitMqConnection = new Mock<IConnection>();
            _mockLancamentoRepository = new Mock<ILancamentoRepository>();

            _lancamentoService = new LancamentoService(
                _mockAdicionarLancamentoUseCase.Object,
                _mockRedisCacheService.Object,
                _mockMediator.Object,
                _mockRabbitMqConnection.Object,
                _mockLancamentoRepository.Object);
        }


        [Fact]
        public async Task ObterTodosLancamentosAsync_ShouldReturnAllLancamentos()
        {
            // Arrange
            var lancamentos = new List<Lancamento>
            {
                new Lancamento { Id = Guid.NewGuid(), Valor = 100, Tipo = "Crédito", Data = DateTime.UtcNow },
                new Lancamento { Id = Guid.NewGuid(), Valor = 200, Tipo = "Débito", Data = DateTime.UtcNow }
            };

            _mockLancamentoRepository.Setup(x => x.ObterTodosLancamentosAsync()).ReturnsAsync(lancamentos);

            // Act
            var result = await _lancamentoService.ObterTodosLancamentosAsync();

            // Assert
            Assert.Equal(lancamentos, result);
            _mockLancamentoRepository.Verify(x => x.ObterTodosLancamentosAsync(), Times.Once);
        }

        [Fact]
        public async Task ObterLancamentosPorDataAsync_ShouldReturnLancamentosByDate()
        {
            // Arrange
            var data = new DateTime(2024, 10, 30);
            var lancamentos = new List<Lancamento>
            {
                new Lancamento { Id = Guid.NewGuid(), Valor = 100, Tipo = "Crédito", Data = data }
            };

            _mockLancamentoRepository.Setup(x => x.ObterLancamentosPorDataAsync(data)).ReturnsAsync(lancamentos);

            // Act
            var result = await _lancamentoService.ObterLancamentosPorDataAsync(data);

            // Assert
            Assert.Equal(lancamentos, result);
            _mockLancamentoRepository.Verify(x => x.ObterLancamentosPorDataAsync(data), Times.Once);
        }

        [Fact]
        public async Task ObterLancamentoPorIdAsync_ShouldReturnLancamentoById()
        {
            // Arrange
            var lancamentoId = Guid.NewGuid();
            var lancamento = new Lancamento { Id = lancamentoId, Valor = 100, Tipo = "Crédito", Data = DateTime.UtcNow };

            _mockLancamentoRepository.Setup(x => x.ObterLancamentoPorIdAsync(lancamentoId)).ReturnsAsync(lancamento);

            // Act
            var result = await _lancamentoService.ObterLancamentoPorIdAsync(lancamentoId);

            // Assert
            Assert.Equal(lancamento, result);
            _mockLancamentoRepository.Verify(x => x.ObterLancamentoPorIdAsync(lancamentoId), Times.Once);
        }

        [Fact]
        public async Task ObterLancamentoDoCacheAsync_ShouldReturnLancamentoFromCache()
        {
            // Arrange
            var lancamentoId = Guid.NewGuid();
            var lancamento = new Lancamento { Id = lancamentoId, Valor = 100, Tipo = "Crédito", Data = DateTime.UtcNow };

            _mockRedisCacheService.Setup(x => x.GetCacheValueAsync<Lancamento>(It.Is<string>(key => key == $"Lancamento:{lancamentoId}")))
                                  .ReturnsAsync(lancamento);

            // Act
            var result = await _lancamentoService.ObterLancamentoDoCacheAsync(lancamentoId);

            // Assert
            Assert.Equal(lancamento, result);
            _mockRedisCacheService.Verify(x => x.GetCacheValueAsync<Lancamento>(It.Is<string>(key => key == $"Lancamento:{lancamentoId}")), Times.Once);
        }
    }
}
