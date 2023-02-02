using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;

namespace TorneSe.ServicoLancamentoNotas.Dominio.Mensagens;

public class NotaLancadaMensagem : Mensagem 
{
    public int AlunoId { get; }
    public int AtividadeId { get; }
    public string CorrelationId { get; }
    public double ValorNota { get; }
    public bool NotaSubstitutiva { get; }

    public NotaLancadaMensagem(int alunoId, int atividadeId, string correlationId, double valorNota, bool notaSubstitutiva)
    {
        AlunoId = alunoId;
        AtividadeId = atividadeId;
        CorrelationId = correlationId;
        ValorNota = valorNota;
        NotaSubstitutiva = notaSubstitutiva;
    }

    public static NotaLancadaMensagem DeNota(Nota nota, string correlationId, bool substitutiva)
       => new(nota.AlunoId, nota.AtividadeId, correlationId, nota.ValorNota, substitutiva);
}
