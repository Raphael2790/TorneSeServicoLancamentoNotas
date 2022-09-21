using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Mapeadores;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar;

public class AtualizarNota : IAtualizarNota
{
    private readonly INotaRepository _notaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AtualizarNota(INotaRepository notaRepository, IUnitOfWork unitOfWork)
    {
        _notaRepository = notaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<NotaOutputModel> Handle(AtualizarNotaInput request, CancellationToken cancellationToken)
    {
        var nota = await _notaRepository.BuscarNotaPorAlunoEAtividade(request.AlunoId, request.AtividadeId, cancellationToken);
        nota.AtualizarValorNota(request.ValorNota);
        await _notaRepository.Atualizar(nota, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        return MapeadorAplicacao.NotaEmNotaOuputModel(nota);
    }
}
