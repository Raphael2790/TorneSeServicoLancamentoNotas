using Microsoft.EntityFrameworkCore;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.DTOs;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Contexto;
using TornseSe.ServicoLancamentoNotas.TestesIntegracao.Base;

namespace TornseSe.ServicoLancamentoNotas.TestesIntegracao.Aplicacao.CasosDeUso.Nota.Atualizar;

[CollectionDefinition(nameof(AtualizarNotaTestsFixture))]
public class AtualizarNotaFixtureCollection
    : ICollectionFixture<AtualizarNotaTestsFixture>
{ }

public class AtualizarNotaTestsFixture
    : BaseFixture
{
    public AtualizarNotaInput RetornaInput(int? alunoId = null, int? atividadeId = null, double? valorNota = null)
        => new(alunoId ?? RetornaNumeroIdRandomico(), atividadeId ?? RetornaNumeroIdRandomico(), 
            RetornaNumeroIdRandomico(),valorNota ?? RetornaValorNotaAleatorioValido());

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
