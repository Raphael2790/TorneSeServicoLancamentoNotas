using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Enums;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork.BuscaRepository;

namespace TorneSe.ServicoLancamentoNotas.Testes.Aplicacao.CasosDeUsoNota.Consultar;

[Collection(nameof(ConsultarNotaTestsFixture))]
public class ConsultarNotaTests
{
    private readonly ConsultarNotaTestsFixture _fixture;
    private readonly Mock<INotaRepository> _notaRepository;
    private readonly Mock<ILogger<ConsultaNota>> _logger;
    private readonly IConsultaNota _sut;
    
    public ConsultarNotaTests(ConsultarNotaTestsFixture fixture)
    {
        _fixture = fixture;
        _notaRepository = new();
        _logger = new();    
        _sut = new ConsultaNota(_notaRepository.Object, _logger.Object);
    }

    [Fact(DisplayName = nameof(Handle_QuandoBuscaRetornaValores_DeveRetornarResultadoComSucessoEValores))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoBuscaRetornaValores_DeveRetornarResultadoComSucessoEValores()
    {
        //Arrange
        var buscaInput = _fixture.RetornaListBuscaInput();
        var buscaOutput = _fixture.RetornaOutputRepositorio();
        _notaRepository.Setup(x => x.Buscar(It.IsAny<BuscaInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(buscaOutput);

        //Act
        var output = await _sut.Handle(buscaInput, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Should().BeAssignableTo<Resultado<ListaNotaOutput>>();
        output.Sucesso.Should().BeTrue();
        output.Erro.Should().BeNull();
        output.DescricaoErro.Should().BeNull();
        output.Dado.Total.Should().Be(buscaOutput.Total);
        output.Dado.Pagina.Should().Be(buscaOutput.Pagina);
        output.Dado.PorPagina.Should().Be(buscaOutput.PorPagina);
        output.Dado.Items.Should().HaveCount(buscaOutput.Items.Count);
        output.Dado.Items.ToList().ForEach(item =>
        {
            var nota = buscaOutput.Items
                    .FirstOrDefault(x => x.AtividadeId == item.AtividadeId && item.AlunoId == x.AlunoId);
            nota.Should().NotBeNull();
            item.ValorNota.Should().Be(nota!.ValorNota);
            item.StatusIntegracao.Should().Be(nota.StatusIntegracao);
            item.Cancelada.Should().Be(nota.Cancelada);
        });
    }

    [Fact(DisplayName = nameof(Handle_QuandoBuscaNaoRetornaValores_DeveRetornarResultadoComSucessoOutputVazio))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoBuscaNaoRetornaValores_DeveRetornarResultadoComSucessoOutputVazio()
    {
        //Arrange
        var buscaInput = _fixture.RetornaListBuscaInput();
        var buscaOutput = _fixture.RetornaOutputRepositorioVazio();
        _notaRepository.Setup(x => x.Buscar(It.IsAny<BuscaInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(buscaOutput);

        //Act
        var output = await _sut.Handle(buscaInput, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Should().BeAssignableTo<Resultado<ListaNotaOutput>>();
        output.Sucesso.Should().BeTrue();
        output.Erro.Should().BeNull();
        output.DescricaoErro.Should().BeNull();
        output.Dado.Total.Should().Be(buscaOutput.Total);
        output.Dado.Pagina.Should().Be(buscaOutput.Pagina);
        output.Dado.PorPagina.Should().Be(buscaOutput.PorPagina);
        output.Dado.Items.Should().HaveCount(buscaOutput.Items.Count);
    }

    [Fact(DisplayName = nameof(Handle_QuandoBuscaLancaExcecaoNaoEsperada_DeveRetornarResultadoComFalhaEErro))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoBuscaLancaExcecaoNaoEsperada_DeveRetornarResultadoComFalhaEErro()
    {
        //Arrange
        var buscaInput = _fixture.RetornaListBuscaInput();
        _notaRepository.Setup(x => x.Buscar(It.IsAny<BuscaInput>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        //Act
        var output = await _sut.Handle(buscaInput, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Should().BeAssignableTo<Resultado<ListaNotaOutput>>();
        output.Sucesso.Should().BeFalse();
        output.Erro.Should().NotBeNull();
        output.Erro.Should().Be(TipoErro.ErroInesperado);
        output.DescricaoErro.Should().NotBeNull();
        output.Dado.Should().BeNull();
    }
}
