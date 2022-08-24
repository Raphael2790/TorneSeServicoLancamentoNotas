using TorneSe.ServicoLancamentoNotas.Dominio.Constantes;
using TorneSe.ServicoLancamentoNotas.Dominio.Enums;
using TorneSe.ServicoLancamentoNotas.Dominio.Params;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;
using TorneSe.ServicoLancamentoNotas.Dominio.Validacoes;
using TorneSe.ServicoLancamentoNotas.Dominio.Validacoes.Validador;

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
    public bool Cancelada { get; private set; }
    public string? MotivoCancelamento { get; private set; }
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
        DataCriacao = DateTime.Now;
        //Validar();

        Validar(this, NotaValidador.Instance);
    }

    private void Validar()
        => ValidacoesDominio
            .Validar(this, NotaValidador.Instance);

    public void Cancelar(string motivoCancelamento)
    {
        if (string.IsNullOrWhiteSpace(motivoCancelamento))
        {
            Notificar(new(nameof(MotivoCancelamento), ConstantesDominio.MensagensValidacoes.ERRO_MOTIVO_CANCELAMENTO_NAO_INFORMADO));
            EhValida = false;
            return;
        }
        MotivoCancelamento = motivoCancelamento;
        Cancelada = true;
        DataAtualizacao = DateTime.Now;
        Validar(this, NotaValidador.Instance);
    }

    public void CancelarPorRetentativa()
    {
        MotivoCancelamento = ConstantesDominio.Mensagens.NOTA_CANCELADA_POR_RETENTATIVA;
        Cancelada = true;
        CanceladaPorRetentativa = true;
        DataAtualizacao = DateTime.Now;
        Validar();
    }

    public void AtualizarValorNota(double novoValorNota)
    {
        ValorNota = novoValorNota;
        DataAtualizacao = DateTime.Now;
        Validar();
    }

    public void AtualizarStatusIntegracao(StatusIntegracao novoStatus)
    {
        if (StatusIntegracao == StatusIntegracao.AguardandoIntegracao
            && novoStatus == StatusIntegracao.IntegradaComSucesso
            || novoStatus == StatusIntegracao.FalhaNaIntegracao)
        {
            Notificar(new Notificacao(nameof(StatusIntegracao), 
                $"Não é permitada a mudança do status {StatusIntegracao} para {novoStatus}"));
            EhValida = false;
            return;
        }

        StatusIntegracao = novoStatus;
        DataAtualizacao = DateTime.Now;
        Validar();
    }
}
