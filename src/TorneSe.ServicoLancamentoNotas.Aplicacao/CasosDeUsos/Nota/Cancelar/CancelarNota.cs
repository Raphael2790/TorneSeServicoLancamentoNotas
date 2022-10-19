using Microsoft.Extensions.Logging;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Enums;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Mapeadores;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar;

public class CancelarNota : ICancelarNota
{
    private readonly INotaRepository _notaRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CancelarNota> _logger;

    public CancelarNota(INotaRepository notaRepository, 
                        IUnitOfWork unitOfWork, 
                        ILogger<CancelarNota> logger)
    {
        _notaRepository = notaRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Resultado<NotaOutputModel>> Handle(CancelarNotaInput request, CancellationToken cancellationToken)
    {
        try
        {
            var nota = await _notaRepository.BuscarNotaPorAlunoEAtividade(request.AlunoId, request.AtividadeId, cancellationToken);

            if (nota is null)
                return Resultado<NotaOutputModel>.RetornaResultadoErro(TipoErro.NotaNaoEncontrada);

            nota.Cancelar(request.Motivo);

            if(!nota.EhValida)
                return Resultado<NotaOutputModel>.RetornaResultadoErro(TipoErro.NotaInvalida,
                       nota.Notificacoes.Select(notificacao => new DetalheErro(notificacao.Campo, notificacao.Mensagem)).ToList());

            await _notaRepository.Atualizar(nota, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return Resultado<NotaOutputModel>.RetornaResultadoSucesso(MapeadorAplicacao.NotaEmNotaOuputModel(nota));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Resultado<NotaOutputModel>.RetornaResultadoErro(TipoErro.ErroInesperado);
        }
    }
}
