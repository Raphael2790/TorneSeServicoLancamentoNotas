namespace TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;

public abstract class Mensagem
{
    protected DateTime DataMensagemCriada { get; }

    protected Mensagem() 
    {
        DataMensagemCriada = DateTime.Now;
    }
}
