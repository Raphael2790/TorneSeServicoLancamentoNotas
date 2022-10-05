using MediatR;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.DTOs;

public record struct AtualizarNotaInput(int AlunoId, int AtividadeId, int ProfessorId, double ValorNota)
    : IRequest<Resultado<NotaOutputModel>>;
