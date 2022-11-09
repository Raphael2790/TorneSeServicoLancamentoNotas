using Microsoft.EntityFrameworkCore;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar.DTOs;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Contexto;
using TornseSe.ServicoLancamentoNotas.TestesIntegracao.Base;

namespace TornseSe.ServicoLancamentoNotas.TestesIntegracao.Aplicacao.CasosDeUso.Nota.Cancelar;

[CollectionDefinition(nameof(CancelarNotaTestsFixture))]
public class CancelarNotaTestsFixtureCollection
    : ICollectionFixture<CancelarNotaTestsFixture>
{ }

public class CancelarNotaTestsFixture
    : BaseFixture
{
    public CancelarNotaInput RetornaInput(int? alunoId = null, int? atividadeId = null, string? motivo = null)
        => new(alunoId ?? RetornaNumeroIdRandomico(), atividadeId ?? RetornaNumeroIdRandomico()
            , RetornaNumeroIdRandomico(), motivo ?? string.Empty);

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
