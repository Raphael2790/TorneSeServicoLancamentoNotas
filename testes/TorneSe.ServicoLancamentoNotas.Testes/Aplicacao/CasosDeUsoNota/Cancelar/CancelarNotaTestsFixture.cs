using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar.DTOs;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.Params;
using TorneSe.ServicoLancamentoNotas.Testes.Comum;

namespace TorneSe.ServicoLancamentoNotas.Testes.Aplicacao.CasosDeUsoNota.Cancelar;

[CollectionDefinition(nameof(CancelarNotaTestsFixture))]
public class CancelarNotaTestsFixtureCollection 
    : ICollectionFixture<CancelarNotaTestsFixture>
{ }

public class CancelarNotaTestsFixture
    : BaseFixture
{
	public NotaParams RetornaValoresParametrosNotaValidos()
		=> new(RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(),
				RetornaValorNotaAleatorioValido(), DateTime.Now);

	public Nota RetornaNota()
		=> new(RetornaValoresParametrosNotaValidos());

	public CancelarNotaInput RetornaInput()
		=> new(RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), Faker.Commerce.ProductName());

    public CancelarNotaInput RetornaInputInvalido()
    {
        string motivoCancelamento = Faker.Lorem.Text();
        while (motivoCancelamento.Length <= 500)
            motivoCancelamento += Faker.Lorem.Text();

        return new(RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), motivoCancelamento);
    }
}
