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
        services.AddSwaggerGen();
        return services;
    }
}
