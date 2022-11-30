namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Visitors;

public interface IVisitor<TObject>
{
    TObject Visit(TObject objetoVisitado);
}
