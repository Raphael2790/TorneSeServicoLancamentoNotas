using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;

namespace TorneSe.ServicoLancamentoNotas.Dominio.Mensagens;

public class NotaCanceladaMensagem : Mensagem
{
    public int AlunoId { get; set; }
    public int AtividadeId { get; set; }
    public string CorrelationId { get; set; }
    public string Motivo { get; set; }

    public NotaCanceladaMensagem(int alunoId, int atividadeId, string correlationId, string motivo)
    {
        AlunoId = alunoId;
        AtividadeId = atividadeId;
        Motivo = motivo;
        CorrelationId = correlationId;
    }

    public static NotaCanceladaMensagem DeNota(Nota nota, string correlationId)
       => new(nota.AlunoId, nota.AtividadeId, correlationId, nota.MotivoCancelamento!);
}
