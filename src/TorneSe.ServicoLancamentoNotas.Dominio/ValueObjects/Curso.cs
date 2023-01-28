namespace TorneSe.ServicoLancamentoNotas.Dominio.ValueObjects;

public class Curso
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataTermino { get; set; }
    public ICollection<Atividade> Atividades { get; set; }
    public ICollection<Aluno> Alunos { get; set; }


    public Curso
        (int id,
        string nome,
        bool ativo,
        DateTime dataInicio,
        DateTime dataTermino,
        ICollection<Atividade> atividades,
        ICollection<Aluno> alunos)
    {
        Id = id;
        Nome = nome;
        Ativo = ativo;
        DataInicio = dataInicio;
        DataTermino = dataTermino;
        Atividades = atividades;
        Alunos = alunos;
    }
}
