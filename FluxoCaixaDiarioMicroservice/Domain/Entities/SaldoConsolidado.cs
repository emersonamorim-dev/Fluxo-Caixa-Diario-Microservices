using System;
using System.Text.Json.Serialization;

namespace FluxoCaixaDiarioMicroservice.Domain.Entities
{
    public class SaldoConsolidado
    {
        public DateTime Data { get; set; }
        public decimal Saldo { get; set; }
        public decimal TotalCreditos { get; set; }
        public decimal TotalDebitos { get; set; }
        public decimal SaldoFinal { get; set; }

        // Construtor padrão necessário para a desserialização
        public SaldoConsolidado() { }

        // Construtor para inicializar saldo consolidado com valores completos
        [JsonConstructor]
        public SaldoConsolidado(DateTime data, decimal saldo, decimal totalCreditos, decimal totalDebitos)
        {
            Data = data;
            Saldo = saldo;
            TotalCreditos = totalCreditos;
            TotalDebitos = totalDebitos;
            SaldoFinal = saldo + totalCreditos - totalDebitos; 
        }

        // Construtor para consolidação sem saldo inicial
        public SaldoConsolidado(DateTime data, decimal totalCreditos, decimal totalDebitos)
        {
            Data = data;
            TotalCreditos = totalCreditos;
            TotalDebitos = totalDebitos;
            SaldoFinal = totalCreditos - totalDebitos;
        }
    }
}
