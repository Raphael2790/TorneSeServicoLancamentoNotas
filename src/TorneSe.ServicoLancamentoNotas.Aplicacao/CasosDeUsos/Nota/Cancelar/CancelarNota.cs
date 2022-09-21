using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Mapeadores;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar;

public class CancelarNota : ICancelarNota
{
    private readonly INotaRepository _notaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelarNota(INotaRepository notaRepository, IUnitOfWork unitOfWork)
    {
        _notaRepository = notaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<NotaOutputModel> Handle(CancelarNotaInput request, CancellationToken cancellationToken)
    {
        var nota = await _notaRepository.BuscarNotaPorAlunoEAtividade(request.AlunoId, request.AtividadeId, cancellationToken);
        nota.Cancelar(request.Motivo);
        await _notaRepository.Atualizar(nota, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        return MapeadorAplicacao.NotaEmNotaOuputModel(nota);
    }
}
