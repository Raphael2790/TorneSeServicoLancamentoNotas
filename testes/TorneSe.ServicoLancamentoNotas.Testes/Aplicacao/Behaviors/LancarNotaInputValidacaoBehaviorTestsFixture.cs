using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;
using TorneSe.ServicoLancamentoNotas.Dominio.Enums;
using TorneSe.ServicoLancamentoNotas.Testes.Comum;

namespace TorneSe.ServicoLancamentoNotas.Testes.Aplicacao.Behaviors;

[CollectionDefinition(nameof(LancarNotaInputValidacaoBehaviorTestsFixture))]
public class LancarNotaInputValidacaoBehaviorTestsFixtureCollection
    : ICollectionFixture<LancarNotaInputValidacaoBehaviorTestsFixture>
{ }

public class LancarNotaInputValidacaoBehaviorTestsFixture
    : BaseFixture
{
    public LancarNotaInput DevolveNotaInputInvalido()
       => new(-1, -1, -1, 11,
           false);

    public LancarNotaInput DevolveNotaInputValido()
        => new(RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), RetornaValorNotaAleatorioValido(),
            false);

    public Task<Resultado<NotaOutputModel>> RetornaSucesso()
        => Task.FromResult(Resultado<NotaOutputModel>.RetornaResultadoSucesso(new NotaOutputModel(1, 1, 1, DateTime.Now, false, null, StatusIntegracao.AguardandoIntegracao)));
}
