using Microsoft.Extensions.Logging;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Enums;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Mapeadores;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork.BuscaRepository;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar;

public class ConsultaNota : IConsultaNota
{
    private readonly INotaRepository _notaRepository;
    private readonly ILogger<ConsultaNota> _logger;

    public ConsultaNota(INotaRepository notaRepository, 
                        ILogger<ConsultaNota> logger)
    {
        _notaRepository = notaRepository;
        _logger = logger;
    }

    public async Task<Resultado<ListaNotaOutput>> Handle(ListaNotaInput request, CancellationToken cancellationToken)
    {
        try
        {
            var buscaOutput = await _notaRepository.Buscar(new BuscaInput(request.Pagina, request.PorPagina, request.AlunoId,
                                                            request.AtividadeId, request.OrdenarPor, request.Ordenacao), cancellationToken);

            ListaNotaOutput retorno = new(buscaOutput.Pagina, buscaOutput.PorPagina, buscaOutput.Total,
                buscaOutput.Items.Select(nota => MapeadorAplicacao.NotaEmNotaOuputModel(nota)).ToList().AsReadOnly());

            return Resultado<ListaNotaOutput>.RetornaResultadoSucesso(retorno);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Resultado<ListaNotaOutput>.RetornaResultadoErro(TipoErro.ErroInesperado);
        }
    }
}
