namespace TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;

public abstract class Evento 
{
    public string CorrelationId { get; }

	protected Evento()
	{
		CorrelationId = Guid.NewGuid().ToString();
	}
}
