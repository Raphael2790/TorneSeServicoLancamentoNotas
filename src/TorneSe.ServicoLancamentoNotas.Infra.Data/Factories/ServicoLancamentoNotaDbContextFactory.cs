using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Exceptions;
using TorneSe.ServicoLancamentoNotas.Dominio.ValueObjects;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Contexto;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Providers;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Visitors;

namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Factories;

public class ServicoLancamentoNotaDbContextFactory : IDesignTimeDbContextFactory<ServicoLancamentoNotaDbContext>
{
    public ServicoLancamentoNotaDbContext CreateDbContext(string[] args)
    {
        Environment.SetEnvironmentVariable("TENANTS", "torne-se-csharp;torne-se-javascript;torne-se-java");
        Environment.SetEnvironmentVariable("CONNECTION_STRING_TORNESECSHARP", "connection_mysql");
        Environment.SetEnvironmentVariable("CONNECTION_STRING_TORNESEJAVA", "connection_mysql");
        Environment.SetEnvironmentVariable("CONNECTION_STRING_TORNESEJAVASCRIPT", "connection_mysql");

        //-args torne-se-java
        Tenant tenant = args.Last();
        var variaveisAmbienteProvider = VariaveisAmbienteProvider.Instance;
        var tenantProvider = new TenantProvider(null!, variaveisAmbienteProvider);
        if (!tenantProvider.ValidarTenant(tenant))
            throw new TenantInvalidoException(tenant);

        tenantProvider.AtribuirTenant(tenant);

        var optionsBuilder = new DbContextOptionsBuilder<ServicoLancamentoNotaDbContext>();
        var visitor = new DbContextOptionsBuilderVisitor(variaveisAmbienteProvider, tenantProvider);
        visitor.Visit(optionsBuilder);
        return new ServicoLancamentoNotaDbContext(optionsBuilder.Options);
    }
}
