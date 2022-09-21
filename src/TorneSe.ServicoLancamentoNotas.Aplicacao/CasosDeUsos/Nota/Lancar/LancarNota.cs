using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Mapeadores;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar;

public sealed class LancarNota : ILancarNota
{
    private readonly INotaRepository _notaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LancarNota(INotaRepository notaRepository, IUnitOfWork unitOfWork)
    {
        _notaRepository = notaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<NotaOutputModel> Handle(LancarNotaInput request, CancellationToken cancellationToken)
    {
        if (request.NotaSubstitutiva)
        {
            var nota = await _notaRepository.BuscarNotaPorAlunoEAtividade(request.AlunoId, request.AtividadeId, cancellationToken);
            nota.CancelarPorRetentativa();
            await _notaRepository.Atualizar(nota, cancellationToken);
        }

        var novaNota = MapeadorAplicacao.LancarNotaInputEmNota(request);

        await _notaRepository.Inserir(novaNota, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return MapeadorAplicacao.NotaEmNotaOuputModel(novaNota);
    }
}
