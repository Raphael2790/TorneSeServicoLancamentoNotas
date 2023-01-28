using Polly;

namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Factories.Interfaces;

public interface ICursoClientResilienciaFactory
{
    IAsyncPolicy<HttpResponseMessage> Retry();
    IAsyncPolicy<HttpResponseMessage> Timeout();
    IAsyncPolicy<HttpResponseMessage> CircuitBreak();
}
