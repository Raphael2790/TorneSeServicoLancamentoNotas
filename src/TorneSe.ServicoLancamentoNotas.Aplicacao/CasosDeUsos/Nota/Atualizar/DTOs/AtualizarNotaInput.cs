using MediatR;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.DTOs;

public record struct AtualizarNotaInput(int AlunoId, int AtividadeId, int ProfessorId, double ValorNota)
    : IRequest<NotaOutputModel>;
