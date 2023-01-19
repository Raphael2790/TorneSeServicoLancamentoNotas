using TorneSe.ServicoLancamentoNotas.Dominio.ValueObjects;

namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Providers.Interfaces;

public interface IVariaveisAmbienteProvider
{
    HashSet<string> Tenants { get; }
    string? ObterConnectionStringPorTenant(Tenant tenant);
    string? UrlBaseCursos { get; }
    string? PathObterCursos { get; }
}
