using FluxoCaixaDiarioMicroservice.Application.Requests;
using System;
using Xunit;

namespace FluxoCaixaDiarioMicroservice.Tests.Application.Requests
{
    public class ConsolidarSaldoRequestTests
    {
        [Fact]
        public void Constructor_DeveInicializarComDataValida()
        {
            // Arrange
            var data = new DateTime(2024, 10, 29);

            // Act
            var request = new ConsolidarSaldoRequest { Data = data };

            // Assert
            Assert.Equal(data, request.Data);
        }

        [Fact]
        public void Data_DevePermitirAtribuicaoDeValor()
        {
            // Arrange
            var dataInicial = new DateTime(2024, 10, 29);
            var novaData = new DateTime(2024, 11, 1);
            var request = new ConsolidarSaldoRequest { Data = dataInicial };

            // Act
            request.Data = novaData;

            // Assert
            Assert.Equal(novaData, request.Data);
        }

        [Fact]
        public void Data_DeveSerDoTipoDateTime()
        {
            // Arrange & Act
            var request = new ConsolidarSaldoRequest();

            // Assert
            Assert.IsType<DateTime>(request.Data);
        }
    }
}
