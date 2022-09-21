using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.DTOs;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.Params;
using TorneSe.ServicoLancamentoNotas.Testes.Comum;

namespace TorneSe.ServicoLancamentoNotas.Testes.Aplicacao.CasosDeUsoNota.Atualizar;

[CollectionDefinition(nameof(AtualizarNotaTestsFixture))]
public class AtualizarNotaTestsFixtureCollection
	: ICollectionFixture<AtualizarNotaTestsFixture>
{ }

public class AtualizarNotaTestsFixture
	: BaseFixture
{
	public AtualizarNotaInput RetornaInputValido()
		=> new(RetornaNumeroIdRandomico(),
				RetornaNumeroIdRandomico(),
				RetornaNumeroIdRandomico(),
				RetornaValorNotaAleatorioValido());

	public NotaParams RetornaValoresParametrosNotaValidos()
		=> new(RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(),
				RetornaValorNotaAleatorioValido(), DateTime.Now);

	public Nota RetornaNota()
		=> new(RetornaValoresParametrosNotaValidos());
}
