using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.Params;
using TorneSe.ServicoLancamentoNotas.Testes.Comum;

namespace TorneSe.ServicoLancamentoNotas.Testes.Aplicacao.Mapeadores;

[CollectionDefinition(nameof(MapeadorAplicacaoFixture))]
public class MapeadorAplicacaoFixtureCollection 
    : ICollectionFixture<MapeadorAplicacaoFixture>
{ }

public class MapeadorAplicacaoFixture
    : BaseFixture
{
    public static int RetornaNumeroIdRandomico()
       => new Random().Next(1, 1_000_000);

    public double RetornaValorNotaAleatorioValido()
        => Faker.Random.Double(0.00, 10.00);

    public bool RetornaBoleanoRandomico()
        => new Random().Next(0, 10) > 5;

    public LancarNotaInput DevolveNotaInputValido()
        => new(RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), RetornaValorNotaAleatorioValido(),
            RetornaBoleanoRandomico());

    public NotaParams RetornaValoresParametrosNotaValidos()
        => new(RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(),
                RetornaValorNotaAleatorioValido(), DateTime.Now);

    public Nota RetornaNotaValida()
        => new(RetornaValoresParametrosNotaValidos());
}
