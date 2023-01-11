using FluentAssertions;
using FluentValidation;
using Moq;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Behaviors;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Enums;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Validacoes;

namespace TorneSe.ServicoLancamentoNotas.Testes.Aplicacao.Behaviors;

[Collection(nameof(InputValidacaoBehaviorTestsFixture))]
public class ValidacaoInputBehaviorTests
{
    private readonly InputValidacaoBehaviorTestsFixture _fixture;
    private readonly Mock<IValidator<AtualizarNotaInput>> _validadorMock;
    private ValidacaoInputBehavior<AtualizarNotaInput, Resultado<NotaOutputModel>> _sut;

    public ValidacaoInputBehaviorTests(InputValidacaoBehaviorTestsFixture fixture)
    {
        _fixture = fixture;
        _validadorMock = new Mock<IValidator<AtualizarNotaInput>>();
        _sut = new ValidacaoInputBehavior<AtualizarNotaInput, Resultado<NotaOutputModel>>(_validadorMock.Object);
    }

    [Fact(DisplayName = nameof(Handle_QuandoValidacaoPossuiErros_DeveRetornarErro))]
    [Trait("Aplicacao", "Nota - Comportamentos")]
    public async Task Handle_QuandoValidacaoPossuiErros_DeveRetornarErro()
    {
        var request = _fixture.DevolveAtualizarNotaInputInvalido();
        var resultadoValidacao = await AtualizarNotaInputValidator.Instance.ValidateAsync(request);
        _validadorMock.Setup(x => x.ValidateAsync(It.IsAny<AtualizarNotaInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultadoValidacao);

        var resultado = await _sut.Handle(request, null!, CancellationToken.None);

        resultado.Should().NotBeNull();
        resultado.Erro.Should().Be(TipoErro.InputNotaInvalido);
        resultado.Sucesso.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Handle_QuandoValidacaoNaoPossuiErros_DeveRetornarSucesso))]
    [Trait("Aplicacao", "Nota - Comportamentos")]
    public async Task Handle_QuandoValidacaoNaoPossuiErros_DeveRetornarSucesso()
    {
        var request = _fixture.DevolveNotaAtualizarInputValido();
        var resultadoValidacao = await AtualizarNotaInputValidator.Instance.ValidateAsync(request);
        _validadorMock.Setup(x => x.ValidateAsync(It.IsAny<AtualizarNotaInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultadoValidacao);

        var resultado = await _sut.Handle(request, _fixture.RetornaSucesso, CancellationToken.None);

        resultado.Should().NotBeNull();
        resultado.Sucesso.Should().BeTrue();
        resultado.Dado.Should().NotBeNull();
    }

    [Fact(DisplayName = nameof(Handle_QuandoValidadorEstaNulo_DeveRetornarSucesso))]
    [Trait("Aplicacao", "Nota - Comportamentos")]
    public async Task Handle_QuandoValidadorEstaNulo_DeveRetornarSucesso()
    {
        var request = _fixture.DevolveNotaAtualizarInputValido();
        _sut = new ValidacaoInputBehavior<AtualizarNotaInput, Resultado<NotaOutputModel>>(null!);

        var resultado = await _sut.Handle(request, _fixture.RetornaSucesso, CancellationToken.None);

        resultado.Should().NotBeNull();
        resultado.Sucesso.Should().BeTrue();
        resultado.Dado.Should().NotBeNull();
    }
}
