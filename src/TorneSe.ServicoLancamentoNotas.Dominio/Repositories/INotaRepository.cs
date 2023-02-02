using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork.BuscaRepository;

namespace TorneSe.ServicoLancamentoNotas.Dominio.Repositories;

public interface INotaRepository : IGenericRepository<Nota>, IBuscaRepository<Nota>
{
    Task<Nota?> BuscarNotaPorAlunoEAtividade(int alunoId, int atividadeId, CancellationToken cancellationToken);
    Task<bool> ExisteNotaCanceladaPorAlunoEAtividade(int alunoId, int atividadeId, CancellationToken cancellationToken);
}
