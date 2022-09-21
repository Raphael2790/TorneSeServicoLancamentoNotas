namespace TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;

public interface IGenericRepository<TAgregado> : IRepository
{
    Task Inserir(TAgregado agregado, CancellationToken cancellationToken);
    Task Atualizar(TAgregado agregado, CancellationToken cancellationToken);
}
