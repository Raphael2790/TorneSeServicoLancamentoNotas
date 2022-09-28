using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Mapeadores;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar;

public class ConsultaNota : IConsultaNota
{
    private readonly INotaRepository _notaRepository;

    public ConsultaNota(INotaRepository notaRepository) 
        => _notaRepository = notaRepository;

    public async Task<ListaNotaOutput> Handle(ListaNotaInput request, CancellationToken cancellationToken)
    {
        var buscaOutput = await _notaRepository.Buscar(new(request.Pagina, request.PorPagina, request.AlunoId,
                                                        request.AtividadeId, request.OrdenarPor, request.Ordenacao), cancellationToken);

        return new(buscaOutput.Pagina, buscaOutput.PorPagina, buscaOutput.Total,
            buscaOutput.Items.Select(nota => MapeadorAplicacao.NotaEmNotaOuputModel(nota)).ToList().AsReadOnly());
    }
}
