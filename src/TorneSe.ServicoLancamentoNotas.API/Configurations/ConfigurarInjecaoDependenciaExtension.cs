using TorneSe.ServicoLancamentoNotas.API.Configurations.Swagger;
using TorneSe.ServicoLancamentoNotas.API.Extensions;
using TorneSe.ServicoLancamentoNotas.API.Filters;
using TorneSe.ServicoLancamentoNotas.API.Middlewares;
using TorneSe.ServicoLancamentoNotas.Infra.CrossCutting.IoC;

namespace TorneSe.ServicoLancamentoNotas.API.Configurations;

public static class ConfigurarInjecaoDependenciaExtension
{
    public static IServiceCollection ConfigurarServicos(this IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(ApiGlobalExceptionFilter));
        })
        .AdicionarSerializerContext();
        services.RegistrarServicos(environment, configuration);
        services.AddEndpointsApiExplorer();
        services.AddHttpContextAccessor();
        services.AddScoped<BuscaTenantMiddleware>();
        services.AdicionarConfiguracoesSwagger();
        return services;
    }
}
