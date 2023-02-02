using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Enums;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;
using TorneSe.ServicoLancamentoNotas.Dominio.Clients;
using TorneSe.ServicoLancamentoNotas.Dominio.Constantes;
using TorneSe.ServicoLancamentoNotas.Dominio.Enums;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Contexto;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Repositories;
using TorneSe.ServicoLancamentoNotas.Infra.Data.UoW;
using TornseSe.ServicoLancamentoNotas.TestesIntegracao.FakeComponents.CursoClient;

namespace TornseSe.ServicoLancamentoNotas.TestesIntegracao.Aplicacao.CasosDeUso.Nota.Lancar;

[Collection(nameof(LancarNotaTestsFixture))]
public class LancarNotaTests
{
    private readonly LancarNotaTestsFixture _fixture;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotaRepository _notaRepository;
    private readonly ILogger<LancarNota> _logger;
    private readonly ServicoLancamentoNotaDbContext _context;
    private readonly ICursoClient _cursoClient;
    private readonly ILancarNota _sut;

    public LancarNotaTests(LancarNotaTestsFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.CriarDbContext();
        _unitOfWork = new UnitOfWork(_context);
        _notaRepository = new NotaRepository(_context);
        var loggerFactory = new LoggerFactory();
        _logger = loggerFactory.CreateLogger<LancarNota>();
        _cursoClient = new CursoFakeClient();
        _sut = new LancarNota(_notaRepository, _unitOfWork, _logger, _cursoClient);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    [Fact(DisplayName = nameof(Handle_QuandoNotaValida_DeveSerSalvar))]
    [Trait("Aplicacao", "Integracao/LancarNota - Casos de Uso")]
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
        output.Dado.Cancelada.Should().BeFalse();

        var notaSalva = await _context.Notas
            .FirstOrDefaultAsync(x => x.AlunoId == input.AlunoId && x.AtividadeId == input.AtividadeId);
        notaSalva.Should().NotBeNull();
        notaSalva!.ValorNota.Should().Be(input.ValorNota);
        notaSalva.Cancelada.Should().BeFalse();
        notaSalva.StatusIntegracao.Should().Be(StatusIntegracao.AguardandoIntegracao);
    }

    [Fact(DisplayName = nameof(Handle_QuandoNotaInvalida_NaoDeveSerSalvar))]
    [Trait("Aplicacao", "Integracao/LancarNota - Casos de Uso")]
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

        var notaSalva = await _context.Notas
            .FirstOrDefaultAsync(x => x.AlunoId == input.AlunoId && x.AtividadeId == input.AtividadeId);
        notaSalva.Should().BeNull();
    }

    [Fact(DisplayName = nameof(Handle_QuandoNotaValidaParaSerSalvaESubstitutiva_DeveSerSalvarEAtualizarNotaSubstituida))]
    [Trait("Aplicacao", "Integracao/LancarNota - Casos de Uso")]
    public async Task Handle_QuandoNotaValidaParaSerSalvaESubstitutiva_DeveSerSalvarEAtualizarNotaSubstituida()
    {
        var nota = _fixture.RetornaNota();
        var tracking = await _context.AddAsync(nota);
        await _context.SaveChangesAsync();
        tracking.State = EntityState.Detached;
        var input = _fixture.DevolveNotaSubstitutivaInputValido(nota.AlunoId, nota.AtividadeId);

        var output = await _sut.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Should().BeOfType<Resultado<NotaOutputModel>>();
        output.Sucesso.Should().BeTrue();
        output.Dado.Should().NotBeNull();
        output.Dado.ValorNota.Should().Be(input.ValorNota);
        output.Dado.AtividadeId.Should().Be(input.AtividadeId);
        output.Dado.AlunoId.Should().Be(input.AlunoId);
        output.Dado.Cancelada.Should().BeFalse();

        var notaCancelada = await _context.Notas
            .FirstOrDefaultAsync(x => x.AtividadeId == input.AtividadeId && x.AlunoId == input.AlunoId && x.Cancelada);
        notaCancelada.Should().NotBeNull();
        notaCancelada!.Cancelada.Should().BeTrue();
        notaCancelada.CanceladaPorRetentativa.Should().BeTrue();
        notaCancelada.MotivoCancelamento.Should().Be(ConstantesDominio.Mensagens.NOTA_CANCELADA_POR_RETENTATIVA);

        var notaAtiva = await _context.Notas
            .FirstOrDefaultAsync(x => x.AtividadeId == input.AtividadeId && x.AlunoId == input.AlunoId && !x.Cancelada);
        notaAtiva.Should().NotBeNull();
    }
}
