using MediatR;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.Interfaces;

public interface IConsultaNota : IRequestHandler<ListaNotaInput, Resultado<ListaNotaOutput>> {}
