namespace TorneSe.ServicoLancamentoNotas.Dominio.SeedWork.BuscaRepository;

public interface IBuscaRepository<TAgregado>
    where TAgregado : IRaizAgregacao
{
    Task<BuscaOutput<TAgregado>> Buscar(BuscaInput input, CancellationToken cancellationToken);
    Task<TAgregado?> Buscar(Guid id, CancellationToken cancellation);
}
