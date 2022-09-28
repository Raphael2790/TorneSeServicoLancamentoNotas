namespace TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;

public abstract record class ListaPaginadaOutput<TModel>
    (int Pagina, int PorPagina, int Total, IReadOnlyList<TModel> Items);
