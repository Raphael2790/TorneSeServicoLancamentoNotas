using MediatR;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;
using TorneSe.ServicoLancamentoNotas.Dominio.Enums;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;

public record class ListaNotaInput
    : ListaPaginadaInput, IRequest<Resultado<ListaNotaOutput>>
{
    public ListaNotaInput(int Pagina = 1, int PorPagina = 10, int? AlunoId = null, int? AtividadeId = null, string OrdenarPor = "", OrdenacaoBusca Ordenacao = OrdenacaoBusca.Asc) 
        : base(Pagina, PorPagina, AlunoId, AtividadeId, OrdenarPor, Ordenacao)
    {
    }
}
