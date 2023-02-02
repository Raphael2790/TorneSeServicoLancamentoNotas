using Microsoft.Extensions.Logging;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Eventos;
using TorneSe.ServicoLancamentoNotas.Aplicacao.EventosHandlers.Interfaces;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.EventosHandlers;

public class NotaAtualizadaEventoHandler : INotaAtualizadaEventoHandler
{
    private INotaRepository _notaRepository;
    private ILogger<NotaLancadaEventoHandler> _logger;

    public Task Handle(NotaAtualizadaEvento notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
