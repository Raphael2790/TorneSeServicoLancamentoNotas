namespace TorneSe.ServicoLancamentoNotas.Dominio.ValueObjects;

public class Atividade
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public bool Ativo { get; set;}
    public DateTime DataInicio { get; set; }
    public DateTime DataTermino { get; set; }
    public Professor Professor { get; set; }

    public Atividade(int id, string nome, bool ativo, DateTime dataInicio, DateTime dataTermino, Professor professor)
    {
        Id = id;
        Nome = nome;
        Ativo = ativo;
        DataInicio = dataInicio;
        DataTermino = dataTermino;
        Professor = professor;
    }
}
