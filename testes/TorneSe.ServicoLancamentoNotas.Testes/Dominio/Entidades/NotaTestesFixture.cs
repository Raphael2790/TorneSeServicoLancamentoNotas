using TorneSe.ServicoLancamentoNotas.Dominio.Enums;
using TorneSe.ServicoLancamentoNotas.Dominio.Params;
using TorneSe.ServicoLancamentoNotas.Testes.Comum;

namespace TorneSe.ServicoLancamentoNotas.Testes.Dominio.Entidades;

public class NotaTestesFixture : BaseFixture
{
    public NotaParams RetornaValoresParametrosInvalidosCustomizados(int? alunoId = null, int? atividadeId = null,
                                                                    double? valorNota = null)
        => new(alunoId ?? RetornaNumeroIdRandomico(), atividadeId ?? RetornaNumeroIdRandomico(), valorNota ?? RetornaValorNotaAleatorioValido(),
            DateTime.Now);

    public NotaParams RetornaValoresParametrosNotaValidos()
        => new(RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), 
                RetornaValorNotaAleatorioValido(), DateTime.Now);

    public NotaParams RetornaValoresParametrosNotaValidosComStatus(StatusIntegracao statusIntegracao)
        => new(RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(),
                RetornaValorNotaAleatorioValido(), DateTime.Now,statusIntegracao);
}

[CollectionDefinition(nameof(NotaTestesFixture))]
public class NotaTestesFixtureCollection : ICollectionFixture<NotaTestesFixture> { }
