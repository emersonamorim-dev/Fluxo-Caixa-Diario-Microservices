using FluentValidation.Results;
using FluentValidation;
using FluxoCaixaDiarioMicroservice.Domain.Entities;
using FluxoCaixaDiarioMicroservice.Domain.Interfaces;
using FluxoCaixaDiarioMicroservice.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoCaixaDiarioMicroservice.Tests.Presentation.Controllers
{
    public class LancamentosControllerTests
    {
        private readonly Mock<ILancamentoService> _lancamentoServiceMock;
        private readonly Mock<IValidator<Lancamento>> _lancamentoValidatorMock;
        private readonly Mock<ILogger<LancamentosController>> _loggerMock;
        private readonly LancamentosController _controller;

        public LancamentosControllerTests()
        {
            _lancamentoServiceMock = new Mock<ILancamentoService>();
            _lancamentoValidatorMock = new Mock<IValidator<Lancamento>>();
            _loggerMock = new Mock<ILogger<LancamentosController>>();
            _controller = new LancamentosController(
                _lancamentoServiceMock.Object,
                _lancamentoValidatorMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task AdicionarLancamento_LancamentoNulo_RetornaBadRequest()
        {
            // Act
            var result = await _controller.AdicionarLancamento(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Dados do lançamento inválidos.", badRequestResult.Value);
        }

        [Fact]
        public async Task AdicionarLancamento_LancamentoInvalido_RetornaBadRequest()
        {
            // Arrange
            var lancamento = new Lancamento();
            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("Tipo", "O tipo de lançamento é obrigatório.")
            });

            _lancamentoValidatorMock.Setup(v => v.ValidateAsync(lancamento, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.AdicionarLancamento(lancamento);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(validationResult.Errors, badRequestResult.Value);
        }

        [Fact]
        public async Task AdicionarLancamento_LancamentoValido_RetornaCreatedAtAction()
        {
            // Arrange
            var lancamento = new Lancamento { Id = Guid.NewGuid(), Tipo = "Crédito", Valor = 100, Data = DateTime.UtcNow };
            var validationResult = new ValidationResult();

            _lancamentoValidatorMock.Setup(v => v.ValidateAsync(lancamento, default))
                .ReturnsAsync(validationResult);

            _lancamentoServiceMock.Setup(s => s.AdicionarLancamentoAsync(lancamento))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AdicionarLancamento(lancamento);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.ObterLancamentoPorId), createdAtActionResult.ActionName);
            Assert.Equal(lancamento.Id, ((Lancamento)createdAtActionResult.Value).Id);
        }

        [Fact]
        public async Task ObterLancamentos_RetornaOkComListaDeLancamentos()
        {
            // Arrange
            var lancamentos = new List<Lancamento>
            {
                new Lancamento { Id = Guid.NewGuid(), Tipo = "Crédito", Valor = 100, Data = DateTime.UtcNow },
                new Lancamento { Id = Guid.NewGuid(), Tipo = "Débito", Valor = 50, Data = DateTime.UtcNow }
            };

            _lancamentoServiceMock.Setup(s => s.ObterTodosLancamentosAsync())
                .ReturnsAsync(lancamentos);

            // Act
            var result = await _controller.ObterLancamentos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(lancamentos, okResult.Value);
        }

        [Fact]
        public async Task ObterLancamentoPorId_LancamentoExistente_RetornaOkComLancamento()
        {
            // Arrange
            var lancamento = new Lancamento { Id = Guid.NewGuid(), Tipo = "Crédito", Valor = 100, Data = DateTime.UtcNow };

            _lancamentoServiceMock.Setup(s => s.ObterLancamentoPorIdAsync(lancamento.Id))
                .ReturnsAsync(lancamento);

            // Act
            var result = await _controller.ObterLancamentoPorId(lancamento.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(lancamento, okResult.Value);
        }

        [Fact]
        public async Task ObterLancamentoPorId_LancamentoNaoEncontrado_RetornaNotFound()
        {
            // Arrange
            var lancamentoId = Guid.NewGuid();

            _lancamentoServiceMock.Setup(s => s.ObterLancamentoPorIdAsync(lancamentoId))
                .ReturnsAsync((Lancamento)null);

            // Act
            var result = await _controller.ObterLancamentoPorId(lancamentoId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}

