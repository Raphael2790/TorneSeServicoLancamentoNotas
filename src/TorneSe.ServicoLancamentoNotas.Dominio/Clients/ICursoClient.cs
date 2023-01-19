using TorneSe.ServicoLancamentoNotas.Dominio.ValueObjects;

namespace TorneSe.ServicoLancamentoNotas.Dominio.Clients;

public interface ICursoClient
{
    Task<IEnumerable<Curso>>? ObterInformacoesCursoAluno(int alunoId, int professorId,
        int atividadeId, CancellationToken cancellationToken);
}
