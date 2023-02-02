using MediatR;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;

public interface IMediatorHandler
{
    Task<TResponse> EnviarRequest<TResponse, TRequest>(TRequest request, CancellationToken cancellationToken)
        where TRequest : IRequest<TResponse>;

    Task PublicarEvento<TNotificacao>(TNotificacao notificacao, CancellationToken cancellationToken)
        where TNotificacao : Evento, INotification;
}
