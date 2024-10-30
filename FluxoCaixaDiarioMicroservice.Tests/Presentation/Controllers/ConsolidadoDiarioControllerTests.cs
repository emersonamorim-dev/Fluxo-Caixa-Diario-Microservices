using FluxoCaixaDiarioMicroservice.Application.Requests;
using FluxoCaixaDiarioMicroservice.Domain.Interfaces;
using FluxoCaixaDiarioMicroservice.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FluxoCaixaDiarioMicroservice.Tests.Presentation.Controllers
{
    public class ConsolidadoDiarioControllerTests
    {
        private readonly Mock<IConsolidadoDiarioService> _mockConsolidadoDiarioService;
        private readonly ConsolidadoDiarioController _controller;

        public ConsolidadoDiarioControllerTests()
        {
            _mockConsolidadoDiarioService = new Mock<IConsolidadoDiarioService>();
            _controller = new ConsolidadoDiarioController(_mockConsolidadoDiarioService.Object);
        }

        [Fact]
        public async Task ConsolidarSaldoDiario_ComDataQueryParam_RetornaOk()
        {
            // Arrange
            var data = new DateTime(2024, 10, 30);
            _mockConsolidadoDiarioService.Setup(s => s.ConsolidarSaldoDiarioAsync(data)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ConsolidarSaldoDiario(data, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"Saldo diário consolidado com sucesso para a data: {data:yyyy-MM-dd}", okResult.Value);
        }

        [Fact]
        public async Task ConsolidarSaldoDiario_ComDataNoBody_RetornaOk()
        {
            // Arrange
            var request = new ConsolidarSaldoRequest { Data = new DateTime(2024, 10, 30) };
            _mockConsolidadoDiarioService.Setup(s => s.ConsolidarSaldoDiarioAsync(request.Data)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ConsolidarSaldoDiario(null, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"Saldo diário consolidado com sucesso para a data: {request.Data:yyyy-MM-dd}", okResult.Value);
        }

        [Fact]
        public async Task ConsolidarSaldoDiario_SemData_RetornaBadRequest()
        {
            // Act
            var result = await _controller.ConsolidarSaldoDiario(null, null);

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, badRequestResult.StatusCode);
            Assert.Contains("A data fornecida é inválida", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task ConsolidarSaldoDiario_ComErroInterno_RetornaStatusCode500()
        {
            // Arrange
            var data = new DateTime(2024, 10, 30);
            _mockConsolidadoDiarioService.Setup(s => s.ConsolidarSaldoDiarioAsync(data)).ThrowsAsync(new Exception("Erro interno"));

            // Act
            var result = await _controller.ConsolidarSaldoDiario(data, null);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Contains("Erro ao consolidar saldo diário: Erro interno", statusCodeResult.Value.ToString());
        }
    }
}
