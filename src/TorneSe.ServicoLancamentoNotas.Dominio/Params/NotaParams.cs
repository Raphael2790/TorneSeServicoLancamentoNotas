using TorneSe.ServicoLancamentoNotas.Dominio.Enums;

namespace TorneSe.ServicoLancamentoNotas.Dominio.Params;

public struct NotaParams
{
    public int AlunoId { get; private set; }
    public int AtividadeId { get; private set; }
    public double ValorNota { get; private set; }
    public DateTime DataLancamento { get; private set; }
    public int UsuarioId { get; private set; }
    public StatusIntegracao StatusIntegracao { get; private set; }

    public NotaParams(int alunoId, 
        int atividadeId, 
        double valorNota, DateTime dataLancamento, int usuarioId
        , StatusIntegracao statusIntegracao =  StatusIntegracao.AguardandoIntegracao)
    {
        AlunoId = alunoId;
        AtividadeId = atividadeId;
        ValorNota = valorNota;
        DataLancamento = dataLancamento;
        UsuarioId = usuarioId;
        StatusIntegracao = statusIntegracao;
    }
}
