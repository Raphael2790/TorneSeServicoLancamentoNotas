using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;
using TorneSe.ServicoLancamentoNotas.Dominio.Enums;
using TorneSe.ServicoLancamentoNotas.Testes.Comum;

namespace TorneSe.ServicoLancamentoNotas.Testes.Aplicacao.Behaviors;

[CollectionDefinition(nameof(InputValidacaoBehaviorTestsFixture))]
public class InputValidacaoBehaviorTestsFixtureCollection
    : ICollectionFixture<InputValidacaoBehaviorTestsFixture>
{ }

public class InputValidacaoBehaviorTestsFixture
    : BaseFixture
{
    public LancarNotaInput DevolveNotaInputInvalido()
       => new(-1, -1, -1, 11,
           false);

    public AtualizarNotaInput DevolveAtualizarNotaInputInvalido()
        => new(-1, -1, -1, 11);

    public LancarNotaInput DevolveNotaInputValido()
        => new(RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), RetornaValorNotaAleatorioValido(),
            false);

    public AtualizarNotaInput DevolveNotaAtualizarInputValido()
        => new(RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), RetornaValorNotaAleatorioValido());

    public Task<Resultado<NotaOutputModel>> RetornaSucesso()
        => Task.FromResult(Resultado<NotaOutputModel>.RetornaResultadoSucesso(new NotaOutputModel(1, 1, 1, DateTime.Now, false, null, StatusIntegracao.AguardandoIntegracao)));
}
