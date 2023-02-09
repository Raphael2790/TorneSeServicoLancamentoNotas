using MediatR;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.Eventos;

public class NotaCanceladaEvento : Evento, INotification
{
    public Guid NotaId { get; set; }

    public NotaCanceladaEvento(Guid notaId)
    {
        NotaId = notaId;
    }
}
