using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;

namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Mapeamentos;

public class NotaConfiguration : IEntityTypeConfiguration<Nota>
{
    public void Configure(EntityTypeBuilder<Nota> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.MotivoCancelamento)
            .HasMaxLength(500)
            .IsUnicode(false);

        builder.Property(x => x.ValorNota)
            .HasPrecision(2, 2);

        builder.Property(x => x.AlunoId)
            .HasPrecision(6);

        builder.Property(x => x.AtividadeId)
            .HasPrecision(6);

        builder.Property(x => x.StatusIntegracao)
            .HasConversion<string>();

        builder.Ignore(x => x.Notificacoes);

        builder.Ignore(x => x.EhValida);
    }
}
