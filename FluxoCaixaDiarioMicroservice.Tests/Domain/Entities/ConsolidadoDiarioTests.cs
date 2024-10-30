using FluxoCaixaDiarioMicroservice.Domain.Entities;
using System;
using Xunit;

namespace FluxoCaixaDiarioMicroservice.Tests.Domain.Entities
{
    public class SaldoConsolidadoTests
    {
        [Fact]
        public void Construtor_ComValoresCompletos_DeveInicializarPropriedadesCorretamente()
        {
            // Arrange
            var data = new DateTime(2024, 10, 30);
            decimal saldo = 1000m;
            decimal totalCreditos = 5000m;
            decimal totalDebitos = 3000m;

            // Act
            var saldoConsolidado = new SaldoConsolidado(data, saldo, totalCreditos, totalDebitos);

            // Assert
            Assert.Equal(data, saldoConsolidado.Data);
            Assert.Equal(saldo, saldoConsolidado.Saldo);
            Assert.Equal(totalCreditos, saldoConsolidado.TotalCreditos);
            Assert.Equal(totalDebitos, saldoConsolidado.TotalDebitos);
            Assert.Equal(3000m, saldoConsolidado.SaldoFinal); // 1000 + 5000 - 3000 = 3000
        }

        [Fact]
        public void Construtor_SemSaldoInicial_DeveInicializarPropriedadesCorretamente()
        {
            // Arrange
            var data = new DateTime(2024, 10, 30);
            decimal totalCreditos = 5000m;
            decimal totalDebitos = 3000m;

            // Act
            var saldoConsolidado = new SaldoConsolidado(data, totalCreditos, totalDebitos);

            // Assert
            Assert.Equal(data, saldoConsolidado.Data);
            Assert.Equal(totalCreditos, saldoConsolidado.TotalCreditos);
            Assert.Equal(totalDebitos, saldoConsolidado.TotalDebitos);
            Assert.Equal(2000m, saldoConsolidado.SaldoFinal); // 5000 - 3000 = 2000
        }

        [Fact]
        public void Construtor_Padrao_DeveInicializarPropriedadesComoPadrao()
        {
            // Act
            var saldoConsolidado = new SaldoConsolidado();

            // Assert
            Assert.Equal(default(DateTime), saldoConsolidado.Data);
            Assert.Equal(0m, saldoConsolidado.Saldo);
            Assert.Equal(0m, saldoConsolidado.TotalCreditos);
            Assert.Equal(0m, saldoConsolidado.TotalDebitos);
            Assert.Equal(0m, saldoConsolidado.SaldoFinal);
        }

        [Fact]
        public void CalcularSaldoFinal_QuandoValoresEstaoDefinidos_DeveRetornarSaldoFinalCorreto()
        {
            // Arrange
            var data = new DateTime(2024, 10, 30);
            decimal saldo = 2000m;
            decimal totalCreditos = 3000m;
            decimal totalDebitos = 1000m;

            // Act
            var saldoConsolidado = new SaldoConsolidado(data, saldo, totalCreditos, totalDebitos);

            // Assert
            Assert.Equal(4000m, saldoConsolidado.SaldoFinal); // 2000 + 3000 - 1000 = 4000
        }

        [Fact]
        public void SaldoFinal_DeveSerNegativo_SeTotalDebitosMaiorQueTotalCreditos()
        {
            // Arrange
            var data = new DateTime(2024, 10, 30);
            decimal totalCreditos = 1000m;
            decimal totalDebitos = 3000m;

            // Act
            var saldoConsolidado = new SaldoConsolidado(data, totalCreditos, totalDebitos);

            // Assert
            Assert.Equal(-2000m, saldoConsolidado.SaldoFinal); // 1000 - 3000 = -2000
        }
    }
}
