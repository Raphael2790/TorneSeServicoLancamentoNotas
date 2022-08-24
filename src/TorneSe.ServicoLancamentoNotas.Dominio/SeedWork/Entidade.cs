namespace TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;

public abstract class Entidade : NotifiableObject
{
    public Guid Id { get; set; }
    public DateTime DataAtualizacao { get; protected set; }
    public DateTime DataCriacao { get; protected set; }

    protected Entidade()
    {
        Id = Guid.NewGuid();
    }
}
