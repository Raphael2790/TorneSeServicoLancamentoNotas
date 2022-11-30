using TorneSe.ServicoLancamentoNotas.Dominio.ValueObjects;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.Exceptions;

public class TenantInvalidoException : ApplicationException
{
    public TenantInvalidoException(Tenant tenant) : base($"O tenant {tenant} não é válido") {}
}
