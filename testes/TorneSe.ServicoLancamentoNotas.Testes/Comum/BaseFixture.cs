using Bogus;

namespace TorneSe.ServicoLancamentoNotas.Testes.Comum;

public abstract class BaseFixture
{
    public Faker Faker { get; set; }

    protected BaseFixture()
        => Faker = new("pt_BR");

    public static int RetornaNumeroIdRandomico()
       => new Random().Next(1, 1_000_000);

    public double RetornaValorNotaAleatorioValido()
        => Faker.Random.Double(0.00, 10.00);

    public bool RetornaBoleanoRandomico()
        => new Random().Next(0, 10) > 5;
}
