using Microsoft.Extensions.Logging;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Enums;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Mapeadores;
using TorneSe.ServicoLancamentoNotas.Dominio.Clients;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar;

public class AtualizarNota : NotaHandler, IAtualizarNota
{
    private readonly INotaRepository _notaRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AtualizarNota> _logger;

    public AtualizarNota(INotaRepository notaRepository,
                         IUnitOfWork unitOfWork,
                         ILogger<AtualizarNota> logger,
                         ICursoClient cursoClient) : base(cursoClient)
    {
        _notaRepository = notaRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Resultado<NotaOutputModel>> Handle(AtualizarNotaInput request, CancellationToken cancellationToken)
    {
        try
        {
            var nota = await _notaRepository.BuscarNotaPorAlunoEAtividade(request.AlunoId, request.AtividadeId, cancellationToken);

            if (nota is null)
                return Resultado<NotaOutputModel>.RetornaResultadoErro(TipoErro.RecursoNaoEncontrado);

            nota.AtualizarValorNota(request.ValorNota);

            if (!nota.EhValida)
                return Resultado<NotaOutputModel>.RetornaResultadoErro(TipoErro.NotaInvalida,
                    nota.Notificacoes.Select(notificacao => new DetalheErro(notificacao.Campo, notificacao.Mensagem)).ToList());

            var (valido, detalhes) = await ValidarInformacoesAlunoCurso(
                new ValidacaoCursoInput(request.AlunoId, request.ProfessorId, request.AtividadeId), cancellationToken);

            if (!valido && detalhes.Any())
                return Resultado<NotaOutputModel>.RetornaResultadoErro(TipoErro.NaoFoiPossivelValidarVinculosCurso, detalhes);

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
