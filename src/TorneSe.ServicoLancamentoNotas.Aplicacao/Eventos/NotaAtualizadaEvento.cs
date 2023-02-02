using MediatR;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.Eventos;

public class NotaAtualizadaEvento : Evento, INotification
{
    public Guid NotaId { get; }

    public NotaAtualizadaEvento(Guid notaId)
    {
        NotaId = notaId;
    }
}
