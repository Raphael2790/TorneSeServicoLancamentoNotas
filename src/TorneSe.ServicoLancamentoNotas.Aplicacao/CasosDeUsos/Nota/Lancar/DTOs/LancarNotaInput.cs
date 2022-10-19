using MediatR;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs;

public record struct LancarNotaInput(int AlunoId, int AtividadeId, int ProfessorId, double ValorNota, bool NotaSubstitutiva)
    : IRequest<Resultado<NotaOutputModel>>;
