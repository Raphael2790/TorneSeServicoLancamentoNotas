using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;

namespace TorneSe.ServicoLancamentoNotas.Dominio.Mensagens;

public class NotaAtualizadaMensagem : Mensagem
{
    public int AlunoId { get; }
    public int AtividadeId { get; }
    public string CorrelationId { get; }
    public double ValorNota { get; }

    public NotaAtualizadaMensagem(int alunoId, int atividadeId, string correlationId, double valorNota)
    {
        AlunoId = alunoId;
        AtividadeId = atividadeId;
        CorrelationId = correlationId;
        ValorNota = valorNota;
    }

    public static NotaAtualizadaMensagem DeNota(Nota nota, string correlationId)
       => new(nota.AlunoId, nota.AtividadeId, correlationId, nota.ValorNota);
}
