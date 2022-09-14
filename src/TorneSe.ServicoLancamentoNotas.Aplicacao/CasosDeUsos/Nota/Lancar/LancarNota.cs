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
        var nota = MapeadorAplicacao.LancarNotaInputEmNota(request);

        await _notaRepository.Inserir(nota, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return MapeadorAplicacao.NotaEmNotaOuputModel(nota);
    }
}
