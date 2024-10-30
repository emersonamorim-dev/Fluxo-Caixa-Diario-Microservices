using FluentValidation;
using FluxoCaixaDiarioMicroservice.Domain.Entities;

namespace FluxoCaixaDiarioMicroservice.Presentation.Validators
{
    public class LancamentoValidator : AbstractValidator<Lancamento>
{
    public LancamentoValidator()
    {
        RuleFor(l => l.Tipo)
            .NotEmpty().WithMessage("O tipo de lançamento é obrigatório.")
            .Must(tipo => tipo?.ToLower() == "crédito" || tipo?.ToLower() == "débito")
            .WithMessage("Tipo deve ser 'Crédito' ou 'Débito'.");

        RuleFor(l => l.Valor)
            .GreaterThan(0).WithMessage("O valor deve ser maior que zero.");

        RuleFor(l => l.Data)
            .NotEmpty().WithMessage("A data do lançamento é obrigatória.");
    }
 }
}
