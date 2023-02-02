using Amazon;
using Amazon.SQS;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.SQS.Contexto.Interface;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Providers.Interfaces;

namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.SQS.Contexto;

public class SqsContexto : ISqsContexto
{
    private readonly IVariaveisAmbienteProvider _variaveisAmbienteProvider;
    private readonly int _waitTimeSeconds;
    private readonly string _awsAccessKey;
    private readonly string _awsSecretAccessKey;
    private readonly AmazonSQSClient _sqsClient;

    public AmazonSQSClient Sqs => _sqsClient;
    public int WaitTimeSeconds => _waitTimeSeconds;

    public SqsContexto(IVariaveisAmbienteProvider variaveisAmbienteProvider)
    {
        _variaveisAmbienteProvider = variaveisAmbienteProvider;
        _waitTimeSeconds = variaveisAmbienteProvider.WaitTimeSeconds;
        _awsAccessKey = variaveisAmbienteProvider.AwsAccessKey!;
        _awsSecretAccessKey = variaveisAmbienteProvider.AwsSecretAccessKey!;
        _sqsClient = ObterAmazonSqsClient();
    }

    public async Task<string> ObterUrlFila(string queueVariable)
    {
        string queueName = ObterNomeFila(queueVariable)!;
        return (await _sqsClient.GetQueueUrlAsync(queueName)).QueueUrl;
    }

    private AmazonSQSClient ObterAmazonSqsClient()
        => new(_awsAccessKey, _awsSecretAccessKey, RegionEndpoint.USEast1);

    private string? ObterNomeFila(string nomeFila)
        => _variaveisAmbienteProvider.ObterNomeFila(nomeFila);
}
