using FluentAssertions;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.Enums;
using TorneSe.ServicoLancamentoNotas.Dominio.Specifications;
using TorneSe.ServicoLancamentoNotas.Testes.Dominio.Entidades;

namespace TorneSe.ServicoLancamentoNotas.Testes.Dominio.Specifications;

[Collection(nameof(NotaTestesFixture))]
public class StatusNotaSpecificationTests
{
    private readonly NotaTestesFixture _fixture;

    public StatusNotaSpecificationTests(NotaTestesFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(Nota_QuandoStatusDeAguardandoIntegracao_DeveSatisfazerEspecificao))]
    [Trait("Dominio", "StatusNotaSpecification - Specifications")]
    public void Nota_QuandoStatusDeAguardandoIntegracao_DeveSatisfazerEspecificao()
    {
        var notaParametros = _fixture.RetornaValoresParametrosNotaValidos();
        Nota nota = new(notaParametros);

        StatusAguardandoIntegracaoSpec.Instance.Should().NotBeNull();
        StatusAguardandoIntegracaoSpec.Instance.ToExpression().Should().NotBeNull();
        StatusAguardandoIntegracaoSpec.Instance.IsSatisfied(nota).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(Nota_QuandoStatusDiferenteDeAguardandoIntegracao_NaoDeveSatisfazerEspecificao))]
    [InlineData(StatusIntegracao.IntegradaComSucesso)]
    [InlineData(StatusIntegracao.FalhaNaIntegracao)]
    [InlineData(StatusIntegracao.EnviadaParaIntegracao)]
    [Trait("Dominio", "StatusNotaSpecification - Specifications")]
    public void Nota_QuandoStatusDiferenteDeAguardandoIntegracao_NaoDeveSatisfazerEspecificao(StatusIntegracao statusIntegracao)
    {
        var notaParametros = _fixture.RetornaValoresParametrosNotaValidosComStatus(statusIntegracao);
        Nota nota = new(notaParametros);

        StatusAguardandoIntegracaoSpec.Instance.Should().NotBeNull();
        StatusAguardandoIntegracaoSpec.Instance.ToExpression().Should().NotBeNull();
        StatusAguardandoIntegracaoSpec.Instance.IsSatisfied(nota).Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Nota_QuandoStatusDeEnviadaParaIntegracao_DeveSatisfazerEspecificao))]
    [Trait("Dominio", "StatusNotaSpecification - Specifications")]
    public void Nota_QuandoStatusDeEnviadaParaIntegracao_DeveSatisfazerEspecificao()
    {
        var notaParametros = _fixture.RetornaValoresParametrosNotaValidosComStatus(StatusIntegracao.EnviadaParaIntegracao);
        Nota nota = new(notaParametros);

        StatusEnviadaParaIntegracaoSpec.Instance.Should().NotBeNull();
        StatusEnviadaParaIntegracaoSpec.Instance.ToExpression().Should().NotBeNull();
        StatusEnviadaParaIntegracaoSpec.Instance.IsSatisfied(nota).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(Nota_QuandoStatusDiferenteDeEnviadaParaIntegracao_NaoDeveSatisfazerEspecificao))]
    [InlineData(StatusIntegracao.IntegradaComSucesso)]
    [InlineData(StatusIntegracao.FalhaNaIntegracao)]
    [InlineData(StatusIntegracao.AguardandoIntegracao)]
    [Trait("Dominio", "StatusNotaSpecification - Specifications")]
    public void Nota_QuandoStatusDiferenteDeEnviadaParaIntegracao_NaoDeveSatisfazerEspecificao(StatusIntegracao statusIntegracao)
    {
        var notaParametros = _fixture.RetornaValoresParametrosNotaValidosComStatus(statusIntegracao);
        Nota nota = new(notaParametros);

        StatusEnviadaParaIntegracaoSpec.Instance.Should().NotBeNull();
        StatusEnviadaParaIntegracaoSpec.Instance.ToExpression().Should().NotBeNull();
        StatusEnviadaParaIntegracaoSpec.Instance.IsSatisfied(nota).Should().BeFalse();
    }
}
