using FluentValidation;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Contantes;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.Validacoes;

public class AtualizarNotaInputValidator
    : AbstractValidator<AtualizarNotaInput>
{
	public AtualizarNotaInputValidator()
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
