using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Enums;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;
using TorneSe.ServicoLancamentoNotas.Dominio.Clients;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;
using TorneSe.ServicoLancamentoNotas.Testes.Fakes;

namespace TorneSe.ServicoLancamentoNotas.Testes.Aplicacao.CasosDeUsoNota.Cancelar;

[Collection(nameof(CancelarNotaTestsFixture))]
public class CancelarNotaTests
{
    private readonly CancelarNotaTestsFixture _fixture;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<INotaRepository> _repositoryMock;
    private readonly Mock<ILogger<CancelarNota>> _logger;
    private readonly Mock<ICursoClient> _cursoClientMock;
    private readonly CancelarNota _sut;

    public CancelarNotaTests(CancelarNotaTestsFixture fixture)
    {
        _fixture = fixture;
        _unitOfWorkMock = new();
        _repositoryMock = new();
        _logger = new();
        _cursoClientMock = new Mock<ICursoClient>();
        _sut = new(_repositoryMock.Object,_unitOfWorkMock.Object, _logger.Object, _cursoClientMock.Object);
    }

    [Fact(DisplayName = nameof(Handle_QuandoCancelarInput_DeveCancelarNota))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoCancelarInput_DeveCancelarNota()
    {
        var nota = _fixture.RetornaNota();
        var input = _fixture.RetornaInput();
        _repositoryMock.Setup(x => x.BuscarNotaPorAlunoEAtividade(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(nota);
        _cursoClientMock
            .Setup(x => x.ObterInformacoesCursoAluno(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CursoFake.ObterCursoAluno(input.AtividadeId, input.AlunoId, input.ProfessorId));

        var output = await _sut.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Should().BeOfType<Resultado<NotaOutputModel>>();
        output.Dado.MotivoCancelamento.Should().Be(input.Motivo);
        nota.Cancelada.Should().BeTrue();
        _repositoryMock.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()));
        _repositoryMock.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = nameof(Handle_QuandoVinculoCursoNaoIdentificado_DeveRetornarErro))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoVinculoCursoNaoIdentificado_DeveRetornarErro()
    {
        var nota = _fixture.RetornaNota();
        var input = _fixture.RetornaInput();
        _repositoryMock.Setup(x => x.BuscarNotaPorAlunoEAtividade(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(nota);
        _cursoClientMock
            .Setup(x => x.ObterInformacoesCursoAluno(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CursoFake.ObterCursoAluno());

        var output = await _sut.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Should().BeOfType<Resultado<NotaOutputModel>>();
        output.Sucesso.Should().BeFalse();
        output.Erro.Should().Be(TipoErro.NaoFoiPossivelValidarVinculosCurso);
        output.DescricaoErro.Should().NotBeEmpty();
        output.Dado.Should().BeNull();
        _repositoryMock.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = nameof(Handle_QuandoNotaNaoEncontrada_DeveRetornarInSucesso))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoNotaNaoEncontrada_DeveRetornarInSucesso()
    {
        var input = _fixture.RetornaInput();
        _repositoryMock.Setup(x => x.BuscarNotaPorAlunoEAtividade(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Nota)null!);

        var output = await _sut.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Should().BeOfType<Resultado<NotaOutputModel>>();
        output.Sucesso.Should().BeFalse();
        output.Erro.Should().Be(TipoErro.RecursoNaoEncontrado);
        _repositoryMock.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = nameof(Handle_QuandoInputInvalido_DeveRetornarInSucesso))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoInputInvalido_DeveRetornarInSucesso()
    {
        var nota = _fixture.RetornaNota();
        var input = _fixture.RetornaInputInvalido();
        _repositoryMock.Setup(x => x.BuscarNotaPorAlunoEAtividade(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(nota);

        var output = await _sut.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Should().BeOfType<Resultado<NotaOutputModel>>();
        output.Sucesso.Should().BeFalse();
        output.Erro.Should().Be(TipoErro.NotaInvalida);
        output.DetalhesErros.Should().NotBeEmpty();
        output.DetalhesErros.Should().HaveCount(1);
        _repositoryMock.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = nameof(Handle_QuandoNotaNaoEncontrada_DeveRetornarInSucesso))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoExcecaoNaoEsperada_DeveRetornarInSucesso()
    {
        var input = _fixture.RetornaInputInvalido();
        _repositoryMock.Setup(x => x.BuscarNotaPorAlunoEAtividade(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var output = await _sut.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Should().BeOfType<Resultado<NotaOutputModel>>();
        output.Sucesso.Should().BeFalse();
        output.Erro.Should().Be(TipoErro.ErroInesperado);
        _repositoryMock.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }
}
