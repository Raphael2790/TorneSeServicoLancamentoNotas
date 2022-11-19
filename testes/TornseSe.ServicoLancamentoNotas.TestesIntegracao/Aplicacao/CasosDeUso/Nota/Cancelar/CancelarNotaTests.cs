using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Enums;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Contexto;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Repositories;
using TorneSe.ServicoLancamentoNotas.Infra.Data.UoW;

namespace TornseSe.ServicoLancamentoNotas.TestesIntegracao.Aplicacao.CasosDeUso.Nota.Cancelar;

[Collection(nameof(CancelarNotaTestsFixture))]
public class CancelarNotaTests
{
    private readonly CancelarNotaTestsFixture _fixture;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotaRepository _notaRepository;
    private readonly ILogger<CancelarNota> _logger;
    private readonly ServicoLancamentoNotaDbContext _context;
    private readonly ICancelarNota _sut;

    public CancelarNotaTests(CancelarNotaTestsFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.CriarDbContext();
        _unitOfWork = new UnitOfWork(_context);
        _notaRepository = new NotaRepository(_context);
        var loggerFactory = new LoggerFactory();
        _logger = loggerFactory.CreateLogger<CancelarNota>();
        _sut = new CancelarNota(_notaRepository, _unitOfWork, _logger);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    [Fact(DisplayName = nameof(Handle_QuandoCancelarInput_DeveCancelarNota))]
    [Trait("Aplicacao", "Integracao/CancelarNota - Casos de Uso")]
    public async Task Handle_QuandoCancelarInput_DeveCancelarNota()
    {
        var nota = _fixture.RetornaNota();
        var motivoCancelamento = "Lançamento indevido";
        var input = _fixture.RetornaInput(nota.AlunoId, nota.AtividadeId, motivoCancelamento);
        var tracking = await _context.Notas.AddAsync(nota);
        await _context.SaveChangesAsync();
        tracking.State = EntityState.Detached;

        var resposta = await _sut.Handle(input, CancellationToken.None);

        resposta.Should().NotBeNull();
        resposta.Sucesso.Should().BeTrue();
        resposta.Dado.MotivoCancelamento.Should().Be(motivoCancelamento);
        resposta.Dado.Cancelada.Should().BeTrue();

        var notaSalva = await _context.Notas
            .FirstOrDefaultAsync(x => x.AlunoId == nota.AlunoId && x.AtividadeId == nota.AtividadeId);
        notaSalva.Should().NotBeNull();
        notaSalva!.MotivoCancelamento.Should().Be(motivoCancelamento);
        notaSalva!.Cancelada.Should().BeTrue();
        notaSalva.DataAtualizacao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact(DisplayName = nameof(Handle_QuandoNotaNaoEncontrada_DeveRetornarErroNotaNaoEncontrada))]
    [Trait("Aplicacao", "Integracao/CancelarNota - Casos de Uso")]
    public async Task Handle_QuandoNotaNaoEncontrada_DeveRetornarErroNotaNaoEncontrada()
    {
       
        var motivoCancelamento = "Lançamento indevido";
        var input = _fixture.RetornaInput(motivo: motivoCancelamento);

        var resposta = await _sut.Handle(input, CancellationToken.None);

        resposta.Should().NotBeNull();
        resposta.Sucesso.Should().BeFalse();
        resposta.Dado.Should().BeNull();
        resposta.Erro.Should().Be(TipoErro.NotaNaoEncontrada);
    }

    [Fact(DisplayName = nameof(Handle_QuandoInputInvalido_DeveRetornarErroNotaInvalida))]
    [Trait("Aplicacao", "Integracao/CancelarNota - Casos de Uso")]
    public async Task Handle_QuandoInputInvalido_DeveRetornarErroNotaInvalida()
    {
        var nota = _fixture.RetornaNota();
        var input = _fixture.RetornaInput(nota.AlunoId, nota.AtividadeId);
        var tracking = await _context.Notas.AddAsync(nota);
        await _context.SaveChangesAsync();
        tracking.State = EntityState.Detached;

        var resposta = await _sut.Handle(input, CancellationToken.None);

        resposta.Should().NotBeNull();
        resposta.Sucesso.Should().BeFalse();
        resposta.Dado.Should().BeNull();
        resposta.Erro.Should().Be(TipoErro.NotaInvalida);
        var detalheErro = resposta.DetalhesErros.FirstOrDefault();
        detalheErro.Should().NotBeNull();
        detalheErro!.Campo.Should().Be("MotivoCancelamento");
    }
}
