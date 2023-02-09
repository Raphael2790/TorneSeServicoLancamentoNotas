using Microsoft.Extensions.Logging;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Eventos;
using TorneSe.ServicoLancamentoNotas.Aplicacao.EventosHandlers.Interfaces;
using TorneSe.ServicoLancamentoNotas.Dominio.Clients;
using TorneSe.ServicoLancamentoNotas.Dominio.Mensagens;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.EventosHandlers;

public class NotaLancadaEventoHandler : INotaLancadaEventoHandler
{
    private readonly INotaLancadaMensagemClient _mensagemClient;
    private readonly INotaRepository _notaRepository;
    private readonly ILogger<NotaLancadaEventoHandler> _logger;

    public NotaLancadaEventoHandler(
        INotaLancadaMensagemClient mensagemClient, 
        INotaRepository notaRepository, 
        ILogger<NotaLancadaEventoHandler> logger)
    {
        _mensagemClient = mensagemClient;
        _notaRepository = notaRepository;
        _logger = logger;
    }

    public async Task Handle(NotaLancadaEvento notification, CancellationToken cancellationToken)
    {
        var nota = await _notaRepository.Buscar(notification.NotaId, cancellationToken);

        if(nota is null)
        {
            _logger.LogInformation($"A nota com id {notification.NotaId} não foi encontrada");
            return;
        }

        bool existeNotaJaCancelada = await _notaRepository
            .ExisteNotaCanceladaPorAlunoEAtividade(nota.AlunoId, nota.AtividadeId, cancellationToken);

        var mensagem = NotaLancadaMensagem.DeNota(nota, notification.CorrelationId, existeNotaJaCancelada);

        nota.AlterarStatusIntegracaoParaEnviada();

        await _notaRepository.Atualizar(nota, cancellationToken);

        await _mensagemClient.EnviarMensagem(mensagem);
    }
}
