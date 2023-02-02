using MediatR;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.Mediator;

public class MediatorHandler : IMediatorHandler
{
    private readonly IMediator _mediator;

    public MediatorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<TResponse> EnviarRequest<TResponse, TRequest>(TRequest request, CancellationToken cancellationToken) 
        where TRequest : IRequest<TResponse>
        => await _mediator.Send(request, cancellationToken);

    public async Task PublicarEvento<TNotificacao>(TNotificacao notificacao, CancellationToken cancellationToken)
        where TNotificacao : Evento, INotification
        => await _mediator.Publish(notificacao, cancellationToken);

}
