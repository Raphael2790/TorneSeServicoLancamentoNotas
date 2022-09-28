using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.Params;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork.BuscaRepository;
using TorneSe.ServicoLancamentoNotas.Testes.Comum;

namespace TorneSe.ServicoLancamentoNotas.Testes.Aplicacao.CasosDeUsoNota.Consultar;

[CollectionDefinition(nameof(ConsultarNotaTestsFixture))]
public class ConsultarNotaTestsFixtureCollection
    : ICollectionFixture<ConsultarNotaTestsFixture>
{ }


public class ConsultarNotaTestsFixture
    : BaseFixture
{
    public ListaNotaInput RetornaListBuscaInput()
        => new(1, 10, null, null, "");

    public BuscaOutput<Nota> RetornaOutputRepositorio()
        => new(1, 10, 90, Enumerable.Range(0,10).Select(_ => RetornaNota()).ToList().AsReadOnly());

    public BuscaOutput<Nota> RetornaOutputRepositorioVazio()
        => new(1, 10, 0, new List<Nota>(0).AsReadOnly());

    public NotaParams RetornaValoresParametrosNotaValidos()
        => new(RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(),
                RetornaValorNotaAleatorioValido(), DateTime.Now);

    public Nota RetornaNota()
        => new(RetornaValoresParametrosNotaValidos());
}
