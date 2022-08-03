namespace TorneSe.ServicoLancamentoNotas.Dominio.Params;

public struct NotaParams
{
    public int AlunoId { get; private set; }
    public int AtividadeId { get; private set; }
    public double ValorNota { get; private set; }
    public DateTime DataLancamento { get; private set; }
    public int UsuarioId { get; private set; }

    public NotaParams(int alunoId, int atividadeId, double valorNota, DateTime dataLancamento, int usuarioId)
    {
        AlunoId = alunoId;
        AtividadeId = atividadeId;
        ValorNota = valorNota;
        DataLancamento = dataLancamento;
        UsuarioId = usuarioId;
    }
}
