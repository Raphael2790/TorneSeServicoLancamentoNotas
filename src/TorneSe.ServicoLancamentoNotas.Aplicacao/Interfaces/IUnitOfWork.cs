namespace TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;

public interface IUnitOfWork
{
    Task Commit(CancellationToken cancellationToken);
    Task Rollback(CancellationToken cancellationToken);
}
