using System.Linq.Expressions;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.Enums;
using TorneSe.ServicoLancamentoNotas.Dominio.Specifications.Base;

namespace TorneSe.ServicoLancamentoNotas.Dominio.Specifications;

public class StatusAguardandoIntegracaoSpec : Specification<Nota>
{
    public static readonly StatusAguardandoIntegracaoSpec Instance = new();

    public override Expression<Func<Nota, bool>> ToExpression()
        => nota => nota.StatusIntegracao == StatusIntegracao.AguardandoIntegracao;
}
