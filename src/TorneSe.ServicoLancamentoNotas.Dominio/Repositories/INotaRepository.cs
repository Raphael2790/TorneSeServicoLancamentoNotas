using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;

namespace TorneSe.ServicoLancamentoNotas.Dominio.Repositories;

public interface INotaRepository : IGenericRepository<Nota> 
{
    Task<Nota> BuscarNotaPorAlunoEAtividade(int alunoId, int atividadeId, CancellationToken cancellationToken);
}
