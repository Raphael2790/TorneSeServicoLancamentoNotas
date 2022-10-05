namespace TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;

public sealed class DetalheErro
{
    public string Campo { get; private set; }
    public string Mensagem { get; private set; }


    public DetalheErro(string campo, string mensagem)
    {
        Campo = campo;
        Mensagem = mensagem;
    }
}
