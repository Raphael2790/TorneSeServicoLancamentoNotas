using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Enums;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;
using TorneSe.ServicoLancamentoNotas.Dominio.Clients;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.Curso;
using TorneSe.ServicoLancamentoNotas.Testes.Fakes;

namespace TorneSe.ServicoLancamentoNotas.Testes.Aplicacao.CasosDeUsoNota.Atualizar;

[Collection(nameof(AtualizarNotaTestsFixture))]
public class AtualizarNotaTests
{
    private readonly AtualizarNotaTestsFixture _fixture;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<INotaRepository> _repositoryMock;
    private readonly Mock<ILogger<AtualizarNota>> _loggerMock;
    private readonly Mock<ICursoClient> _cursoClientMock;
    private readonly AtualizarNota _sut;

    public AtualizarNotaTests(AtualizarNotaTestsFixture fixture)
    {
        _fixture = fixture;
        _repositoryMock = new();
        _unitOfWorkMock = new();
        _loggerMock = new();
        _cursoClientMock = new Mock<ICursoClient>();
        _sut = new(_repositoryMock.Object, _unitOfWorkMock.Object, _loggerMock.Object, _cursoClientMock.Object);
    }

    [Fact(DisplayName = nameof(Handle_QuandoAtualizarInput_DeveRetonarResultadoDeSucesso))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoAtualizarInput_DeveRetonarResultadoDeSucesso()
    {
        //Arrange
        var nota = _fixture.RetornaNota();
        var input = _fixture.RetornaInputValido();
        _repositoryMock.Setup(x => x.BuscarNotaPorAlunoEAtividade(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(nota);
        _cursoClientMock
            .Setup(x => x.ObterInformacoesCursoAluno(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CursoFake.ObterCursoAluno(input.AtividadeId, input.AlunoId, input.ProfessorId));

        //Act
        var output = await _sut.Handle(input, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Should().BeOfType<Resultado<NotaOutputModel>>();
        output.Sucesso.Should().BeTrue();
        output.Erro.Should().BeNull();
        output.Dado.ValorNota.Should().Be(input.ValorNota);
        _repositoryMock.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()));
        _repositoryMock.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = nameof(Handle_QuandoAtualizarInput_DeveRetonarResultadoDeSucesso))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoVinculoCursoNaoIdentificado_DeveRetonarResultadoErro()
    {
        //Arrange
        var nota = _fixture.RetornaNota();
        var input = _fixture.RetornaInputValido();
        _repositoryMock.Setup(x => x.BuscarNotaPorAlunoEAtividade(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(nota);
        _cursoClientMock
            .Setup(x => x.ObterInformacoesCursoAluno(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CursoFake.ObterCursoAluno());

        //Act
        var output = await _sut.Handle(input, CancellationToken.None);

        //Assert
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

    [Fact(DisplayName = nameof(Handle_QuandoNotaNaoEncontrada_DeveRetornarResultadoDeFalha))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoNotaNaoEncontrada_DeveRetornarResultadoDeFalha()
    {
        //Arrange
        var input = _fixture.RetornaInputValido();
        _repositoryMock.Setup(x => x.BuscarNotaPorAlunoEAtividade(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Nota)null!);

        //Act
        var output = await _sut.Handle(input, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Should().BeOfType<Resultado<NotaOutputModel>>();
        output.Sucesso.Should().BeFalse();
        output.Erro.Should().Be(TipoErro.RecursoNaoEncontrado);
        output.DescricaoErro.Should().NotBeEmpty();
        output.Dado.Should().BeNull();
        _repositoryMock.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = nameof(Handle_QuandoNotaNaoEncontrada_DeveRetornarResultadoDeFalha))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoLancadaExcecaoNaoEsperada_DeveRetornarResultadoDeFalha()
    {
        //Arrange
        var input = _fixture.RetornaInputValido();
        _repositoryMock.Setup(x => x.BuscarNotaPorAlunoEAtividade(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        //Act
        var output = await _sut.Handle(input, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Should().BeOfType<Resultado<NotaOutputModel>>();
        output.Sucesso.Should().BeFalse();
        output.Erro.Should().Be(TipoErro.ErroInesperado);
        output.DescricaoErro.Should().NotBeEmpty();
        output.Dado.Should().BeNull();
        _repositoryMock.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = nameof(Handle_QuandoNotaNaoEncontrada_DeveRetornarResultadoDeFalha))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoNotaInvalida_DeveRetornarResultadoDeFalha()
    {
        //Arrange
        var nota = _fixture.RetornaNota();
        var input = _fixture.RetornaInputInvalido();
        _repositoryMock.Setup(x => x.BuscarNotaPorAlunoEAtividade(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(nota);

        //Act
        var output = await _sut.Handle(input, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Should().BeOfType<Resultado<NotaOutputModel>>();
        output.Sucesso.Should().BeFalse();
        output.Erro.Should().Be(TipoErro.NotaInvalida);
        output.DescricaoErro.Should().NotBeEmpty();
        output.DetalhesErros.Should().NotBeEmpty();
        output.DetalhesErros.Should().HaveCount(1);
        output.Dado.Should().BeNull();
        _repositoryMock.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }
}
