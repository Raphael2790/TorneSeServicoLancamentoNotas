using FluentAssertions;
using Microsoft.Extensions.Logging;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;
using TorneSe.ServicoLancamentoNotas.Dominio.Enums;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Contexto;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Repositories;

namespace TornseSe.ServicoLancamentoNotas.TestesIntegracao.Aplicacao.CasosDeUso.Nota.Consultar;

[Collection(nameof(ConsultarNotaTestsFixture))]
public class ConsultarNotaTests
{
    private readonly ConsultarNotaTestsFixture _fixture;
    private readonly INotaRepository _notaRepository;
    private readonly ILogger<ConsultaNota> _logger;
    private readonly ServicoLancamentoNotaDbContext _context;
    private readonly IConsultaNota _sut;

    public ConsultarNotaTests(ConsultarNotaTestsFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.CriarDbContext();
        _notaRepository = new NotaRepository(_context);
        var loggerFactory = new LoggerFactory();
        _logger = loggerFactory.CreateLogger<ConsultaNota>();
        _sut = new ConsultaNota(_notaRepository, _logger);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    [Fact(DisplayName = nameof(Handle_QuandoBuscaRetornaValores_DeveRetornarResultadoComSucessoEValores))]
    [Trait("Aplicacao", "Integracao/ConsultaNota - Casos de Uso")]
    public async Task Handle_QuandoBuscaRetornaValores_DeveRetornarResultadoComSucessoEValores()
    {
        var notas = _fixture.RetornaNotasValidas();
        await _context.AddRangeAsync(notas);
        await _context.SaveChangesAsync();
        var buscaInput = _fixture.RetornaListBuscaInput();

        var output = await _sut.Handle(buscaInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Should().BeAssignableTo<Resultado<ListaNotaOutput>>();
        output.Sucesso.Should().BeTrue();
        output.Erro.Should().BeNull();
        output.DescricaoErro.Should().BeNull();
        output.Dado.Total.Should().Be(notas.Count);
        output.Dado.Pagina.Should().Be(buscaInput.Pagina);
        output.Dado.PorPagina.Should().Be(buscaInput.PorPagina);
        output.Dado.Items.Should().HaveCount(buscaInput.PorPagina);
        output.Dado.Items.ToList().ForEach(item =>
        {
            var nota = notas
                    .FirstOrDefault(x => x.AtividadeId == item.AtividadeId && item.AlunoId == x.AlunoId);
            nota.Should().NotBeNull();
            item.ValorNota.Should().Be(nota!.ValorNota);
            item.StatusIntegracao.Should().Be(nota.StatusIntegracao);
            item.Cancelada.Should().Be(nota.Cancelada);
        });
    }

    [Fact(DisplayName = nameof(Handle_QuandoBuscaNaoRetornaValores_DeveRetornarResultadoComSucessoOutputVazio))]
    [Trait("Aplicacao", "Integracao/ConsultaNota - Casos de Uso")]
    public async Task Handle_QuandoBuscaNaoRetornaValores_DeveRetornarResultadoComSucessoOutputVazio()
    {
        var buscaInput = _fixture.RetornaListBuscaInput();

        var output = await _sut.Handle(buscaInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Should().BeAssignableTo<Resultado<ListaNotaOutput>>();
        output.Sucesso.Should().BeTrue();
        output.Erro.Should().BeNull();
        output.DescricaoErro.Should().BeNull();
        output.Dado.Total.Should().Be(default);
        output.Dado.Pagina.Should().Be(buscaInput.Pagina);
        output.Dado.PorPagina.Should().Be(buscaInput.PorPagina);
        output.Dado.Items.Should().HaveCount(default(int));
    }

    [Theory(DisplayName = nameof(DeveRetornarResultadoComSucessoEValores))]
    [Trait("Aplicacao", "Integracao/ConsultaNota - Casos de Uso")]
    [InlineData(OrdenacaoBusca.Desc, "alunoid")]
    [InlineData(OrdenacaoBusca.Asc, "alunoid")]
    [InlineData(OrdenacaoBusca.Desc, "atividadeid")]
    [InlineData(OrdenacaoBusca.Asc, "atividadeid")]
    public async Task DeveRetornarResultadoComSucessoEValores(OrdenacaoBusca ordenacao, string ordernarPor)
    {
        var notas = _fixture.RetornaNotasValidas();
        await _context.AddRangeAsync(notas);
        await _context.SaveChangesAsync();
        var buscaInput =  _fixture.RetornaBuscaInputApenasComPaginacao(porPagina : 20, ordenacao: ordenacao, ordernarPor: ordernarPor);

        var output = await _sut.Handle(buscaInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Dado.Pagina.Should().Be(buscaInput.Pagina);
        output.Dado.PorPagina.Should().Be(buscaInput.PorPagina);
        output.Dado.Items.Should().NotBeEmpty();
        output.Dado.Total.Should().Be(notas.Count());

        var listaOrdenada = _fixture.NotasOrdernadas(notas, ordernarPor, ordenacao);
        for (int index = 0; index < notas.Count(); index++)
        {
            var notaLista = listaOrdenada[index];
            notaLista.Should().NotBeNull();
            output.Dado.Items[index].ValorNota.Should().Be(notaLista!.ValorNota);
            output.Dado.Items[index].AlunoId.Should().Be(notaLista.AlunoId);
            output.Dado.Items[index].AtividadeId.Should().Be(notaLista.AtividadeId);
        }
    }
}
