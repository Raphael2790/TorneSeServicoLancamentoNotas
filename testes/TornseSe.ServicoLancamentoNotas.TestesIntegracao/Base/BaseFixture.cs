using Bogus;
using Microsoft.EntityFrameworkCore;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.Params;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Contexto;

namespace TornseSe.ServicoLancamentoNotas.TestesIntegracao.Base;

public class BaseFixture
{
    protected Faker Faker { get; } = new("pt_BR");

    public static int RetornaNumeroIdRandomico()
      => new Random().Next(1, 1_000_000);

    public double RetornaValorNotaAleatorioValido()
        => Faker.Random.Double(0.00, 10.00);

    public bool RetornaBoleanoRandomico()
        => new Random().Next(0, 10) > 5;

    public NotaParams RetornaValoresParametrosNotaValidos()
        => new(RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(),
                RetornaValorNotaAleatorioValido(), DateTime.Now);

    public Nota RetornaNota()
        => new(RetornaValoresParametrosNotaValidos());

    public ServicoLancamentoNotaDbContext CriarDbContext()
    {
        var dbContext = new ServicoLancamentoNotaDbContext
            (
                new DbContextOptionsBuilder<ServicoLancamentoNotaDbContext>()
                    .UseInMemoryDatabase("integration-tests")
                    .Options
            );

        return dbContext;
    }
}
