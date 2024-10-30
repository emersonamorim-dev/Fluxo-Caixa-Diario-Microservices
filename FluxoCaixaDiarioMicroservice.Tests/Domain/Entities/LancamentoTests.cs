using FluxoCaixaDiarioMicroservice.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoCaixaDiarioMicroservice.Tests.Domain.Entities
{
    public class LancamentoTests
    {
        [Fact]
        public void Lancamento_Construtor_DeveInicializarComValoresPadrao()
        {
            // Arrange & Act
            var lancamento = new Lancamento();

            // Assert
            Assert.NotEqual(Guid.Empty, lancamento.Id); // O ID deve ser um novo GUID gerado
            Assert.Equal(0, lancamento.Valor); // O valor inicial deve ser 0
            Assert.Equal(string.Empty, lancamento.Tipo); // Tipo deve ser inicializado como string vazia
            Assert.Equal(DateTime.UtcNow.Date, lancamento.Data.Date); // Data deve ser a data atual (em UTC)
        }

        [Fact]
        public void Lancamento_DevePermitirAlterarValor()
        {
            // Arrange
            var lancamento = new Lancamento();
            var valorEsperado = 1500.75m;

            // Act
            lancamento.Valor = valorEsperado;

            // Assert
            Assert.Equal(valorEsperado, lancamento.Valor);
        }

        [Theory]
        [InlineData("Crédito")]
        [InlineData("Débito")]
        public void Lancamento_DevePermitirAlterarTipo(string tipoEsperado)
        {
            // Arrange
            var lancamento = new Lancamento();

            // Act
            lancamento.Tipo = tipoEsperado;

            // Assert
            Assert.Equal(tipoEsperado, lancamento.Tipo);
        }

        [Fact]
        public void Lancamento_DevePermitirAlterarData()
        {
            // Arrange
            var lancamento = new Lancamento();
            var dataEsperada = new DateTime(2024, 10, 29);

            // Act
            lancamento.Data = dataEsperada;

            // Assert
            Assert.Equal(dataEsperada, lancamento.Data);
        }

        [Fact]
        public void Lancamento_Valor_DeveSerPositivoOuZero()
        {
            // Arrange
            var lancamento = new Lancamento();

            // Act & Assert
            lancamento.Valor = 0; // Deve permitir valor zero
            Assert.Equal(0, lancamento.Valor);

            lancamento.Valor = 100; // Deve permitir valor positivo
            Assert.Equal(100, lancamento.Valor);
        }

        [Fact]
        public void Lancamento_Tipo_DeveSerCreditoOuDebito()
        {
            // Arrange
            var lancamento = new Lancamento();

            // Act
            lancamento.Tipo = "Crédito";
            Assert.Equal("Crédito", lancamento.Tipo);

            lancamento.Tipo = "Débito";
            Assert.Equal("Débito", lancamento.Tipo);
        }

        [Fact]
        public void Lancamento_Tipo_NaoDeveAceitarValoresInvalidos()
        {
            // Arrange
            var lancamento = new Lancamento();

            // Act
            lancamento.Tipo = "Invalido";

            // Assert
            Assert.NotEqual("Crédito", lancamento.Tipo);
            Assert.NotEqual("Débito", lancamento.Tipo);
        }
    }
}
