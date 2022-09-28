using FluentAssertions;
using Moq;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;

namespace TorneSe.ServicoLancamentoNotas.Testes.Aplicacao.CasosDeUsoNota.Lancar;

[Collection(nameof(LancarNotaTestsFixture))]
public class LancarNotaTests
{
    private readonly LancarNotaTestsFixture _fixture;
    private readonly Mock<INotaRepository> _notaRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly ILancarNota _sut;

    public LancarNotaTests(LancarNotaTestsFixture fixture)
    {
        _fixture = fixture;
        _notaRepository = new Mock<INotaRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _sut = new LancarNota(_notaRepository.Object, _unitOfWork.Object);
    }

    [Fact(DisplayName = nameof(Handle_QuandoNotaValidaParaSerSalva_DeveSerSalvar))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoNotaValidaParaSerSalva_DeveSerSalvar()
    {
        var input = _fixture.DevolveNotaInputValido();

        var output = await _sut.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Should().BeOfType<NotaOutputModel>();
        _notaRepository.Verify(x => x.Inserir(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = nameof(Handle_QuandoNotaValidaParaSerSalva_DeveSerSalvar))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoNotaValidaParaSerSalvaESubstitutiva_DeveSerSalvarEAtualizarNotaSubstituida()
    {
        var nota = _fixture.RetornaNota();
        _notaRepository.Setup(x => x.BuscarNotaPorAlunoEAtividade(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(nota);
        var input = _fixture.DevolveNotaInputValidoSustitutivo();

        var output = await _sut.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Should().BeOfType<NotaOutputModel>();
        nota.CanceladaPorRetentativa.Should().BeTrue();
        nota.MotivoCancelamento.Should().NotBeEmpty();
        _notaRepository.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()));
        _notaRepository.Verify(x => x.Inserir(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Once);
        _notaRepository.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
}
