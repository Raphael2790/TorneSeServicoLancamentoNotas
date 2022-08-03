using Bogus;

namespace TorneSe.ServicoLancamentoNotas.Testes.Comum;

public abstract class BaseFixture
{
    public Faker Faker { get; set; }

    protected BaseFixture()
        => Faker = new("pt_BR");
}
