using FluentValidation;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Contantes;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.Validacoes;

public class CancelarNotaInputValidator : AbstractValidator<CancelarNotaInput>
{
    public readonly static CancelarNotaInputValidator Instance = new();

    public CancelarNotaInputValidator()
	{
        RuleFor(x => x.AlunoId)
            .GreaterThan(default(int))
            .WithMessage(ConstantesAplicacao.MensagensValidacao.ALUNO_ID_INVALIDO);

        RuleFor(x => x.ProfessorId)
            .GreaterThan(default(int))
            .WithMessage(ConstantesAplicacao.MensagensValidacao.PROFESSOR_ID_INVALIDO);

        RuleFor(x => x.AtividadeId)
            .GreaterThan(default(int))
            .WithMessage(ConstantesAplicacao.MensagensValidacao.ATIVIDADE_ID_INVALIDO);

        RuleFor(x => x.Motivo)
            .NotEmpty()
            .WithMessage(ConstantesAplicacao.MensagensValidacao.MOTIVO_CANCELAMENTO)
            .NotNull()
            .WithMessage(ConstantesAplicacao.MensagensValidacao.MOTIVO_CANCELAMENTO);
    }
}
