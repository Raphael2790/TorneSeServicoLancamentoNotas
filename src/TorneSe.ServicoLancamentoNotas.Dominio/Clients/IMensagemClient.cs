using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;

namespace TorneSe.ServicoLancamentoNotas.Dominio.Clients;

public interface IMensagemClient<TMensagem>
    where TMensagem: Mensagem
{
    Task EnviarMensagem(TMensagem message);
    Task EnviarMensagem(List<TMensagem> mensagens);
}
