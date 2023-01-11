namespace TorneSe.ServicoLancamentoNotas.Dominio.ValueObjects;

public class Atividade
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public bool Ativo { get; set;}
    public DateTime DataInicio { get; set; }
    public DateTime DataTermino { get; set; }
    public Professor Professor { get; set; }
}
