using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TorneSe.ServicoLancamentoNotas.API.Controllers.v1;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Enums;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;

namespace TorneSe.ServicoLancamentoNotas.Testes.Api.Nota;

[Collection(nameof(NotaControllerTestsFixture))]
public class NotaControllerTests
{
    private readonly NotaControllerTestsFixture _fixture;
    private readonly Mock<ILogger<NotasController>> _loggerMock;
    private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
    private readonly NotasController _sut;

    public NotaControllerTests(NotaControllerTestsFixture fixture)
    {
        _fixture = fixture;
        _loggerMock = new Mock<ILogger<NotasController>>();
        _mediatorHandlerMock= new Mock<IMediatorHandler>();
        _sut = new NotasController(_loggerMock.Object, _mediatorHandlerMock.Object);
    }

    [Fact(DisplayName = nameof(Lancar_QuandoResultadoSucesso_DeveRetornarOk))]
    [Trait("Api", "Nota - Controller")]
    public async Task Lancar_QuandoResultadoSucesso_DeveRetornarOk()
    {
        var retorno = _fixture.RetornaResultadoSucesso();
        _mediatorHandlerMock.Setup(x =>
                x.EnviarRequest<Resultado<NotaOutputModel>, LancarNotaInput>(
                    It.IsAny<LancarNotaInput>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(retorno);

        var resultado = await _sut.Lancar(It.IsAny<LancarNotaInput>(), It.IsAny<CancellationToken>()) as ObjectResult;
        resultado.Should().NotBeNull();
        resultado!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var resposta = resultado!.Value as Resultado<NotaOutputModel>;
        resposta.Should().NotBeNull();
        resposta!.Dado.Should().NotBeNull();
        resposta.Sucesso.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Lancar_QuandoResultadoDeFalha_DeveRetornarBadRequest))]
    [Trait("Api", "Nota - Controller")]
    public async Task Lancar_QuandoResultadoDeFalha_DeveRetornarBadRequest()
    {
        var retorno = _fixture.RetornaResultadoFalha();
        _mediatorHandlerMock.Setup(x =>
                x.EnviarRequest<Resultado<NotaOutputModel>, LancarNotaInput>(
                    It.IsAny<LancarNotaInput>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(retorno);

        var resultado = await _sut.Lancar(It.IsAny<LancarNotaInput>(), It.IsAny<CancellationToken>()) as ObjectResult;
        resultado.Should().NotBeNull();
        resultado!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        var resposta = resultado!.Value as Resultado<NotaOutputModel>;
        resposta.Should().NotBeNull();
        resposta!.Dado.Should().BeNull();
        resposta.Erro.Should().Be(TipoErro.NotaInvalida);
        resposta.Sucesso.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Atualizar_QuandoResultadoSucesso_DeveRetornarOk))]
    [Trait("Api", "Nota - Controller")]
    public async Task Atualizar_QuandoResultadoSucesso_DeveRetornarOk()
    {
        var retorno = _fixture.RetornaResultadoSucesso();
        _mediatorHandlerMock.Setup(x =>
                x.EnviarRequest<Resultado<NotaOutputModel>, AtualizarNotaInput>(
                    It.IsAny<AtualizarNotaInput>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(retorno);

        var resultado = await _sut.Atualizar(It.IsAny<AtualizarNotaInput>(), It.IsAny<CancellationToken>()) as ObjectResult;
        resultado.Should().NotBeNull();
        resultado!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var resposta = resultado!.Value as Resultado<NotaOutputModel>;
        resposta.Should().NotBeNull();
        resposta!.Dado.Should().NotBeNull();
        resposta.Sucesso.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Atualizar_QuandoResultadoDeFalha_DeveRetornarBadRequest))]
    [Trait("Api", "Nota - Controller")]
    public async Task Atualizar_QuandoResultadoDeFalha_DeveRetornarBadRequest()
    {
        var retorno = _fixture.RetornaResultadoFalha();
        _mediatorHandlerMock.Setup(x =>
                x.EnviarRequest<Resultado<NotaOutputModel>, AtualizarNotaInput>(
                    It.IsAny<AtualizarNotaInput>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(retorno);

        var resultado = await _sut.Atualizar(It.IsAny<AtualizarNotaInput>(), It.IsAny<CancellationToken>()) as ObjectResult;
        resultado.Should().NotBeNull();
        resultado!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        var resposta = resultado!.Value as Resultado<NotaOutputModel>;
        resposta.Should().NotBeNull();
        resposta!.Dado.Should().BeNull();
        resposta.Erro.Should().Be(TipoErro.NotaInvalida);
        resposta.Sucesso.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Atualizar_QuandoNotaNaoEncontrada_DeveRetornarNotFound))]
    [Trait("Api", "Nota - Controller")]
    public async Task Atualizar_QuandoNotaNaoEncontrada_DeveRetornarNotFound()
    {
        var retorno = _fixture.RetornaResultadoNotaNaoEncontrada();
        _mediatorHandlerMock.Setup(x =>
                x.EnviarRequest<Resultado<NotaOutputModel>, AtualizarNotaInput>(
                    It.IsAny<AtualizarNotaInput>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(retorno);

        var resultado = await _sut.Atualizar(It.IsAny<AtualizarNotaInput>(), It.IsAny<CancellationToken>()) as ObjectResult;
        resultado.Should().NotBeNull();
        resultado!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        var resposta = resultado!.Value as Resultado<NotaOutputModel>;
        resposta.Should().NotBeNull();
        resposta!.Dado.Should().BeNull();
        resposta.Erro.Should().Be(TipoErro.RecursoNaoEncontrado);
        resposta.Sucesso.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Cancelar_QuandoResultadoSucesso_DeveRetornarOk))]
    [Trait("Api", "Nota - Controller")]
    public async Task Cancelar_QuandoResultadoSucesso_DeveRetornarOk()
    {
        var retorno = _fixture.RetornaResultadoSucesso();
        _mediatorHandlerMock.Setup(x =>
                x.EnviarRequest<Resultado<NotaOutputModel>, CancelarNotaInput>(
                    It.IsAny<CancelarNotaInput>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(retorno);

        var resultado = await _sut.Cancelar(It.IsAny<CancelarNotaInput>(), It.IsAny<CancellationToken>()) as ObjectResult;
        resultado.Should().NotBeNull();
        resultado!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var resposta = resultado!.Value as Resultado<NotaOutputModel>;
        resposta.Should().NotBeNull();
        resposta!.Dado.Should().NotBeNull();
        resposta.Sucesso.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Cancelar_QuandoResultadoDeFalha_DeveRetornarBadRequest))]
    [Trait("Api", "Nota - Controller")]
    public async Task Cancelar_QuandoResultadoDeFalha_DeveRetornarBadRequest()
    {
        var retorno = _fixture.RetornaResultadoFalha();
        _mediatorHandlerMock.Setup(x =>
                x.EnviarRequest<Resultado<NotaOutputModel>, CancelarNotaInput>(
                    It.IsAny<CancelarNotaInput>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(retorno);

        var resultado = await _sut.Cancelar(It.IsAny<CancelarNotaInput>(), It.IsAny<CancellationToken>()) as ObjectResult;
        resultado.Should().NotBeNull();
        resultado!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        var resposta = resultado!.Value as Resultado<NotaOutputModel>;
        resposta.Should().NotBeNull();
        resposta!.Dado.Should().BeNull();
        resposta.Erro.Should().Be(TipoErro.NotaInvalida);
        resposta.Sucesso.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Cancelar_QuandoNotaNaoEncontrada_DeveRetornarNotFound))]
    [Trait("Api", "Nota - Controller")]
    public async Task Cancelar_QuandoNotaNaoEncontrada_DeveRetornarNotFound()
    {
        var retorno = _fixture.RetornaResultadoNotaNaoEncontrada();
        _mediatorHandlerMock.Setup(x =>
                x.EnviarRequest<Resultado<NotaOutputModel>, CancelarNotaInput>(
                    It.IsAny<CancelarNotaInput>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(retorno);

        var resultado = await _sut.Cancelar(It.IsAny<CancelarNotaInput>(), It.IsAny<CancellationToken>()) as ObjectResult;
        resultado.Should().NotBeNull();
        resultado!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        var resposta = resultado!.Value as Resultado<NotaOutputModel>;
        resposta.Should().NotBeNull();
        resposta!.Dado.Should().BeNull();
        resposta.Erro.Should().Be(TipoErro.RecursoNaoEncontrado);
        resposta.Sucesso.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Buscar_QuandoNaoDevolvidoNotas_DeveRetornarOkComListagemVazia))]
    [Trait("Api", "Nota - Controller")]
    public async Task Buscar_QuandoNaoDevolvidoNotas_DeveRetornarOkComListagemVazia()
    {
        var retorno = _fixture.RetornarResultadoComLista();
        _mediatorHandlerMock.Setup(x =>
                x.EnviarRequest<Resultado<ListaNotaOutput>, ListaNotaInput>(
                    It.IsAny<ListaNotaInput>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(retorno);

        var resultado = await _sut.Buscar(It.IsAny<ListaNotaInput>(), It.IsAny<CancellationToken>()) as ObjectResult;
        resultado.Should().NotBeNull();
        resultado!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var resposta = resultado!.Value as Resultado<ListaNotaOutput>;
        resposta.Should().NotBeNull();
        resposta!.Dado.Should().NotBeNull();
        resposta.Dado.Items.Should().HaveCount(default(int));
        resposta.Sucesso.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Cancelar_QuandoResultadoSucesso_DeveRetornarOk))]
    [Trait("Api", "Nota - Controller")]
    public async Task Buscar_QuandoDevolvidoNotas_DeveRetornarOkComListagem()
    {
        var retorno = _fixture.RetornarResultadoComLista(10);
        _mediatorHandlerMock.Setup(x =>
                x.EnviarRequest<Resultado<ListaNotaOutput>, ListaNotaInput>(
                    It.IsAny<ListaNotaInput>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(retorno);

        var resultado = await _sut.Buscar(It.IsAny<ListaNotaInput>(), It.IsAny<CancellationToken>()) as ObjectResult;
        resultado.Should().NotBeNull();
        resultado!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var resposta = resultado!.Value as Resultado<ListaNotaOutput>;
        resposta.Should().NotBeNull();
        resposta!.Dado.Should().NotBeNull();
        resposta.Dado.Items.Should().HaveCount(10);
        resposta.Sucesso.Should().BeTrue();
    }
}
