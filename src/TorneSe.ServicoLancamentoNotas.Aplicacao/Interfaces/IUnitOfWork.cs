namespace TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;

public interface IUnitOfWork
{
    Task<bool> Commit(CancellationToken cancellationToken);
    Task Rollback(CancellationToken cancellationToken);
}
