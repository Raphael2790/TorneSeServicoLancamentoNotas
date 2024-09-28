using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Behaviors;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Mediator;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Validacoes;
using TorneSe.ServicoLancamentoNotas.Dominio.Clients;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;
using TorneSe.ServicoLancamentoNotas.Infra.CrossCutting.IoC.Extensions;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.Curso;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.SerializerContext;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.SQS;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.SQS.Contexto;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.SQS.Contexto.Interface;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Configuracoes;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Contexto;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Factories;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Factories.Interfaces;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Providers;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Providers.Interfaces;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Repositories;
using TorneSe.ServicoLancamentoNotas.Infra.Data.UoW;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Visitors;
using TorneSe.ServicoLancamentoNotas.Infra.HealthCheck;

namespace TorneSe.ServicoLancamentoNotas.Infra.CrossCutting.IoC;

public static class BootStrapper
{
    public static void RegistrarServicos(this IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
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
            .RegistrarClients()
            .RegistrarSerializers()
            .RegistrarContextoSqs()
            .RegistrarMensagemClients()
            .CarregarVariaveisDeAmbiente(environment.IsDevelopment())
            .AdicionarConfiguracoesHealthChecks()
            .ConfigurarSerilog(configuration, environment);
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
        => services.AddMediatR(opt =>
        {
            opt.RegisterServicesFromAssembly(typeof(ConsultaNota).Assembly);
        });

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
                   .AddSingleton<IVariaveisAmbienteProvider, VariaveisAmbienteProvider>();

    private static IServiceCollection RegistrarClients(this IServiceCollection services)
    {
        services.AddSingleton<ICursoClientResilienciaFactory, CursoClientResilienciaFactory>();

        services.AddHttpClient<ICursoClient, CursoClient>()
            .AddPolicyHandler((prov, _) => prov.GetService<ICursoClientResilienciaFactory>()?.CircuitBreak())
            .AddPolicyHandler((prov, _) => prov.GetService<ICursoClientResilienciaFactory>()?.Retry())
            .AddPolicyHandler((prov, _) => prov.GetService<ICursoClientResilienciaFactory>()?.Timeout());

        return services;
    }

    private static IServiceCollection RegistrarSerializers(this IServiceCollection services)
        => services.AddSingleton(_ =>
        {
            return new CursoSerializerContext(new JsonSerializerOptions(JsonSerializerDefaults.Web));
        });

    private static IServiceCollection RegistrarContextoSqs(this IServiceCollection services)
        => services.AddSingleton<ISqsContexto, SqsContexto>();

    private static IServiceCollection RegistrarMensagemClients(this IServiceCollection services)
        => services.AddScoped<INotaLancadaMensagemClient, NotaLancadaMensagemClient>()
                   .AddScoped<INotaCanceladaMensagemClient, NotaCanceladaMensagemClient>()
                   .AddScoped<INotaAtualizadaMensagemClient, NotaAtualizadaMensagemClient>();

    private static IServiceCollection CarregarVariaveisDeAmbiente(this IServiceCollection services, bool ehDesenvolvimento) 
    {
        if (ehDesenvolvimento)
            ArquivoEnv.CarregarVariaveis();

        return services;
    }
}
