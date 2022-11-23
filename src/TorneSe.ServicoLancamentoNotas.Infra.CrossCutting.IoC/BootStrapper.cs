using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Mediator;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Contexto;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Providers;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Providers.Interfaces;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Repositories;
using TorneSe.ServicoLancamentoNotas.Infra.Data.UoW;

namespace TorneSe.ServicoLancamentoNotas.Infra.CrossCutting.IoC;

public static class BootStrapper
{
    public static void RegistrarServicos(this IServiceCollection services)
    {
        services
            .RegistrarRepositorios()
            .RegistrarDbContext()
            .RegistrarUnitOfWork()
            .RegistrarHandlers()
            .RegistrarMediator()
            .RegistrarProviders();
    }

    private static IServiceCollection RegistrarRepositorios(this IServiceCollection services)
        => services.AddScoped<INotaRepository, NotaRepository>();

    private static IServiceCollection RegistrarDbContext(this IServiceCollection services)
        => services.AddDbContext<ServicoLancamentoNotaDbContext>(options => 
        {
            options.UseInMemoryDatabase("db-teste-in-memory");
        });

    private static IServiceCollection RegistrarHandlers(this IServiceCollection services)
        => services.AddMediatR(typeof(ConsultaNota));

    private static IServiceCollection RegistrarUnitOfWork(this IServiceCollection services)
        => services.AddScoped<IUnitOfWork, UnitOfWork>();

    private static IServiceCollection RegistrarMediator(this IServiceCollection services)
        => services.AddScoped<IMediatorHandler, MediatorHandler>();

    private static IServiceCollection RegistrarProviders(this IServiceCollection services)
        => services.AddScoped<ITenantProvider, TenantProvider>()
                   .AddScoped<IVariaveisAmbienteProvider, VariaveisAmbienteProvider>(); 
}
