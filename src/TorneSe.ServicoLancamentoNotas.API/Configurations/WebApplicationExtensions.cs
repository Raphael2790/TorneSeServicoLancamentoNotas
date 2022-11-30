using TorneSe.ServicoLancamentoNotas.API.Configurations.Swagger;
using TorneSe.ServicoLancamentoNotas.API.Middlewares;

namespace TorneSe.ServicoLancamentoNotas.API.Configurations;

public static class WebApplicationExtensions
{
    public static WebApplication UsarServicos(this WebApplication app)
    {
        app.UsarConfiguracoesSwagger();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseMiddleware<BuscaTenantMiddleware>();

        app.MapControllers();

        return app;
    }

    public static WebApplicationBuilder UseStartup<TStartup>(this WebApplicationBuilder applicationBuilder)
        where TStartup : class, IStartupApplication
    {
        if (Activator.CreateInstance(typeof(TStartup), applicationBuilder.Configuration) is not IStartupApplication startupApplication)
            throw new ArgumentException("Classe Startup.cs Inválida");

        Environment.SetEnvironmentVariable("TENANTS", "torne-se-csharp;torne-se-javascript;torne-se-java");
        Environment.SetEnvironmentVariable("CONNECTION_STRING_TORNESECSHARP", "connection_mysql");
        Environment.SetEnvironmentVariable("CONNECTION_STRING_TORNESEJAVA", "connection_mysql");
        Environment.SetEnvironmentVariable("CONNECTION_STRING_TORNESEJAVASCRIPT", "connection_mysql");

        startupApplication.ConfigureServices(applicationBuilder.Services);

        var app = applicationBuilder.Build();

        startupApplication.Configure(app, app.Environment);

        app.Run();

        return applicationBuilder;
    }
}
