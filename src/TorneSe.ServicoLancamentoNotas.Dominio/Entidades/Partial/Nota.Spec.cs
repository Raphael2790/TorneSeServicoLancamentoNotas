using TorneSe.ServicoLancamentoNotas.Dominio.Specifications;

namespace TorneSe.ServicoLancamentoNotas.Dominio.Entidades;

public partial class Nota
{
    private bool PodeAlterarStatusParaEnviado()
        => StatusAguardandoIntegracaoSpec.Instance
        .IsSatisfied(this);
}
