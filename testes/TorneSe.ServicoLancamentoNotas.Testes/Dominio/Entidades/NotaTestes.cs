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
        nota.Cancelada.Should().BeFalse();
        nota.StatusIntegracao.Should().Be(StatusIntegracao.AguardandoIntegracao);
        nota.MotivoCancelamento.Should().BeNull();
        nota.Should().BeAssignableTo<NotifiableObject>();
        nota.EhValida.Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstanciarNota_QuandoValorNotaInvalido_DevePossuirNotificao))]
    [Trait("Dominio", "Nota - Agregado")]
    [InlineData(-1)]
    [InlineData(11)]
    public void InstanciarNota_QuandoValorNotaInvalido_DevePossuirNotificao(double valorNota)
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

    [Theory(DisplayName = nameof(InstanciarNota_QuandoUsuarioIdInvalido_DevePossuirNotificao))]
    [Trait("Dominio", "Nota - Agregado")]
    [InlineData(-1)]
    [InlineData(0)]
    public void InstanciarNota_QuandoUsuarioIdInvalido_DevePossuirNotificao(int usuarioId)
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

    [Theory(DisplayName = nameof(InstanciarNota_QuandoAlunoIdInvalido_DevePossuirNotificao))]
    [Trait("Dominio", "Nota - Agregado")]
    [InlineData(-1)]
    [InlineData(0)]
    public void InstanciarNota_QuandoAlunoIdInvalido_DevePossuirNotificao(int alunoId)
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

    [Theory(DisplayName = nameof(InstanciarNota_QuandoAtividadeIdInvalida_DevePossuirNotificao))]
    [Trait("Dominio", "Nota - Agregado")]
    [InlineData(-1)]
    [InlineData(0)]
    public void InstanciarNota_QuandoAtividadeIdInvalida_DevePossuirNotificao(int atividadeId)
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

    [Theory(DisplayName = nameof(Cancelar_QuandoNaoInformadoMotivo_DevePossuirNotificacao))]
    [Trait("Dominio", "Nota - Agregado")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void Cancelar_QuandoNaoInformadoMotivo_DevePossuirNotificacao(string motivoCancelamento)
    {
        //Arrange
        var notaParams = _fixture.RetornaValoresParametrosNotaValidos();
        Nota nota = new(notaParams);

        //Act
        nota.Cancelar(motivoCancelamento);

        //Assertt
        nota.Notificacoes.Should().NotBeEmpty();
        nota.Notificacoes.Should().HaveCount(1);
        nota.Notificacoes.First().Campo.Should().Be(nameof(nota.MotivoCancelamento));
        nota.Notificacoes.First().Mensagem.Should().Be(ConstantesDominio.MensagensValidacoes.ERRO_MOTIVO_CANCELAMENTO_NAO_INFORMADO);
        nota.EhValida.Should().BeFalse();
        nota.Cancelada.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Cancelar_QuandoNaoInformadoMotivo_DevePossuirNotificacao))]
    [Trait("Dominio", "Nota - Agregado")]
    public void Cancelar_QuandoInformadoMotivoExtenso_DevePossuirNotificacao()
    {
        //Arrange
        var notaParams = _fixture.RetornaValoresParametrosNotaValidos();
        Nota nota = new(notaParams);
        string motivoCancelamento = _fixture.Faker.Lorem.Text();
        while(motivoCancelamento.Length < 500)
            motivoCancelamento += _fixture.Faker.Lorem.Text();

        //Act
        nota.Cancelar(motivoCancelamento);

        //Assertt
        nota.Notificacoes.Should().NotBeEmpty();
        nota.Notificacoes.Should().HaveCount(1);
        nota.Notificacoes.First().Campo.Should().Be(nameof(nota.MotivoCancelamento));
        nota.Notificacoes.First().Mensagem.Should().Be(ConstantesDominio.MensagensValidacoes.ERRO_MOTIVO_CANCELAMENTO_EXTENSO);
        nota.EhValida.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Cancelar_QuandoNaoInformadoMotivo_DevePossuirNotificacao))]
    [Trait("Dominio", "Nota - Agregado")]
    public void Cancelar_QuandoInformadoMotivo_NaoDevePossuirNotificacao()
    {
        //Arrange
        var notaParams = _fixture.RetornaValoresParametrosNotaValidos();
        Nota nota = new(notaParams);
        string motivoCancelamento = _fixture.Faker.Lorem.Text();

        //Act
        nota.Cancelar(motivoCancelamento);

        //Assertt
        nota.Notificacoes.Should().BeEmpty();
        nota.EhValida.Should().BeTrue();
        nota.Cancelada.Should().BeTrue();
    }

    //Preciso controlar se a nota lançada já foi integrada
    //Caso uma nota venha a ser cancelada preciso de um motivo para o cancelamento
    //Uma valor de nota deve estar no intervalo entre 0 a 10
}
