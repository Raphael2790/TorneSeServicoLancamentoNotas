using FluentAssertions;
using Moq;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;

namespace TorneSe.ServicoLancamentoNotas.Testes.Aplicacao.CasosDeUsoNota.Cancelar;

[Collection(nameof(CancelarNotaTestsFixture))]
public class CancelarNotaTests
{
    private readonly CancelarNotaTestsFixture _fixture;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<INotaRepository> _repositoryMock;
    private readonly CancelarNota _sut;

    public CancelarNotaTests(CancelarNotaTestsFixture fixture)
    {
        _fixture = fixture;
        _unitOfWorkMock = new();
        _repositoryMock = new();
        _sut = new(_repositoryMock.Object,_unitOfWorkMock.Object);
    }

    [Fact(DisplayName = "")]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoCancelarInput_DeveCancelarNota()
    {
        var nota = _fixture.RetornaNota();
        var input = _fixture.RetornaInput();
        _repositoryMock.Setup(x => x.BuscarNotaPorAlunoEAtividade(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(nota);

        var output = await _sut.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Should().BeOfType<NotaOutputModel>();
        output.MotivoCancelamento.Should().Be(input.Motivo);
        nota.Cancelada.Should().BeTrue();
        _repositoryMock.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()));
        _repositoryMock.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
}
