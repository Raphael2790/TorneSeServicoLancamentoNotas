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
        nota.Id.Should().NotBeEmpty();
        nota.Id.Should().NotBe(Guid.Empty);
        nota.AlunoId.Should().Be(parametrosNota.AlunoId);
        nota.AtividadeId.Should().Be(parametrosNota.AtividadeId);
        nota.ValorNota.Should().Be(parametrosNota.ValorNota);
        nota.DataLancamento.Should().NotBe(default);
        nota.DataLancamento.Should().Be(parametrosNota.DataLancamento);
        nota.DataCriacao.Should().NotBe(default);
        nota.DataCriacao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        nota.UsuarioId.Should().Be(parametrosNota.UsuarioId);
        nota.CanceladaPorRetentativa.Should().BeFalse();
        nota.Cancelada.Should().BeFalse();
        nota.StatusIntegracao.Should().Be(parametrosNota.StatusIntegracao);
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

    [Fact(DisplayName = nameof(Cancelar_QuandoInformadoMotivoExtenso_DevePossuirNotificacao))]
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

    [Fact(DisplayName = nameof(Cancelar_QuandoInformadoMotivo_NaoDevePossuirNotificacao))]
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
        nota.DataAtualizacao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact(DisplayName = nameof(CancelarPorRetentativa_QuandoSolitadoCancelamento_NaoDevePossuirNotificacao))]
    [Trait("Dominio", "Nota - Agregado")]
    public void CancelarPorRetentativa_QuandoSolitadoCancelamento_NaoDevePossuirNotificacao()
    {
        //Arrange
        var notaParams = _fixture.RetornaValoresParametrosNotaValidos();
        Nota nota = new(notaParams);

        //Act
        nota.CancelarPorRetentativa();

        //Assertt
        nota.Notificacoes.Should().BeEmpty();
        nota.EhValida.Should().BeTrue();
        nota.Cancelada.Should().BeTrue();
        nota.CanceladaPorRetentativa.Should().BeTrue();
        nota.DataAtualizacao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Theory(DisplayName = nameof(AtualizarValorNota_QuandoInformadoValoresInvalidos_DevePossuirNotificacao))]
    [Trait("Dominio", "Nota - Agregado")]
    [InlineData(-1)]
    [InlineData(11)]
    public void AtualizarValorNota_QuandoInformadoValoresInvalidos_DevePossuirNotificacao(double novoValorNota)
    {
        //Arrange
        var notaParams = _fixture.RetornaValoresParametrosNotaValidos();
        Nota nota = new(notaParams);

        //Act
        nota.AtualizarValorNota(novoValorNota);

        //Assertt
        nota.Notificacoes.Should().NotBeEmpty();
        nota.EhValida.Should().BeFalse();
        nota.Notificacoes.Should().HaveCount(1);
        nota.Notificacoes.First().Campo.Should().Be(nameof(nota.ValorNota));
        nota.Notificacoes.First().Mensagem.Should().Be(ConstantesDominio.MensagensValidacoes.ERRO_VALOR_NOTA_INVALIDO);
    }

    [Theory(DisplayName = nameof(AtualizarValorNota_QuandoInformadoValoresInvalidos_DevePossuirNotificacao))]
    [Trait("Dominio", "Nota - Agregado")]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(10)]
    public void AtualizarValorNota_QuandoInformadoValoresValidos_NaoDevePossuirNotificacao(double novoValorNota)
    {
        //Arrange
        var notaParams = _fixture.RetornaValoresParametrosNotaValidos();
        Nota nota = new(notaParams);

        //Act
        nota.AtualizarValorNota(novoValorNota);

        //Assertt
        nota.Notificacoes.Should().BeEmpty();
        nota.EhValida.Should().BeTrue();
        nota.ValorNota.Should().Be(novoValorNota);
        nota.DataAtualizacao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact(DisplayName = nameof(AlterarStatusIntegracaoParaEnviada_QuandoPermitidoTrocaStatus_DeveAtualizarOStatus))]
    [Trait("Dominio", "Nota - Agregado")]
    public void AlterarStatusIntegracaoParaEnviada_QuandoPermitidoTrocaStatus_DeveAtualizarOStatus()
    {
        //Arrange
        var notaParams = _fixture.RetornaValoresParametrosNotaValidos();
        Nota nota = new(notaParams);

        //Act
        nota.AlterarStatusIntegracaoParaEnviada();

        //Assertt
        nota.Notificacoes.Should().BeEmpty();
        nota.EhValida.Should().BeTrue();
        nota.DataAtualizacao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        nota.StatusIntegracao.Should().Be(StatusIntegracao.EnviadaParaIntegracao);
    }

    [Theory(DisplayName = nameof(AlterarStatusIntegracaoParaEnviada_QuandoNaoPermitidoTrocaStatus_DeveAdicionarNotificao))]
    [InlineData(StatusIntegracao.EnviadaParaIntegracao)]
    [InlineData(StatusIntegracao.IntegradaComSucesso)]
    [InlineData(StatusIntegracao.FalhaNaIntegracao)]
    [Trait("Dominio", "Nota - Agregado")]
    public void AlterarStatusIntegracaoParaEnviada_QuandoNaoPermitidoTrocaStatus_DeveAdicionarNotificao(StatusIntegracao statusIntegracao)
    {
        //Arrange
        var notaParams = _fixture.RetornaValoresParametrosNotaValidosComStatus(statusIntegracao);
        Nota nota = new(notaParams);

        //Act
        nota.AlterarStatusIntegracaoParaEnviada();

        //Assertt
        nota.Notificacoes.Should().NotBeEmpty();
        nota.EhValida.Should().BeFalse();
        nota.Notificacoes.Should().HaveCount(1);
    }
}
