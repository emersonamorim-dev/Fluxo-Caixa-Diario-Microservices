using FluxoCaixaDiarioMicroservice.Domain.Entities;
using FluxoCaixaDiarioMicroservice.Domain.Interfaces;
using FluxoCaixaDiarioMicroservice.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FluxoCaixaDiarioMicroservice.Application.UseCases
{
    public class ConsolidarSaldoDiarioUseCase : IConsolidarSaldoDiarioUseCase
    {
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly ILogger<ConsolidarSaldoDiarioUseCase> _logger;

        public ConsolidarSaldoDiarioUseCase(ILancamentoRepository lancamentoRepository, ILogger<ConsolidarSaldoDiarioUseCase> logger)
        {
            _lancamentoRepository = lancamentoRepository ?? throw new ArgumentNullException(nameof(lancamentoRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<SaldoConsolidado> ExecuteAsync(DateTime data)
        {
            try
            {
                _logger.LogInformation("Iniciando consolidação do saldo para a data: {Data}", data);

                // Obtém valores de crédito e débito separadamente
                var (totalCreditos, totalDebitos) = await CalcularCreditosDebitosDiario(data);

                _logger.LogInformation("Saldo consolidado para a data {Data} foi calculado com sucesso.", data);

                return new SaldoConsolidado(data, totalCreditos, totalDebitos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consolidar o saldo para a data: {Data}", data);
                throw;
            }
        }

        private async Task<(decimal totalCreditos, decimal totalDebitos)> CalcularCreditosDebitosDiario(DateTime data)
        {
            try
            {
                _logger.LogInformation("Obtendo lançamentos para a data: {Data}", data);

                var lancamentos = await _lancamentoRepository.ObterLancamentosPorDataAsync(data);

                if (lancamentos == null || !lancamentos.Any())
                {
                    _logger.LogWarning("Nenhum lançamento encontrado para a data: {Data}", data);
                    return (0, 0);
                }

                // Separar créditos e débitos
                var totalCreditos = lancamentos
                    .Where(l => l.Tipo.Equals("Crédito", StringComparison.OrdinalIgnoreCase))
                    .Sum(l => l.Valor);

                var totalDebitos = lancamentos
                    .Where(l => l.Tipo.Equals("Débito", StringComparison.OrdinalIgnoreCase))
                    .Sum(l => l.Valor);

                _logger.LogInformation("Total de créditos para a data {Data}: {TotalCreditos}, Total de débitos: {TotalDebitos}", data, totalCreditos, totalDebitos);

                return (totalCreditos, totalDebitos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao calcular créditos e débitos para a data: {Data}", data);
                throw;
            }
        }
    }
}
