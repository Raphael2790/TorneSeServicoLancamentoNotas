using TorneSe.ServicoLancamentoNotas.Infra.Data.Constantes;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Providers.Interfaces;

namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Providers;

public class VariaveisAmbienteProvider : IVariaveisAmbienteProvider
{
    public HashSet<string> Tenants => _tenants;

    private readonly HashSet<string> _tenants;

    public VariaveisAmbienteProvider()
    {
        _tenants = new HashSet<string>(RetornaTenants());
    }

    private IEnumerable<string> RetornaTenants()
    {
        var tenants = Buscar(VariaveisAmbienteConstantes.TENANTS);

        return new HashSet<string>(tenants!.Split(VariaveisAmbienteConstantes.SEPARADOR_TENANTS,
            StringSplitOptions.RemoveEmptyEntries));
    }

    private string? Buscar(string nome)
        => Environment.GetEnvironmentVariable(nome);
}
