using TorneSe.ServicoLancamentoNotas.Dominio.Clients;
using TorneSe.ServicoLancamentoNotas.Dominio.ValueObjects;
using TornseSe.ServicoLancamentoNotas.TestesIntegracao.FakeComponents.Fakers;

namespace TornseSe.ServicoLancamentoNotas.TestesIntegracao.FakeComponents.CursoClient;

public class CursoFakeClient : ICursoClient
{
    public Task<IEnumerable<Curso>>? ObterInformacoesCursoAluno(int alunoId, int professorId, int atividadeId, CancellationToken cancellationToken)
        => Task.FromResult(CursoFake.ObterCursoAluno(atividadeId, alunoId, professorId));
}
