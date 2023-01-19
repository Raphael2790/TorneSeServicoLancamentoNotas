using TorneSe.ServicoLancamentoNotas.Dominio.ValueObjects;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Constantes;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Providers.Interfaces;

namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Providers;

public class VariaveisAmbienteProvider : IVariaveisAmbienteProvider
{
    public static readonly VariaveisAmbienteProvider Instance = new VariaveisAmbienteProvider();

    public HashSet<string> Tenants => _tenants;

    public string? UrlBaseCursos => Buscar(VariaveisAmbienteConstantes.URL_BASE_CURSOS);

    public string? PathObterCursos => Buscar(VariaveisAmbienteConstantes.PATH_OBTER_CURSOS);

    private readonly HashSet<string> _tenants;

    public VariaveisAmbienteProvider()
    {
        _tenants = new HashSet<string>(RetornaTenants());
    }

    public string? ObterConnectionStringPorTenant(Tenant tenant)
    {
        var nomeTenant = tenant.ToString().Replace("-", "").ToUpperInvariant();
        var varivelAmbiente = $"{VariaveisAmbienteConstantes.CONNECTION_STRING_PREFIX}{nomeTenant}";
        return Buscar(varivelAmbiente);
    }

    private IEnumerable<string> RetornaTenants()
    {
        var tenants = Buscar(VariaveisAmbienteConstantes.TENANTS);

        if (string.IsNullOrWhiteSpace(tenants))
            return new HashSet<string>();

        return new HashSet<string>(tenants!.Split(VariaveisAmbienteConstantes.SEPARADOR_TENANTS,
            StringSplitOptions.RemoveEmptyEntries));
    }

    private string? Buscar(string nome)
        => Environment.GetEnvironmentVariable(nome);
}
