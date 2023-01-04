using FluentValidation;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Contantes;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.Validacoes;

public class LancarNotaInputValidator : AbstractValidator<LancarNotaInput>
{
	public readonly static LancarNotaInputValidator Instance = new();

	public LancarNotaInputValidator()
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

		RuleFor(x => x.ValorNota)
			.GreaterThan(default(int))
			.WithMessage(ConstantesAplicacao.MensagensValidacao.NOTA_INVALIDA);
	}
}
