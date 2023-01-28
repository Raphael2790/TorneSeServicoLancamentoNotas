namespace TorneSe.ServicoLancamentoNotas.Dominio.ValueObjects;

public class Professor
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public bool Ativo { get; set;}

    public Professor(int id, string nome, bool ativo)
    {
        Id = id;
        Nome = nome;
        Ativo = ativo;
    }
}
