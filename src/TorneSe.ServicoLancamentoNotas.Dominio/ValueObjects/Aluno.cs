namespace TorneSe.ServicoLancamentoNotas.Dominio.ValueObjects;

public class Aluno
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public bool Ativo { get; set;}

    public Aluno(int id, string nome, bool ativo)
    {
        Id = id;
        Nome = nome;
        Ativo = ativo;
    }
}
