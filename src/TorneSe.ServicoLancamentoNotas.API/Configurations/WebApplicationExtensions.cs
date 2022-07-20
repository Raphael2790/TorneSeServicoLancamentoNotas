namespace TorneSe.ServicoLancamentoNotas.API.Configurations;

public static class WebApplicationExtensions
{
    public static WebApplication UsarServicos(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }

    public static WebApplicationBuilder UseStartup<TStartup>(this WebApplicationBuilder applicationBuilder)
        where TStartup : class, IStartupApplication
    {
        if (Activator.CreateInstance(typeof(TStartup), applicationBuilder.Configuration) is not IStartupApplication startupApplication)
            throw new ArgumentException("Classe Startup.cs Inválida");

        startupApplication.ConfigureServices(applicationBuilder.Services);

        var app = applicationBuilder.Build();

        startupApplication.Configure(app, app.Environment);

        app.Run();

        return applicationBuilder;
    }
}
