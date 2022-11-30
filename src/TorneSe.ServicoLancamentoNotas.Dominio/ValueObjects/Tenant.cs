namespace TorneSe.ServicoLancamentoNotas.Dominio.ValueObjects;

public class Tenant
{
    public string Origem { get; private set; }

    public Tenant(string origem)
    {
        Origem = origem ?? string.Empty;
    }

    public Tenant()
    {

    }

    public void AtribuirOrigem(string origem)
    {
        Origem = origem ?? string.Empty;
    }

    public override string ToString()
    {
        return Origem ?? string.Empty;
    }

    public static implicit operator Tenant(string origem)
        => new(origem);

    public static implicit operator string(Tenant origem)
    {
        return origem?.Origem ?? "";
    }
}
