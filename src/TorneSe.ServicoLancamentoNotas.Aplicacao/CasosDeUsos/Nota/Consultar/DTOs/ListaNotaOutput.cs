using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;

public record class ListaNotaOutput
    : ListaPaginadaOutput<NotaOutputModel>
{
    public ListaNotaOutput(int Pagina, int PorPagina, int Total, IReadOnlyList<NotaOutputModel> Items) 
        : base(Pagina, PorPagina, Total, Items)
    {
    }
}
