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
}
