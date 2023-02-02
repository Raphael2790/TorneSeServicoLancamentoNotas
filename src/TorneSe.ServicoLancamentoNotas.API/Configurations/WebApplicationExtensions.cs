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
        Environment.SetEnvironmentVariable("CONNECTION_STRING_TORNESECSHARP", "Server=localhost;Database=TorneSeCsharp;Port=3306;Uid=root;Pwd=root;Pooling=True;");
        Environment.SetEnvironmentVariable("CONNECTION_STRING_TORNESEJAVA", "Server=localhost;Database=TorneSeJava;Port=3306;Uid=root;Pwd=root;Pooling=True;");
        Environment.SetEnvironmentVariable("CONNECTION_STRING_TORNESEJAVASCRIPT", "Server=localhost;Database=TorneSeJavascript;Port=3306;Uid=root;Pwd=root;Pooling=True;");
        Environment.SetEnvironmentVariable("URL_BASE_CURSOS", "https://localhost:7295/");
        Environment.SetEnvironmentVariable("PATH_OBTER_CURSOS", "obterCurso");
        Environment.SetEnvironmentVariable("TIMEOUT", "7");
        Environment.SetEnvironmentVariable("NUMERO_RETENTATIVAS", "3");
        Environment.SetEnvironmentVariable("DURACAO_CIRCUITO_ABERTO", "30");
        Environment.SetEnvironmentVariable("NUMERO_ERROS_PARA_ABERTURA_CIRCUITO", "6");
        Environment.SetEnvironmentVariable("NOTAS_LANCADAS_QUEUE", "Nota_Lancada_HLG");
        Environment.SetEnvironmentVariable("AWS_LONGPOOLING", "20");
        Environment.SetEnvironmentVariable("AWS_ACCESS_KEY", "valor");
        Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", "valor");

        startupApplication.ConfigureServices(applicationBuilder.Services);

        var app = applicationBuilder.Build();

        startupApplication.Configure(app, app.Environment);

        app.Run();

        return applicationBuilder;
    }
}
