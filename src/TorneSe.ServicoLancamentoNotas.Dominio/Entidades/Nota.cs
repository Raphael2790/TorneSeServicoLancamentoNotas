using TorneSe.ServicoLancamentoNotas.Dominio.Constantes;
using TorneSe.ServicoLancamentoNotas.Dominio.Enums;
using TorneSe.ServicoLancamentoNotas.Dominio.Exceptions;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;

namespace TorneSe.ServicoLancamentoNotas.Dominio.Entidades;

public class Nota : Entidade, IRaizAgregacao
{
    public int AlunoId { get; private set; }
    public int AtividadeId { get; private set; }
    public double ValorNota { get; private set; }
    public DateTime DataLancamento { get; private set; }
    public DateTime DataAtualizacao { get; private set; }
    public int UsuarioId { get; private set; }
    public bool CanceladaPorRetentativa { get; private set; }
    public string MotivoCancelamento { get; private set; }
    public StatusIntegracao StatusIntegracao { get; private set; }

    public Nota(int alunoId, int atividadeId, double valorNota, DateTime dataLancamento, int usuarioId)
    {
        AlunoId = alunoId;
        AtividadeId = atividadeId;
        ValorNota = valorNota;
        DataLancamento = dataLancamento;
        UsuarioId = usuarioId;
        CanceladaPorRetentativa = false;
        DataAtualizacao = DateTime.Now;
        StatusIntegracao = StatusIntegracao.AguardandoIntegracao;

        Validar();
    }

    private void Validar()
    {
        if(ValorNota < default(double) || ValorNota > 10)
            throw new ValidacaoEntidadeException(ConstantesDominio.MensagensValidacoes.ERRO_VALOR_NOTA_INVALIDO);
        if (UsuarioId <= default(int))
            throw new ValidacaoEntidadeException(ConstantesDominio.MensagensValidacoes.ERRO_USUARIO_INVALIDO);
        if (AlunoId <= default(int))
            throw new ValidacaoEntidadeException(ConstantesDominio.MensagensValidacoes.ERRO_ALUNO_INVALIDO);
        if (AtividadeId <= default(int))
            throw new ValidacaoEntidadeException(ConstantesDominio.MensagensValidacoes.ERRO_ATIVIDADE_INVALIDA);
    }
}
