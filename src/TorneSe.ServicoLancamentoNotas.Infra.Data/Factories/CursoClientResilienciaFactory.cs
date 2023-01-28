using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Factories.Interfaces;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Providers.Interfaces;

namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Factories;

public class CursoClientResilienciaFactory
    : ICursoClientResilienciaFactory
{
    private readonly IVariaveisAmbienteProvider _variaveisAmbienteProvider;

    public CursoClientResilienciaFactory(IVariaveisAmbienteProvider variaveisAmbienteProvider) 
        => _variaveisAmbienteProvider = variaveisAmbienteProvider;

    public IAsyncPolicy<HttpResponseMessage> CircuitBreak()
        => CriarPolicy()
           .CircuitBreakerAsync(
            _variaveisAmbienteProvider.ErrosAntesDeAbrirCircuito, 
            _variaveisAmbienteProvider.DuracaoCircuito);

    public IAsyncPolicy<HttpResponseMessage> Retry()
        => CriarPolicy()
           .WaitAndRetryAsync(_variaveisAmbienteProvider.NumeroRetentativas, contador =>
           {
               var tempoEspera = TimeSpan.FromSeconds(Math.Pow(2,contador));
               return tempoEspera;
           });

    public IAsyncPolicy<HttpResponseMessage> Timeout()
        => Policy.TimeoutAsync<HttpResponseMessage>(_variaveisAmbienteProvider.Timeout, TimeoutStrategy.Optimistic);

    private static PolicyBuilder<HttpResponseMessage> CriarPolicy()
        => HttpPolicyExtensions
        .HandleTransientHttpError()
        .Or<TimeoutRejectedException>();
}
