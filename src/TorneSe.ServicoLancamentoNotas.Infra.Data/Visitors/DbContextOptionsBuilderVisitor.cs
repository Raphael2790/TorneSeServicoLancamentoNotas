using Microsoft.EntityFrameworkCore;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Providers.Interfaces;

namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Visitors;

public class DbContextOptionsBuilderVisitor : IVisitor<DbContextOptionsBuilder>
{
    private readonly IVariaveisAmbienteProvider _variaveisAmbienteProvider;
    private readonly ITenantProvider _tenantProvider;

    public DbContextOptionsBuilderVisitor(IVariaveisAmbienteProvider variaveisAmbienteProvider, ITenantProvider tenantProvider)
    {
        _variaveisAmbienteProvider = variaveisAmbienteProvider;
        _tenantProvider = tenantProvider;
    }

    public DbContextOptionsBuilder Visit(DbContextOptionsBuilder objetoVisitado)
    {
        var tenant = _tenantProvider.Tenant;
        var connectionString = _variaveisAmbienteProvider.ObterConnectionStringPorTenant(tenant);
        if (!string.IsNullOrWhiteSpace(connectionString))
            objetoVisitado.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return objetoVisitado;
    }
}
