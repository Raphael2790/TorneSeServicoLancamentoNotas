using TorneSe.ServicoLancamentoNotas.Dominio.Constantes;
using TorneSe.ServicoLancamentoNotas.Dominio.Enums;
using TorneSe.ServicoLancamentoNotas.Dominio.Params;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;
using TorneSe.ServicoLancamentoNotas.Dominio.Validacoes;

namespace TorneSe.ServicoLancamentoNotas.Dominio.Entidades;

public class Nota : Entidade, IRaizAgregacao
{
    private const double VALOR_MAXIMO_NOTA = 10.00;

    public int AlunoId { get; private set; }
    public int AtividadeId { get; private set; }
    public double ValorNota { get; private set; }
    public DateTime DataLancamento { get; private set; }
    public int UsuarioId { get; private set; }
    public bool CanceladaPorRetentativa { get; private set; }
    public string MotivoCancelamento { get; private set; }
    public StatusIntegracao StatusIntegracao { get; private set; }

    public Nota(NotaParams notaParams)
    {
        AlunoId = notaParams.AlunoId;
        AtividadeId = notaParams.AtividadeId;
        ValorNota = notaParams.ValorNota;
        DataLancamento = notaParams.DataLancamento;
        UsuarioId = notaParams.UsuarioId;
        CanceladaPorRetentativa = false;
        StatusIntegracao = StatusIntegracao.AguardandoIntegracao;

        Validar();
    }

    private void Validar()
    {
        ValidacoesDominio
            .DeveEstarEntre(ValorNota, default, VALOR_MAXIMO_NOTA, this, nameof(ValorNota),
            ConstantesDominio.MensagensValidacoes.ERRO_VALOR_NOTA_INVALIDO);
        ValidacoesDominio
            .MaiorQue(UsuarioId, default, this, nameof(UsuarioId), ConstantesDominio.MensagensValidacoes.ERRO_USUARIO_INVALIDO);
        ValidacoesDominio
            .MaiorQue(AlunoId, default, this, nameof(AlunoId), ConstantesDominio.MensagensValidacoes.ERRO_ALUNO_INVALIDO);
        ValidacoesDominio
             .MaiorQue(AtividadeId, default, this, nameof(AtividadeId), ConstantesDominio.MensagensValidacoes.ERRO_ATIVIDADE_INVALIDA);

        if (!Notificacoes.Any())
            EhValida = true;
    }
}
