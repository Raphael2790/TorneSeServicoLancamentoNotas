namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Providers.Interfaces;

public interface IVariaveisAmbienteProvider
{
    HashSet<string> Tenants { get; }
}
