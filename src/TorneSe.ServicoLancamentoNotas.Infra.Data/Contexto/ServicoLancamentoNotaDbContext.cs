using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;

namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Contexto;

public class ServicoLancamentoNotaDbContext : DbContext
{
    public DbSet<Nota> Notas => Set<Nota>();

    public ServicoLancamentoNotaDbContext(DbContextOptions<ServicoLancamentoNotaDbContext> options)
        : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
