using FluentAssertions;
using TorneSe.ServicoLancamentoNotas.Dominio.Constantes;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.Enums;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;

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
        nota.Should().BeAssignableTo<NotifiableObject>();
        nota.EhValida.Should().BeTrue();
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
        var nota = new Nota(parametrosNota);

        //Assert
        nota.Notificacoes.Should().NotBeEmpty();
        nota.Notificacoes.Should().HaveCount(1);
        nota.Notificacoes.First().Campo.Should().Be(nameof(nota.ValorNota));
        nota.Notificacoes.First().Mensagem.Should().Be(ConstantesDominio.MensagensValidacoes.ERRO_VALOR_NOTA_INVALIDO);
        nota.EhValida.Should().BeFalse();
    }

    [Theory(DisplayName = nameof(InstanciarNota_QuandoUsuarioIdInvalido_DeveLancarExcecao))]
    [Trait("Dominio", "Nota - Agregado")]
    [InlineData(-1)]
    [InlineData(0)]
    public void InstanciarNota_QuandoUsuarioIdInvalido_DeveLancarExcecao(int usuarioId)
    {
        var parametrosNota = _fixture.RetornaValoresParametrosInvalidosCustomizados(usuarioId: usuarioId);

        //Act
        var nota = new Nota(parametrosNota);

        //Assert
        nota.Notificacoes.Should().NotBeEmpty();
        nota.Notificacoes.Should().HaveCount(1);
        nota.Notificacoes.First().Campo.Should().Be(nameof(nota.UsuarioId));
        nota.Notificacoes.First().Mensagem.Should().Be(ConstantesDominio.MensagensValidacoes.ERRO_USUARIO_INVALIDO);
        nota.EhValida.Should().BeFalse();
    }

    [Theory(DisplayName = nameof(InstanciarNota_QuandoAlunoIdInvalido_DeveLancarExcecao))]
    [Trait("Dominio", "Nota - Agregado")]
    [InlineData(-1)]
    [InlineData(0)]
    public void InstanciarNota_QuandoAlunoIdInvalido_DeveLancarExcecao(int alunoId)
    {
        var parametrosNota = _fixture.RetornaValoresParametrosInvalidosCustomizados(alunoId: alunoId);

        //Act
        var nota = new Nota(parametrosNota);

        nota.Notificacoes.Should().NotBeEmpty();
        nota.Notificacoes.Should().HaveCount(1);
        nota.Notificacoes.First().Campo.Should().Be(nameof(nota.AlunoId));
        nota.Notificacoes.First().Mensagem.Should().Be(ConstantesDominio.MensagensValidacoes.ERRO_ALUNO_INVALIDO);
        nota.EhValida.Should().BeFalse();
    }

    [Theory(DisplayName = nameof(InstanciarNota_QuandoAtividadeIdInvalida_DeveLancarExcecao))]
    [Trait("Dominio", "Nota - Agregado")]
    [InlineData(-1)]
    [InlineData(0)]
    public void InstanciarNota_QuandoAtividadeIdInvalida_DeveLancarExcecao(int atividadeId)
    {
        var parametrosNota = _fixture.RetornaValoresParametrosInvalidosCustomizados(atividadeId: atividadeId);

        //Act
        var nota = new Nota(parametrosNota);

        nota.Notificacoes.Should().NotBeEmpty();
        nota.Notificacoes.Should().HaveCount(1);
        nota.Notificacoes.First().Campo.Should().Be(nameof(nota.AtividadeId));
        nota.Notificacoes.First().Mensagem.Should().Be(ConstantesDominio.MensagensValidacoes.ERRO_ATIVIDADE_INVALIDA);
        nota.EhValida.Should().BeFalse();
    }

    //Preciso controlar se a nota lançada já foi integrada
    //Caso uma nota venha a ser cancelada preciso de um motivo para o cancelamento
    //Uma valor de nota deve estar no intervalo entre 0 a 10
}
