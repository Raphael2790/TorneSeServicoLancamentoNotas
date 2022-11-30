using TorneSe.ServicoLancamentoNotas.API.Configurations.Swagger;
using TorneSe.ServicoLancamentoNotas.API.Middlewares;
using TorneSe.ServicoLancamentoNotas.Infra.CrossCutting.IoC;

namespace TorneSe.ServicoLancamentoNotas.API.Configurations;

public static class ConfigurarInjecaoDependenciaExtension
{
    public static IServiceCollection ConfigurarServicos(this IServiceCollection services)
    {
        services.AddControllers();
        services.RegistrarServicos();
        services.AddEndpointsApiExplorer();
        services.AddHttpContextAccessor();
        services.AddScoped<BuscaTenantMiddleware>();
        services.AdicionarConfiguracoesSwagger();
        return services;
    }
}
