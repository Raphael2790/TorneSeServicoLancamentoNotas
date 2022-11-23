using Microsoft.AspNetCore.Http;
using TorneSe.ServicoLancamentoNotas.Dominio.ValueObjects;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Constantes;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Providers.Interfaces;

namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Providers;

public class TenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _accessor;
    private readonly IVariaveisAmbienteProvider _variaveisAmbienteProvider;

    public Tenant Tenant { get; private set; }

    public TenantProvider(IHttpContextAccessor accessor, IVariaveisAmbienteProvider variaveisAmbienteProvider)
    {
        _accessor = accessor;
        _variaveisAmbienteProvider = variaveisAmbienteProvider;
    }

    public void AtribuirTenant(Tenant tenant)
    {
        Tenant = tenant;
    }

    public Tenant ObterTenant()
    {
        var origem = ObterViaHeader();
        if (string.IsNullOrWhiteSpace(origem))
            origem = ObterViaPath();
        return string.IsNullOrWhiteSpace(origem) ? new Tenant(string.Empty) : new Tenant(origem);
    }

    private string? ObterViaPath()
    {
        var path = _accessor?.HttpContext?.Request?.Path;
        if(string.IsNullOrWhiteSpace(path))
            return default;

        var pathSplit = path.ToString()!.Split('/', StringSplitOptions.RemoveEmptyEntries);

        return pathSplit.Last();
    }

    private string? ObterViaHeader()
    {
        var origem = _accessor?.HttpContext?.Request?.Headers?[RequestConstantes.ORIGEM];
        return !string.IsNullOrWhiteSpace(origem) ? origem.ToString()!.ToLowerInvariant() : default; 
    }

    public bool ValidarTenant(Tenant tenant)
    {
        var tenants = _variaveisAmbienteProvider.Tenants;
        return !string.IsNullOrWhiteSpace(tenant) && tenants.Contains(tenant);
    }
}
