using MediatR;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;
using TorneSe.ServicoLancamentoNotas.Dominio.Enums;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;

public record class ListaNotaInput
    : ListaPaginadaInput, IRequest<Resultado<ListaNotaOutput>>
{
    public ListaNotaInput(int Pagina, int PorPagina, int? AlunoId, int? AtividadeId, string OrdenarPor, OrdenacaoBusca Ordenacao = OrdenacaoBusca.Asc) 
        : base(Pagina, PorPagina, AlunoId, AtividadeId, OrdenarPor, Ordenacao)
    {
    }
}
