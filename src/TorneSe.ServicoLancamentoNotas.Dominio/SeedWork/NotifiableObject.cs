namespace TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;

public abstract class NotifiableObject
{
    public bool EhValida { get; protected set; }
    public List<Notificacao> Notificacoes { get; set; } = new();

    public void Notificar(Notificacao notificacao)
        => Notificacoes.Add(notificacao);
}
