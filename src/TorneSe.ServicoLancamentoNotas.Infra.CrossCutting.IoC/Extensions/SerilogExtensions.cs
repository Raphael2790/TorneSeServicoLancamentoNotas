using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Providers;

namespace TorneSe.ServicoLancamentoNotas.Infra.CrossCutting.IoC.Extensions;

public static class SerilogExtensions
{
    public static IServiceCollection ConfigurarSerilog(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", configuration["Application:ApplicationName"])
            .Enrich.WithProperty("Environment", hostEnvironment.EnvironmentName)
            .Enrich.WithExceptionDetails()
            .Enrich.WithMachineName()
            .WriteTo.Console()
            //.WriteTo.Elasticsearch(GetElasticsearchSinkOptions(configuration, hostEnvironment))
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        return services;
    }

    //private static ElasticsearchSinkOptions GetElasticsearchSinkOptions(IConfiguration configuration, IHostEnvironment hostEnvironment) =>
    //    new(new Uri(VariaveisAmbienteProvider.Instance.ElasticSearchUrl))
    //    {
    //        MinimumLogEventLevel = LogEventLevel.Information,
    //        IndexFormat = $"{configuration["Application:ApplicationName"]}-logs-{hostEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}",
    //        AutoRegisterTemplate = true,
    //        NumberOfShards = 2,
    //        NumberOfReplicas = 1,
    //    };
}
