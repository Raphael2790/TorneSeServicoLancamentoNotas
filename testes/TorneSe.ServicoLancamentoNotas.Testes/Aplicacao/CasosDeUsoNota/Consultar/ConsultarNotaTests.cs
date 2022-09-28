using FluentAssertions;
using Moq;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.Interfaces;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork.BuscaRepository;

namespace TorneSe.ServicoLancamentoNotas.Testes.Aplicacao.CasosDeUsoNota.Consultar;

[Collection(nameof(ConsultarNotaTestsFixture))]
public class ConsultarNotaTests
{
    private readonly ConsultarNotaTestsFixture _fixture;
    private readonly Mock<INotaRepository> _notaRepository;
    private readonly IConsultaNota _sut;
    
    public ConsultarNotaTests(ConsultarNotaTestsFixture fixture)
    {
        _fixture = fixture;
        _notaRepository = new();
        _sut = new ConsultaNota(_notaRepository.Object);
    }

    [Fact(DisplayName = nameof(Handle_QuandoBuscaRetornaValores_DeveRetornarOutputComValores))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoBuscaRetornaValores_DeveRetornarOutputComValores()
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
        output.Should().BeAssignableTo<ListaNotaOutput>();
        output.Total.Should().Be(buscaOutput.Total);
        output.Pagina.Should().Be(buscaOutput.Pagina);
        output.PorPagina.Should().Be(buscaOutput.PorPagina);
        output.Items.Should().HaveCount(buscaOutput.Items.Count);
        output.Items.ToList().ForEach(item =>
        {
            var nota = buscaOutput.Items
                    .FirstOrDefault(x => x.AtividadeId == item.AtividadeId && item.AlunoId == x.AlunoId);
            nota.Should().NotBeNull();
            item.ValorNota.Should().Be(nota!.ValorNota);
            item.StatusIntegracao.Should().Be(nota.StatusIntegracao);
            item.Cancelada.Should().Be(nota.Cancelada);
        });
    }

    [Fact(DisplayName = nameof(Handle_QuandoBuscaRetornaValores_DeveRetornarOutputComValores))]
    [Trait("Aplicacao", "Nota - Casos de Uso")]
    public async Task Handle_QuandoBuscaNaoRetornaValores_DeveRetornarOutputVazio()
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
        output.Should().BeAssignableTo<ListaNotaOutput>();
        output.Total.Should().Be(buscaOutput.Total);
        output.Pagina.Should().Be(buscaOutput.Pagina);
        output.PorPagina.Should().Be(buscaOutput.PorPagina);
        output.Items.Should().HaveCount(buscaOutput.Items.Count);
    }
}
