namespace TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;

public abstract class Entidade
{
    public Guid Id { get; set; }
    public DateTime DataAtualizacao { get; protected set; }

    protected Entidade()
    {
        Id = Guid.NewGuid();
    }
}
