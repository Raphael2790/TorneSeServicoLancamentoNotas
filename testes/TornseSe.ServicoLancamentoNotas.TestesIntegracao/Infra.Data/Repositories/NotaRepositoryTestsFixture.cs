using TornseSe.ServicoLancamentoNotas.TestesIntegracao.Base;

namespace TornseSe.ServicoLancamentoNotas.TestesIntegracao.Infra.Data.Repositories;

[CollectionDefinition(nameof(NotaRepositoryTestsFixture))]
public class NotaRepositoryTestsFixtureCollection
    : ICollectionFixture<NotaRepositoryTestsFixture>
{ }

public class NotaRepositoryTestsFixture
    : BaseFixture
{
}
