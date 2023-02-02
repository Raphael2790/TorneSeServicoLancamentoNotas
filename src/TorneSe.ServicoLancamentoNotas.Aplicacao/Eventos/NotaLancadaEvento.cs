using MediatR;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.Eventos;

public class NotaLancadaEvento : Evento, INotification
{
    public Guid NotaId { get; }

    public NotaLancadaEvento(Guid notaId)
    {
        NotaId = notaId;
    }
}
