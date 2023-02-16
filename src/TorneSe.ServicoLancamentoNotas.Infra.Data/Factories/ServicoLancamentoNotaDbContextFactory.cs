using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Exceptions;
using TorneSe.ServicoLancamentoNotas.Dominio.ValueObjects;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Configuracoes;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Contexto;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Providers;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Visitors;

namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Factories;

public class ServicoLancamentoNotaDbContextFactory : IDesignTimeDbContextFactory<ServicoLancamentoNotaDbContext>
{
    public ServicoLancamentoNotaDbContext CreateDbContext(string[] args)
    {
        ArquivoEnv.CarregarVariaveis();

        //-args torne-se-java
        Tenant tenant = args[^1];
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
