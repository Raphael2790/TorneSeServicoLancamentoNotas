using Microsoft.EntityFrameworkCore;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Contexto;
using TornseSe.ServicoLancamentoNotas.TestesIntegracao.Base;

namespace TornseSe.ServicoLancamentoNotas.TestesIntegracao.Infra.Data.UoW;

[CollectionDefinition(nameof(UnitOfWorkTestsFixture))]
public class UnitOfWorkTestsFixtureCollection
    : ICollectionFixture<UnitOfWorkTestsFixture>
{ }

public class UnitOfWorkTestsFixture
    : BaseFixture
{
    public List<Nota> RetornarNotas(int? quantidadeGerada = null)
       => Enumerable.Range(1, quantidadeGerada ?? 10).Select(id => RetornaNota(id)).ToList();

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
}
