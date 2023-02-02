using MediatR;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Eventos;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.EventosHandlers.Interfaces;

public interface INotaAtualizadaEventoHandler : INotificationHandler<NotaAtualizadaEvento> {}
