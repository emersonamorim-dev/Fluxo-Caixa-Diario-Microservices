using FluxoCaixaDiarioMicroservice.Domain.Entities;
using System;
using Xunit;

namespace FluxoCaixaDiarioMicroservice.Tests.Domain.Entities
{
    public class SaldoConsolidadoTests
    {
        [Fact]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var expectedData = new DateTime(2024, 10, 29);
            var expectedSaldo = 1000m;

            // Act
            var saldoConsolidado = new SaldoConsolidado(expectedData, expectedSaldo);

            // Assert
            Assert.Equal(expectedData, saldoConsolidado.Data);
            Assert.Equal(expectedSaldo, saldoConsolidado.Saldo);
        }

        [Fact]
        public void Data_SetAndGet_ShouldReturnCorrectValue()
        {
            // Arrange
            var saldoConsolidado = new SaldoConsolidado(DateTime.Now, 500m);
            var newData = new DateTime(2024, 11, 1);

            // Act
            saldoConsolidado.Data = newData;

            // Assert
            Assert.Equal(newData, saldoConsolidado.Data);
        }

        [Fact]
        public void Saldo_SetAndGet_ShouldReturnCorrectValue()
        {
            // Arrange
            var saldoConsolidado = new SaldoConsolidado(DateTime.Now, 500m);
            var newSaldo = 1500m;

            // Act
            saldoConsolidado.Saldo = newSaldo;

            // Assert
            Assert.Equal(newSaldo, saldoConsolidado.Saldo);
        }

        [Fact]
        public void Constructor_ShouldAllowNegativeSaldo()
        {
            // Arrange
            var data = DateTime.Now;
            var negativeSaldo = -500m;

            // Act
            var saldoConsolidado = new SaldoConsolidado(data, negativeSaldo);

            // Assert
            Assert.Equal(negativeSaldo, saldoConsolidado.Saldo);
        }
    }
}
