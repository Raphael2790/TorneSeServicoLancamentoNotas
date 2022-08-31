using System.Linq.Expressions;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.Enums;
using TorneSe.ServicoLancamentoNotas.Dominio.Specifications.Base;

namespace TorneSe.ServicoLancamentoNotas.Dominio.Specifications;

public class StatusEnviadaParaIntegracaoSpec : Specification<Nota>
{
    public static readonly StatusEnviadaParaIntegracaoSpec Instance = new();

    public override Expression<Func<Nota, bool>> ToExpression()
        => nota => nota.StatusIntegracao == StatusIntegracao.EnviadaParaIntegracao;
}
