using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Behaviors;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Mediator;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Validacoes;
using TorneSe.ServicoLancamentoNotas.Dominio.Clients;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.Curso;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Contexto;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Providers;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Providers.Interfaces;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Repositories;
using TorneSe.ServicoLancamentoNotas.Infra.Data.UoW;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Visitors;

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
            .RegistrarProviders()
            .RegistrarValidacoes()
            .RegistrarComportamentos()
            .RegistrarClients();
    }

    private static IServiceCollection RegistrarRepositorios(this IServiceCollection services)
        => services.AddScoped<INotaRepository, NotaRepository>();

    private static IServiceCollection RegistrarDbContext(this IServiceCollection services)
        => services
        .AddTransient<IVisitor<DbContextOptionsBuilder>, DbContextOptionsBuilderVisitor>()
        .AddDbContext<ServicoLancamentoNotaDbContext>((provider,options) => 
        {
            provider.GetRequiredService<IVisitor<DbContextOptionsBuilder>>().Visit(options);
            //options.UseInMemoryDatabase("db-teste-in-memory");
        });

    private static IServiceCollection RegistrarHandlers(this IServiceCollection services)
        => services.AddMediatR(typeof(ConsultaNota));

    private static IServiceCollection RegistrarValidacoes(this IServiceCollection services)
        => services.AddValidatorsFromAssembly(typeof(LancarNotaInputValidator).Assembly);

    private static IServiceCollection RegistrarComportamentos(this IServiceCollection services)
        => services
            //.AddScoped<IPipelineBehavior<LancarNotaInput, Resultado<NotaOutputModel>>, LancarNotaInputValidacaoBehavior>()
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidacaoInputBehavior<,>));

    private static IServiceCollection RegistrarUnitOfWork(this IServiceCollection services)
        => services.AddScoped<IUnitOfWork, UnitOfWork>();

    private static IServiceCollection RegistrarMediator(this IServiceCollection services)
        => services.AddScoped<IMediatorHandler, MediatorHandler>();

    private static IServiceCollection RegistrarProviders(this IServiceCollection services)
        => services.AddScoped<ITenantProvider, TenantProvider>()
                   .AddScoped<IVariaveisAmbienteProvider, VariaveisAmbienteProvider>();

    private static IServiceCollection RegistrarClients(this IServiceCollection services)
    {
        services.AddHttpClient<ICursoClient, CursoClient>("curso");

        return services;
    }
}
