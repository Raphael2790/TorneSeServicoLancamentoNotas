namespace TorneSe.ServicoLancamentoNotas.API.Configurations;

public class Startup : IStartupApplication
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
    {
       services.ConfigurarServicos(environment, configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        app.UsarServicos();
    }
}
