using FluentAssertions;
using TorneSe.ServicoLancamentoNotas.Dominio.Constantes;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.Enums;
using TorneSe.ServicoLancamentoNotas.Dominio.Exceptions;

namespace TorneSe.ServicoLancamentoNotas.Testes.Dominio.Entidades;

[Collection(nameof(NotaTestesFixture))]
public class NotaTestes
{
    private readonly NotaTestesFixture _fixture;
    public NotaTestes(NotaTestesFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(InstanciarNota))]
    [Trait("Dominio", "Nota - Agregado")]
    public void InstanciarNota()
    {
        //Arrange
        var parametrosNota = _fixture.RetornaValoresParametrosNotaValidos();

        //Act
        var nota = new Nota(parametrosNota);

        //Assert
        nota.Should().NotBeNull();
        nota.AlunoId.Should().Be(parametrosNota.AlunoId);
        nota.AtividadeId.Should().Be(parametrosNota.AtividadeId);
        nota.ValorNota.Should().Be(parametrosNota.ValorNota);
        nota.DataLancamento.Should().NotBe(default);
        nota.DataLancamento.Should().Be(parametrosNota.DataLancamento);
        nota.UsuarioId.Should().Be(parametrosNota.UsuarioId);
        nota.CanceladaPorRetentativa.Should().BeFalse();
        nota.StatusIntegracao.Should().Be(StatusIntegracao.AguardandoIntegracao);
        nota.MotivoCancelamento.Should().BeNull();
    }

    [Theory(DisplayName = nameof(InstanciarNota_QuandoValorNotaInvalido_DeveLancarExcecao))]
    [Trait("Dominio", "Nota - Agregado")]
    [InlineData(-1)]
    [InlineData(11)]
    public void InstanciarNota_QuandoValorNotaInvalido_DeveLancarExcecao(double valorNota)
    {
        //Arrange
        var parametrosNota = _fixture.RetornaValoresParametrosInvalidosCustomizados(valorNota : valorNota);

        //Act
        var action = () => new Nota(parametrosNota);
        var nota = new Nota(parametrosNota);

        //Assert
        nota.EhValida.Should().BeFalse();
        nota.Notificacoes.Count().Should().BeGreaterThanOrEqualTo(1);
        nota.Notificacoes.Should().Contains(ConstantesDominio.MensagensValidacoes.ERRO_VALOR_NOTA_INVALIDO);
        action.Should().ThrowExactly<ValidacaoEntidadeException>()
            .And.Message.Should().Be(ConstantesDominio.MensagensValidacoes.ERRO_VALOR_NOTA_INVALIDO);
    }

    [Theory(DisplayName = nameof(InstanciarNota_QuandoUsuarioIdInvalido_DeveLancarExcecao))]
    [Trait("Dominio", "Nota - Agregado")]
    [InlineData(-1)]
    [InlineData(0)]
    public void InstanciarNota_QuandoUsuarioIdInvalido_DeveLancarExcecao(int usuarioId)
    {
        var parametrosNota = _fixture.RetornaValoresParametrosInvalidosCustomizados(usuarioId: usuarioId);

        //Act
        var action = () => new Nota(parametrosNota);

        action.Should().ThrowExactly<ValidacaoEntidadeException>()
            .And.Message.Should().Be(ConstantesDominio.MensagensValidacoes.ERRO_USUARIO_INVALIDO);
    }

    [Theory(DisplayName = nameof(InstanciarNota_QuandoAlunoIdInvalido_DeveLancarExcecao))]
    [Trait("Dominio", "Nota - Agregado")]
    [InlineData(-1)]
    [InlineData(0)]
    public void InstanciarNota_QuandoAlunoIdInvalido_DeveLancarExcecao(int alunoId)
    {
        var parametrosNota = _fixture.RetornaValoresParametrosInvalidosCustomizados(alunoId: alunoId);

        //Act
        var action = () => new Nota(parametrosNota);

        action.Should().ThrowExactly<ValidacaoEntidadeException>()
            .And.Message.Should().Be(ConstantesDominio.MensagensValidacoes.ERRO_ALUNO_INVALIDO);
    }

    [Theory(DisplayName = nameof(InstanciarNota_QuandoAtividadeIdInvalida_DeveLancarExcecao))]
    [Trait("Dominio", "Nota - Agregado")]
    [InlineData(-1)]
    [InlineData(0)]
    public void InstanciarNota_QuandoAtividadeIdInvalida_DeveLancarExcecao(int atividadeId)
    {
        var parametrosNota = _fixture.RetornaValoresParametrosInvalidosCustomizados(atividadeId: atividadeId);

        //Act
        var action = () => new Nota(parametrosNota);

        action.Should().ThrowExactly<ValidacaoEntidadeException>()
            .And.Message.Should().Be(ConstantesDominio.MensagensValidacoes.ERRO_ATIVIDADE_INVALIDA);
    }

    //Preciso controlar se a nota lançada já foi integrada
    //Caso uma nota venha a ser cancelada preciso de um motivo para o cancelamento
    //Uma valor de nota deve estar no intervalo entre 0 a 10
}
