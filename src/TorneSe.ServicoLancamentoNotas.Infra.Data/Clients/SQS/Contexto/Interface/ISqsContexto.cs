using Amazon.SQS;

namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.SQS.Contexto.Interface;

public interface ISqsContexto
{
    AmazonSQSClient Sqs { get; }
    int WaitTimeSeconds { get; }
    Task<string> ObterUrlFila(string queueVariable);
}
