using Microsoft.EntityFrameworkCore;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;
using TorneSe.ServicoLancamentoNotas.Dominio.Enums;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Contexto;
using TornseSe.ServicoLancamentoNotas.TestesIntegracao.Base;
using DomainEntities = TorneSe.ServicoLancamentoNotas.Dominio.Entidades;

namespace TornseSe.ServicoLancamentoNotas.TestesIntegracao.Aplicacao.CasosDeUso.Nota.Consultar;

[CollectionDefinition(nameof(ConsultarNotaTestsFixture))]
public class ConsultaNotaTestsFixtureCollection
    : ICollectionFixture<ConsultarNotaTestsFixture>
{ }

public class ConsultarNotaTestsFixture
    : BaseFixture
{
    public ServicoLancamentoNotaDbContext CriarDbContext()
    {
        var dbContext = new ServicoLancamentoNotaDbContext
            (
                new DbContextOptionsBuilder<ServicoLancamentoNotaDbContext>()
                    .UseInMemoryDatabase("integration-tests")
                    .Options
            );

        return dbContext;
    }

    public ListaNotaInput RetornaListBuscaInput()
        => new(1, 10, null, null, "");

    public List<DomainEntities.Nota> RetornaNotasValidas()
        => Enumerable.Range(1, 20).Select(id => RetornaNota(id)).ToList();

    public ListaNotaInput RetornaBuscaInputApenasComPaginacao(int? alunoId = null,
                                                          int? atividadeId = null, OrdenacaoBusca ordenacao = OrdenacaoBusca.Asc,
                                                          string ordernarPor = "", int? pagina = null, int? porPagina = null)
        => new(pagina ?? 1, porPagina ?? 10, alunoId ?? null, atividadeId ?? null, ordernarPor, ordenacao);

    public List<DomainEntities.Nota> NotasOrdernadas(IEnumerable<DomainEntities.Nota> lista, string ordenarPor, OrdenacaoBusca ordenacao)
        => (ordenacao, ordenarPor.ToLower()) switch
        {
            (OrdenacaoBusca.Asc, "atividadeid") => lista.OrderBy(x => x.AtividadeId).ToList(),
            (OrdenacaoBusca.Desc, "atividadeid") => lista.OrderByDescending(x => x.AtividadeId).ToList(),
            (OrdenacaoBusca.Asc, "alunoid") => lista.OrderBy(x => x.AlunoId).ToList(),
            (OrdenacaoBusca.Desc, "alunoid") => lista.OrderByDescending(x => x.AlunoId).ToList(),
            _ => lista.OrderBy(x => x.AlunoId).ToList()
        };
}
