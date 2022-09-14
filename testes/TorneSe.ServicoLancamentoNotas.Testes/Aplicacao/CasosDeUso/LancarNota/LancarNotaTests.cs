using FluentAssertions;
using Moq;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;
using CasosDeUsoNota = TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar;

namespace TorneSe.ServicoLancamentoNotas.Testes.Aplicacao.CasosDeUso.LancarNota;

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
        _sut = new CasosDeUsoNota.LancarNota(_notaRepository.Object,_unitOfWork.Object);
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
}
