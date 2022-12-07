using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Enums;
using TorneSe.ServicoLancamentoNotas.Dominio.Enums;
using TorneSe.ServicoLancamentoNotas.Testes.Comum;

namespace TorneSe.ServicoLancamentoNotas.Testes.Api.Nota;

[CollectionDefinition(nameof(NotaControllerTestsFixture))]
public class NotaControllerTestsFixtureCollection
    : ICollectionFixture<NotaControllerTestsFixture>
{ }

public class NotaControllerTestsFixture
    : BaseFixture
{
    public Resultado<NotaOutputModel> RetornaResultadoSucesso()
        => Resultado<NotaOutputModel>.RetornaResultadoSucesso(RetornaOutputModel());

    public Resultado<ListaNotaOutput> RetornarResultadoComLista(int quantidade = 0)
        => Resultado<ListaNotaOutput>.RetornaResultadoSucesso(RetornaListaOutput(quantidade));

    public Resultado<NotaOutputModel> RetornaResultadoFalha()
        => Resultado<NotaOutputModel>.RetornaResultadoErro(TipoErro.NotaInvalida);

    public Resultado<NotaOutputModel> RetornaResultadoNotaNaoEncontrada()
        => Resultado<NotaOutputModel>.RetornaResultadoErro(TipoErro.NotaNaoEncontrada);

    private ListaNotaOutput RetornaListaOutput(int quantidade)
        => new(1, 10, 0, Enumerable.Range(0, quantidade).Select(_ => RetornaOutputModel()).ToList());

    private NotaOutputModel RetornaOutputModel()
        => new(
            RetornaNumeroIdRandomico(),
            RetornaNumeroIdRandomico(),
            RetornaValorNotaAleatorioValido(),
            DateTime.Now,
            false,
            null!,
            StatusIntegracao.AguardandoIntegracao);
}
