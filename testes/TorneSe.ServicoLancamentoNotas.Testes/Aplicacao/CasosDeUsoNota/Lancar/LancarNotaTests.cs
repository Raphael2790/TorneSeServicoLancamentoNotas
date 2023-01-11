using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Enums;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;
using TorneSe.ServicoLancamentoNotas.Dominio.Clients;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;

namespace TorneSe.ServicoLancamentoNotas.Testes.Aplicacao.CasosDeUsoNota.Lancar;

[Collection(nameof(LancarNotaTestsFixture))]
public class LancarNotaTests
{
    private readonly LancarNotaTestsFixture _fixture;
    private readonly Mock<INotaRepository> _notaRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<ILogger<LancarNota>> _logger;
    private readonly Mock<ICursoClient> _cursoClient;
    private readonly ILancarNota _sut;

    public LancarNotaTests(LancarNotaTestsFixture fixture)
    {
        _fixture = fixture;
        _notaRepository = new Mock<INotaRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _logger = new Mock<ILogger<LancarNota>>();
        _cursoClient = new Mock<ICursoClient>();
        _sut = new LancarNota(_notaRepository.Object, _unitOfWork.Object, _logger.Object, _cursoClient.Object);
    }

    [Fact(DisplayName = nameof(Handle_QuandoNotaValida_DeveSerSalvar))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoNotaValida_DeveSerSalvar()
    {
        var input = _fixture.DevolveNotaInputValido();

        var output = await _sut.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Should().BeOfType<Resultado<NotaOutputModel>>();
        output.Sucesso.Should().BeTrue();
        output.Dado.Should().NotBeNull();
        output.Dado.ValorNota.Should().Be(input.ValorNota);
        output.Dado.AtividadeId.Should().Be(input.AtividadeId);
        output.Dado.AlunoId.Should().Be(input.AlunoId);
        _notaRepository.Verify(x => x.Inserir(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = nameof(Handle_QuandoNotaValidaParaSerSalvaESubstitutiva_DeveSerSalvarEAtualizarNotaSubstituida))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoNotaValidaParaSerSalvaESubstitutiva_DeveSerSalvarEAtualizarNotaSubstituida()
    {
        var nota = _fixture.RetornaNota();
        _notaRepository.Setup(x => x.BuscarNotaPorAlunoEAtividade(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(nota);
        var input = _fixture.DevolveNotaInputValidoSustitutivo();

        var output = await _sut.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Should().BeOfType<Resultado<NotaOutputModel>>();
        output.Sucesso.Should().BeTrue();
        output.Dado.Should().NotBeNull();
        output.Dado.ValorNota.Should().Be(input.ValorNota);
        output.Dado.AtividadeId.Should().Be(input.AtividadeId);
        output.Dado.AlunoId.Should().Be(input.AlunoId);
        nota.CanceladaPorRetentativa.Should().BeTrue();
        nota.MotivoCancelamento.Should().NotBeEmpty();
        _notaRepository.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()));
        _notaRepository.Verify(x => x.Inserir(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Once);
        _notaRepository.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = nameof(Handle_QuandoNotaInvalida_NaoDeveSerSalvar))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoNotaInvalida_NaoDeveSerSalvar()
    {
        var input = _fixture.DevolveNotaInputInvalido();

        var output = await _sut.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Should().BeOfType<Resultado<NotaOutputModel>>();
        output.Dado.Should().BeNull();
        output.Sucesso.Should().BeFalse();
        output.DetalhesErros.Should().NotBeEmpty();
        output.DetalhesErros.Should().HaveCount(3);
        output.Erro.Should().Be(TipoErro.NotaInvalida);
        _notaRepository.Verify(x => x.Inserir(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = nameof(Handle_QuandoNotaInvalida_NaoDeveSerSalvar))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoExcecaoInesperada_NaoDeveSerSalvar()
    {
        var input = _fixture.DevolveNotaInputValido();
        _notaRepository.Setup(x => x.Inserir(It.IsAny<Nota>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var output = await _sut.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Should().BeOfType<Resultado<NotaOutputModel>>();
        output.Dado.Should().BeNull();
        output.Sucesso.Should().BeFalse();
        output.Erro.Should().Be(TipoErro.ErroInesperado);
        _notaRepository.Verify(x => x.Inserir(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = nameof(Handle_QuandoNotaValidaParaSerSalvaESubstitutivaNaoEncontrada_DeveSerSalvar))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoNotaValidaParaSerSalvaESubstitutivaNaoEncontrada_DeveSerSalvar()
    {
        _notaRepository.Setup(x => x.BuscarNotaPorAlunoEAtividade(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Nota)null!);
        var input = _fixture.DevolveNotaInputValidoSustitutivo();

        var output = await _sut.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Should().BeOfType<Resultado<NotaOutputModel>>();
        output.Sucesso.Should().BeTrue();
        output.Dado.Should().NotBeNull();
        output.Dado.ValorNota.Should().Be(input.ValorNota);
        output.Dado.AtividadeId.Should().Be(input.AtividadeId);
        output.Dado.AlunoId.Should().Be(input.AlunoId);
        _notaRepository.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()), Times.Once);
        _notaRepository.Verify(x => x.Inserir(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Once);
        _notaRepository.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
}
