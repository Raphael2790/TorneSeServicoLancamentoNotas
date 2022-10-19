using Microsoft.EntityFrameworkCore;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.Enums;
using TorneSe.ServicoLancamentoNotas.Dominio.Repositories;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork.BuscaRepository;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Contexto;

namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Repositories;

internal class NotaRepository : INotaRepository
{
    private readonly DbSet<Nota> _contextoNota;

    public NotaRepository(ServicoLancamentoNotaDbContext context)
        => _contextoNota = context.Notas;

    public async Task Atualizar(Nota agregado, CancellationToken _)
        => await Task.FromResult(_contextoNota.Update(agregado));

    public async Task<BuscaOutput<Nota>> Buscar(BuscaInput input, CancellationToken cancellationToken)
    {
        var toSkip = (input.Pagina - 1) * input.PorPagina;
        var query = _contextoNota.AsNoTracking();

        if (input.AtividadeId.HasValue)
            query = query.Where(x => x.AtividadeId == input.AtividadeId.Value);
        if (input.AlunoId.HasValue)
            query = query.Where(x => x.AlunoId == input.AlunoId.Value);

        query = OrdenarResultado(query, input.OrdenarPor, input.Ordenacao);

        var items = await query.Skip(toSkip).Take(input.PorPagina).ToListAsync(cancellationToken);
        var total = await query.CountAsync(cancellationToken);
        return new BuscaOutput<Nota>(input.Pagina, input.PorPagina, total, items);
    }

    private static IQueryable<Nota> OrdenarResultado(IQueryable<Nota> query, string ordenarPor, OrdenacaoBusca ordenacao)
        => (ordenacao, ordenarPor.ToLower()) switch
        {
            (OrdenacaoBusca.Asc, "atividadeid") => query.OrderBy(x => x.AtividadeId),
            (OrdenacaoBusca.Desc, "atividadeid") => query.OrderByDescending(x => x.AtividadeId),
            (OrdenacaoBusca.Asc, "alunoid") => query.OrderBy(x => x.AlunoId),
            (OrdenacaoBusca.Desc, "alunoid") => query.OrderByDescending(x => x.AlunoId),
            _ => query.OrderBy(x => x.AlunoId)
        };

    public async Task<Nota?> BuscarNotaPorAlunoEAtividade(int alunoId, int atividadeId, CancellationToken cancellationToken)
        => await _contextoNota.FirstOrDefaultAsync(x => x.AlunoId == alunoId && x.AtividadeId == atividadeId, cancellationToken);

    public async Task Inserir(Nota agregado, CancellationToken cancellationToken)
        => await _contextoNota.AddAsync(agregado, cancellationToken);
}
