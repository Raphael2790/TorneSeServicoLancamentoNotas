using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.Interfaces;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Enums;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;
using TorneSe.ServicoLancamentoNotas.Dominio.Clients;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Contexto;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Repositories;
using TorneSe.ServicoLancamentoNotas.Infra.Data.UoW;
using TornseSe.ServicoLancamentoNotas.TestesIntegracao.FakeComponents.CursoClient;
using TornseSe.ServicoLancamentoNotas.TestesIntegracao.FakeComponents.Mediator;

namespace TornseSe.ServicoLancamentoNotas.TestesIntegracao.Aplicacao.CasosDeUso.Nota.Atualizar;

[Collection(nameof(AtualizarNotaTestsFixture))]
public class AtualizarNotaTests
{
    private readonly AtualizarNotaTestsFixture _fixture;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotaRepository _notaRepository;
    private readonly ILogger<AtualizarNota> _logger;
    private readonly ServicoLancamentoNotaDbContext _context;
    private readonly ICursoClient _cursoClient;
    private readonly IAtualizarNota _sut;

    public AtualizarNotaTests(AtualizarNotaTestsFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.CriarDbContext();
        _unitOfWork = new UnitOfWork(_context);
        _notaRepository = new NotaRepository(_context);
        var loggerFactory = new LoggerFactory();
        _logger = loggerFactory.CreateLogger<AtualizarNota>();
        _cursoClient = new CursoFakeClient();
        var mediatorFake = new MediatorFakeHandler();
        _sut = new AtualizarNota(_notaRepository, _unitOfWork, _logger, _cursoClient, mediatorFake);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    [Fact(DisplayName = nameof(Atualizar_QuandoNotaExiste_DeveAtualizarValores))]
    [Trait("Aplicacao", "Integracao/AtualizarNota - Casos de Uso")]
    public async Task Atualizar_QuandoNotaExiste_DeveAtualizarValores()
    {   
        //Arrange
        var novoValorNota = 10;
        var nota = _fixture.RetornaNota();
        var input = _fixture.RetornaInput(nota.AlunoId, nota.AtividadeId, novoValorNota);
        var tracking = await _context.Notas.AddAsync(nota);
        await _context.SaveChangesAsync();
        tracking.State = EntityState.Detached;

        //Act
        var resposta = await _sut.Handle(input, CancellationToken.None);

        //Assert
        resposta.Should().NotBeNull();
        resposta.Sucesso.Should().BeTrue();
        resposta.Dado.ValorNota.Should().Be(novoValorNota);

        var notaSalva = await _context.Notas
            .FirstOrDefaultAsync(x => x.AlunoId == nota.AlunoId && x.AtividadeId == nota.AtividadeId);
        notaSalva.Should().NotBeNull();
        notaSalva!.ValorNota.Should().Be(novoValorNota);
    }

    [Fact(DisplayName = nameof(Atualizar_QuandoNotaNaoExiste_DeveInformarErroNotaNaoEncontrada))]
    [Trait("Aplicacao", "Integracao/AtualizarNota - Casos de Uso")]
    public async Task Atualizar_QuandoNotaNaoExiste_DeveInformarErroNotaNaoEncontrada()
    {
        //Arrange
        var input = _fixture.RetornaInput();

        //Act
        var resposta = await _sut.Handle(input, CancellationToken.None);

        //Assert
        resposta.Should().NotBeNull();
        resposta.Sucesso.Should().BeFalse();
        resposta.Dado.Should().BeNull();
        resposta.Erro.Should().Be(TipoErro.RecursoNaoEncontrado);
    }

    [Theory(DisplayName = nameof(Atualizar_QuandoNotaInformadoInputInvalido_DeveInformarErroNotaInvalida))]
    [InlineData(-1)]
    [InlineData(11)]
    [Trait("Aplicacao", "Integracao/AtualizarNota - Casos de Uso")]
    public async Task Atualizar_QuandoNotaInformadoInputInvalido_DeveInformarErroNotaInvalida(int novoValorNota)
    {
        //Arrange
        var nota = _fixture.RetornaNota();
        var input = _fixture.RetornaInput(nota.AlunoId, nota.AtividadeId, novoValorNota);
        var tracking = await _context.Notas.AddAsync(nota);
        await _context.SaveChangesAsync();
        tracking.State = EntityState.Detached;

        //Act
        var resposta = await _sut.Handle(input, CancellationToken.None);

        //Assert
        resposta.Should().NotBeNull();
        resposta.Sucesso.Should().BeFalse();
        resposta.Dado.Should().BeNull();
        resposta.Erro.Should().Be(TipoErro.NotaInvalida);
        resposta.DetalhesErros.Should().NotBeEmpty();
        resposta.DetalhesErros.Should().HaveCount(1);
        var detalheErro = resposta.DetalhesErros.FirstOrDefault();
        detalheErro.Should().NotBeNull();
        detalheErro!.Campo.Should().Be("ValorNota");
    }
}
