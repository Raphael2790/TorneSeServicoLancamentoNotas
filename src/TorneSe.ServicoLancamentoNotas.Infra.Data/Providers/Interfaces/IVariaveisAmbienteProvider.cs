using TorneSe.ServicoLancamentoNotas.Dominio.ValueObjects;

namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Providers.Interfaces;

public interface IVariaveisAmbienteProvider
{
    HashSet<string> Tenants { get; }
    string? ObterConnectionStringPorTenant(Tenant tenant);
    string? UrlBaseCursos { get; }
    string? PathObterCursos { get; }
    int ErrosAntesDeAbrirCircuito { get; }
    TimeSpan DuracaoCircuito { get; }
    int NumeroRetentativas { get; }
    int Timeout { get; }
    int WaitTimeSeconds { get; }
    string? AwsAccessKey { get; }
    string? AwsSecretAccessKey { get; }
    string? ObterNomeFila(string nomeFila);
    string ElasticSearchUrl { get; }
}
