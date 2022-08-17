using FluentValidation;
using TorneSe.ServicoLancamentoNotas.Dominio.Constantes;
using TorneSe.ServicoLancamentoNotas.Testes.Fakes;

namespace TorneSe.ServicoLancamentoNotas.Testes.Validacoes.Validador;

public class NotaFakeValidador : AbstractValidator<NotaFake>
{
    public static readonly NotaFakeValidador Instance = new();

    public NotaFakeValidador()
    {
        RuleFor(x => x.AlunoId)
            .GreaterThan(default(int))
            .WithMessage(ConstantesDominio.MensagensValidacoes.ERRO_ALUNO_INVALIDO);

        RuleFor(x => x.AtividadeId)
            .GreaterThan(default(int))
            .WithMessage(ConstantesDominio.MensagensValidacoes.ERRO_ATIVIDADE_INVALIDA);

        RuleFor(x => x.UsuarioId)
            .GreaterThan(default(int))
            .WithMessage(ConstantesDominio.MensagensValidacoes.ERRO_USUARIO_INVALIDO);

        RuleFor(x => x.ValorNota)
            .ExclusiveBetween(0.00, 10.00)
            .WithMessage(ConstantesDominio.MensagensValidacoes.ERRO_VALOR_NOTA_INVALIDO);

        When(x => x.MotivoCancelamento is not null, () =>
        {
            RuleFor(x => x.MotivoCancelamento)
                .MaximumLength(500)
                .WithMessage(ConstantesDominio.MensagensValidacoes.ERRO_MOTIVO_CANCELAMENTO_EXTENSO);

            RuleFor(x => x.CanceladaPorRetentativa)
                .Equal(true)
                .WithMessage(ConstantesDominio.MensagensValidacoes.ERRO_FLAG_CANCELADA_INATIVA);
        });
    }
}
