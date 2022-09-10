using TorneSe.ServicoLancamentoNotas.Dominio.Specifications;

namespace TorneSe.ServicoLancamentoNotas.Dominio.Entidades;

public partial class Nota
{
    private bool PodeAlterarStatusParaEnviado()
        => StatusAguardandoIntegracaoSpec.Instance
        .IsSatisfied(this);

    private bool PodeAlterarStatusParaFalhaIntegracao()
        => StatusEnviadaParaIntegracaoSpec.Instance
        .IsSatisfied(this);

    private bool PodeAlterarStatusParaIntegradaComSucesso()
        => StatusEnviadaParaIntegracaoSpec.Instance
        .IsSatisfied(this);
}
