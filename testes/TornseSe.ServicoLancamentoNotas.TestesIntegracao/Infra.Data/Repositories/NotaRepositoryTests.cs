using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TorneSe.ServicoLancamentoNotas.Dominio.Enums;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Contexto;
using InfraRepos = TorneSe.ServicoLancamentoNotas.Infra.Data.Repositories;

namespace TornseSe.ServicoLancamentoNotas.TestesIntegracao.Infra.Data.Repositories;

[Collection(nameof(NotaRepositoryTestsFixture))]
public class NotaRepositoryTests
{
    private readonly INotaRepository _sut;
    private readonly NotaRepositoryTestsFixture _fixture;
    private ServicoLancamentoNotaDbContext _context;

    public NotaRepositoryTests(NotaRepositoryTestsFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.CriarDbContext();
        _sut = new InfraRepos.NotaRepository(_context);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    [Fact(DisplayName = nameof(Inserir_QuandoFornecidaNota_DeveSalvar))]
    [Trait("Infra.Data", "Integracao/Repositories - Nota Repository")]
    public async Task Inserir_QuandoFornecidaNota_DeveSalvar()
    {
        //Arrange
        var nota = _fixture.RetornaNota();

        //Act
        await _sut.Inserir(nota, CancellationToken.None);
        await _context.SaveChangesAsync();

        //Assert
        var notaSalva = _context.Notas.FirstOrDefault(x => x.Id == nota.Id);
        notaSalva.Should().NotBeNull();
        notaSalva!.AlunoId.Should().Be(nota.AlunoId);
        notaSalva.AtividadeId.Should().Be(nota.AtividadeId);
        notaSalva.ValorNota.Should().Be(nota.ValorNota);
        notaSalva.DataLancamento.Should().Be(nota.DataLancamento);
        notaSalva.DataCriacao.Should().BeCloseTo(nota.DataCriacao, TimeSpan.FromSeconds(1));
    }

    [Fact(DisplayName = nameof(BuscarNotaPorAlunoEAtividade_QuandoNotaExiste_DeveRetornar))]
    [Trait("Infra.Data", "Integracao/Repositories - Nota Repository")]
    public async Task BuscarNotaPorAlunoEAtividade_QuandoNotaExiste_DeveRetornar()
    {
        var nota = _fixture.RetornaNota();
        await _context.Notas.AddAsync(nota);
        await _context.SaveChangesAsync();

        var notaSalva = await _sut.BuscarNotaPorAlunoEAtividade(nota.AlunoId, nota.AtividadeId, CancellationToken.None);

        notaSalva.Should().NotBeNull();
        notaSalva!.AlunoId.Should().Be(nota.AlunoId);
        notaSalva.AtividadeId.Should().Be(nota.AtividadeId);
        notaSalva.ValorNota.Should().Be(nota.ValorNota);
        notaSalva.DataLancamento.Should().Be(nota.DataLancamento);
        notaSalva.DataCriacao.Should().BeCloseTo(nota.DataCriacao, TimeSpan.FromSeconds(1));
    }

    [Fact(DisplayName = nameof(Atualizar_QuandoNotaExiste_DeveAtualizarValores))]
    [Trait("Infra.Data", "Integracao/Repositories - Nota Repository")]
    public async Task Atualizar_QuandoNotaExiste_DeveAtualizarValores()
    {
        var novoValorNota = 10;
        var nota = _fixture.RetornaNota();
        var tracking = await _context.Notas.AddAsync(nota);
        await _context.SaveChangesAsync();
        tracking.State = EntityState.Detached;
        nota!.AtualizarValorNota(novoValorNota);

        await _sut.Atualizar(nota!, CancellationToken.None);
        await _context.SaveChangesAsync();

        var notas = await _context.Notas.ToListAsync();
        var notaSalva = notas.FirstOrDefault(x => x.Id == nota.Id);
        notaSalva.Should().NotBeNull();
        notaSalva!.AlunoId.Should().Be(nota.AlunoId);
        notaSalva.AtividadeId.Should().Be(nota.AtividadeId);
        notaSalva.ValorNota.Should().Be(novoValorNota);
        notaSalva.DataLancamento.Should().Be(nota.DataLancamento);
        notaSalva.DataCriacao.Should().BeCloseTo(nota.DataCriacao, TimeSpan.FromSeconds(1));
    }

    [Fact(DisplayName = nameof(Buscar_QuandoNaoExistemNotasCadastradas_DeveRetornarListaVazia))]
    [Trait("Infra.Data", "Integracao/Repositories - Nota Repository")]
    public async Task Buscar_QuandoNaoExistemNotasCadastradas_DeveRetornarListaVazia()
    {
        //Arrange
        var input = _fixture.RetornaBuscaInputApenasComPaginacao();

        //Act
        var resultado = await _sut.Buscar(input, CancellationToken.None);

        //Assert
        resultado.Should().NotBeNull();
        resultado.Pagina.Should().Be(input.Pagina);
        resultado.PorPagina.Should().Be(input.PorPagina);
        resultado.Items.Should().BeEmpty();
        resultado.Total.Should().Be(default);
    }

    [Fact(DisplayName = nameof(Buscar_QuandoExistemNotasCadastradas_DeveRetornarLista))]
    [Trait("Infra.Data", "Integracao/Repositories - Nota Repository")]
    public async Task Buscar_QuandoExistemNotasCadastradas_DeveRetornarLista()
    {
        //Arrange
        var input = _fixture.RetornaBuscaInputApenasComPaginacao();
        var notas = _fixture.RetornarNotas();
        await _context.AddRangeAsync(notas);
        await _context.SaveChangesAsync();

        //Act
        var resultado = await _sut.Buscar(input, CancellationToken.None);

        //Assert
        resultado.Should().NotBeNull();
        resultado.Pagina.Should().Be(input.Pagina);
        resultado.PorPagina.Should().Be(input.PorPagina);
        resultado.Items.Should().NotBeEmpty();
        resultado.Total.Should().Be(notas.Count());
        foreach (var nota in resultado.Items)
        {
            var notaLista = notas.FirstOrDefault(x => x.Id == nota.Id);
            notaLista.Should().NotBeNull();
            nota.ValorNota.Should().Be(notaLista!.ValorNota);
            nota.AlunoId.Should().Be(notaLista.AlunoId);
            nota.AtividadeId.Should().Be(notaLista.AtividadeId);
        }
    }

    [Theory(DisplayName = nameof(Buscar_QuandoInformadaOrdenacao_DeveRetornarLista))]
    [Trait("Infra.Data", "Integracao/Repositories - Nota Repository")]
    [InlineData(OrdenacaoBusca.Desc, "alunoid")]
    [InlineData(OrdenacaoBusca.Asc, "alunoid")]
    [InlineData(OrdenacaoBusca.Desc, "atividadeid")]
    [InlineData(OrdenacaoBusca.Asc, "atividadeid")]
    public async Task Buscar_QuandoInformadaOrdenacao_DeveRetornarLista(OrdenacaoBusca ordenacao, string ordernarPor)
    {
        //Arrange
        var input = _fixture.RetornaBuscaInputApenasComPaginacao(ordenacao : ordenacao, ordernarPor: ordernarPor);
        var notas = _fixture.RetornarNotas();
        await _context.AddRangeAsync(notas);
        await _context.SaveChangesAsync();

        //Act
        var resultado = await _sut.Buscar(input, CancellationToken.None);

        //Assert
        var listaOrdenada = _fixture.NotasOrdernadas(notas, ordernarPor, ordenacao);
        resultado.Should().NotBeNull();
        resultado.Pagina.Should().Be(input.Pagina);
        resultado.PorPagina.Should().Be(input.PorPagina);
        resultado.Items.Should().NotBeEmpty();
        resultado.Total.Should().Be(notas.Count());
        for (int index = 0; index < notas.Count(); index++)
        {
            var notaLista = listaOrdenada[index];
            notaLista.Should().NotBeNull();
            resultado.Items[index].ValorNota.Should().Be(notaLista!.ValorNota);
            resultado.Items[index].AlunoId.Should().Be(notaLista.AlunoId);
            resultado.Items[index].AtividadeId.Should().Be(notaLista.AtividadeId);
        }
    }

    [Fact(DisplayName = nameof(Buscar_QuandoInformadoAlunoId_DeveRetornarLista))]
    [Trait("Infra.Data", "Integracao/Repositories - Nota Repository")]
    public async Task Buscar_QuandoInformadoAlunoId_DeveRetornarLista()
    {
        //Arrange
        var notas = _fixture.RetornarNotas();
        var alunoId = notas.First().AlunoId;
        var input = _fixture.RetornaBuscaInputApenasComPaginacao(alunoId);
        await _context.AddRangeAsync(notas);
        await _context.SaveChangesAsync();

        //Act
        var resultado = await _sut.Buscar(input, CancellationToken.None);

        //Assert
        resultado.Should().NotBeNull();
        resultado.Pagina.Should().Be(input.Pagina);
        resultado.PorPagina.Should().Be(input.PorPagina);
        resultado.Items.Should().NotBeEmpty();
        resultado.Total.Should().Be(notas.Count(x => x.AlunoId == alunoId));
        foreach (var nota in resultado.Items)
        {
            var notaLista = notas.FirstOrDefault(x => x.Id == nota.Id);
            notaLista.Should().NotBeNull();
            nota.ValorNota.Should().Be(notaLista!.ValorNota);
            nota.AlunoId.Should().Be(notaLista.AlunoId);
            nota.AtividadeId.Should().Be(notaLista.AtividadeId);
        }
    }

    [Fact(DisplayName = nameof(Buscar_QuandoInformadoAtividadeId_DeveRetornarLista))]
    [Trait("Infra.Data", "Integracao/Repositories - Nota Repository")]
    public async Task Buscar_QuandoInformadoAtividadeId_DeveRetornarLista()
    {
        //Arrange
        var notas = _fixture.RetornarNotas();
        var atividadeId = notas.First().AtividadeId;
        var input = _fixture.RetornaBuscaInputApenasComPaginacao(atividadeId: atividadeId);
        await _context.AddRangeAsync(notas);
        await _context.SaveChangesAsync();

        //Act
        var resultado = await _sut.Buscar(input, CancellationToken.None);

        //Assert
        resultado.Should().NotBeNull();
        resultado.Pagina.Should().Be(input.Pagina);
        resultado.PorPagina.Should().Be(input.PorPagina);
        resultado.Items.Should().NotBeEmpty();
        resultado.Total.Should().Be(notas.Count(x => x.AtividadeId == atividadeId));
        foreach (var nota in resultado.Items)
        {
            var notaLista = notas.FirstOrDefault(x => x.Id == nota.Id);
            notaLista.Should().NotBeNull();
            nota.ValorNota.Should().Be(notaLista!.ValorNota);
            nota.AlunoId.Should().Be(notaLista.AlunoId);
            nota.AtividadeId.Should().Be(notaLista.AtividadeId);
        }
    }

    [Theory(DisplayName = nameof(Buscar_QuandoInformadaPaginacao_DeveRetornarLista))]
    [InlineData(20, 1, 10, 10)]
    [InlineData(15, 2, 10, 5)]
    [Trait("Infra.Data", "Integracao/Repositories - Nota Repository")]
    public async Task Buscar_QuandoInformadaPaginacao_DeveRetornarLista
    (
        int quantidadeGerada, int pagina, int porPagina, int quantidadeItensEsperada
    )
    {
        //Arrange
        var notas = _fixture.RetornarNotas(quantidadeGerada);
        var input = _fixture.RetornaBuscaInputApenasComPaginacao(pagina : pagina, porPagina: porPagina);
        await _context.AddRangeAsync(notas);
        await _context.SaveChangesAsync();

        //Act
        var resultado = await _sut.Buscar(input, CancellationToken.None);

        //Assert
        resultado.Should().NotBeNull();
        resultado.Pagina.Should().Be(input.Pagina);
        resultado.PorPagina.Should().Be(input.PorPagina);
        resultado.Items.Should().NotBeEmpty();
        resultado.Items.Count.Should().Be(quantidadeItensEsperada);
        resultado.Total.Should().Be(notas.Count());
    }
}
