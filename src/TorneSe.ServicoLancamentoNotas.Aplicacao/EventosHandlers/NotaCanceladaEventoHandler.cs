using Microsoft.Extensions.Logging;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Eventos;
using TorneSe.ServicoLancamentoNotas.Aplicacao.EventosHandlers.Interfaces;
using TorneSe.ServicoLancamentoNotas.Dominio.Clients;
using TorneSe.ServicoLancamentoNotas.Dominio.Mensagens;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.EventosHandlers;

public class NotaCanceladaEventoHandler : INotaCanceladaEventoHandler
{
    private readonly INotaCanceladaMensagemClient _mensagemClient;
    private readonly INotaRepository _notaRepository;
    private readonly ILogger<NotaCanceladaEventoHandler> _logger;

    public NotaCanceladaEventoHandler(
        INotaCanceladaMensagemClient mensagemClient, 
        INotaRepository notaRepository, 
        ILogger<NotaCanceladaEventoHandler> logger)
    {
        _mensagemClient = mensagemClient;
        _notaRepository = notaRepository;
        _logger = logger;
    }

    public async Task Handle(NotaCanceladaEvento notification, CancellationToken cancellationToken)
    {
        var nota = await _notaRepository.Buscar(notification.NotaId, cancellationToken);

        if (nota is null)
        {
            _logger.LogInformation($"A nota com id {notification.NotaId} não foi encontrada");
            return;
        }

        var mensagem = NotaCanceladaMensagem.DeNota(nota, notification.CorrelationId);

        nota.AlterarStatusIntegracaoParaEnviada();

        await _notaRepository.Atualizar(nota, cancellationToken);

        await _mensagemClient.EnviarMensagem(mensagem);
    }
}
