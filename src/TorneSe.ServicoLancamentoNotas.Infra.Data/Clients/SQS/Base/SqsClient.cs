using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TorneSe.ServicoLancamentoNotas.Dominio.Clients;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.SQS.Contexto.Interface;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Providers.Interfaces;

namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.SQS.Base;

public abstract class SqsClient<TMensagem> : IMensagemClient<TMensagem>
    where TMensagem : Mensagem
{
    private readonly ISqsContexto _sqsContexto;
    private readonly string _nomeFila;
    private readonly ILogger _logger;
    private readonly ITenantProvider _tenantProvider;

    protected SqsClient(ISqsContexto sqsContexto, string nomeFila, ILogger logger, ITenantProvider tenantProvider)
    {
        _sqsContexto = sqsContexto;
        _nomeFila = nomeFila;
        _logger = logger;
        _tenantProvider = tenantProvider;
    }

    public virtual async Task EnviarMensagem(TMensagem message)
    {
        try
        {
            var queueUrl = await _sqsContexto.ObterUrlFila(_nomeFila);
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = JsonSerializer.Serialize(message),
                MessageAttributes = new Dictionary<string, MessageAttributeValue>
                {
                    {"Tenant", new MessageAttributeValue
                    {
                        StringValue = _tenantProvider.Tenant,
                        DataType = "String"
                    }}
                }
            };

            await _sqsContexto.Sqs.SendMessageAsync(sendMessageRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public virtual async Task EnviarMensagem(List<TMensagem> mensagens)
    {
        try
        {
            var queueUrl = await _sqsContexto.ObterUrlFila(_nomeFila);
            var messageBatchList = new List<SendMessageBatchRequestEntry>();
            foreach (var message in mensagens)
            {
                messageBatchList.Add(new SendMessageBatchRequestEntry()
                {
                    Id = message.GetHashCode().ToString(),
                    MessageBody = JsonSerializer.Serialize(message),
                    MessageAttributes = new Dictionary<string, MessageAttributeValue>
                    {
                        {
                            "Tenant", new MessageAttributeValue
                            {
                                StringValue = _tenantProvider.Tenant,
                                DataType = "String"
                            }
                        }
                    }
                });
            }

            var sendMessageBatchRequest = new SendMessageBatchRequest
            {
                QueueUrl = queueUrl,
                Entries = messageBatchList
            };

            await _sqsContexto.Sqs.SendMessageBatchAsync(sendMessageBatchRequest);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
