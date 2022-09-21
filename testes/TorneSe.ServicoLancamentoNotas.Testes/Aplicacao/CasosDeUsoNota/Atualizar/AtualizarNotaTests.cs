using FluentAssertions;
using Moq;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;

namespace TorneSe.ServicoLancamentoNotas.Testes.Aplicacao.CasosDeUsoNota.Atualizar;

[Collection(nameof(AtualizarNotaTestsFixture))]
public class AtualizarNotaTests
{
    private readonly AtualizarNotaTestsFixture _fixture;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<INotaRepository> _repositoryMock;
    private readonly AtualizarNota _sut;

    public AtualizarNotaTests(AtualizarNotaTestsFixture fixture)
    {
        _fixture = fixture;
        _repositoryMock = new();
        _unitOfWorkMock = new();
        _sut = new(_repositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact(DisplayName = nameof(Handle_QuandoAtualizarInput_DeveAtualizarNota))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoAtualizarInput_DeveAtualizarNota()
    {
        //Arrange
        var nota = _fixture.RetornaNota();
        var input = _fixture.RetornaInputValido();
        _repositoryMock.Setup(x => x.BuscarNotaPorAlunoEAtividade(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(nota);

        //Act
        var output = await _sut.Handle(input, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Should().BeOfType<NotaOutputModel>();
        output.ValorNota.Should().Be(input.ValorNota);
        _repositoryMock.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()));
        _repositoryMock.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
}
