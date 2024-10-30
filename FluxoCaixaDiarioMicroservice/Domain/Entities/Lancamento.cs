namespace FluxoCaixaDiarioMicroservice.Domain.Entities
{
    public class Lancamento
    {
        public Guid Id { get; set; }
        public decimal Valor { get; set; }
        public string? Tipo { get; set; }  // Crédito ou Débito
        public DateTime Data { get; set; }

        public Lancamento()
        {
            Id = Guid.NewGuid();
            Tipo = string.Empty; // garante que não seja nulo
            Data = DateTime.UtcNow;
        }
    }
}
