using Microsoft.Extensions.Logging;
using TorneSe.ServicoLancamentoNotas.Dominio.Clients;
using TorneSe.ServicoLancamentoNotas.Dominio.Mensagens;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.SQS.Base;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.SQS.Contexto.Interface;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Providers.Interfaces;

namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.SQS;

public class NotaCanceladaMensagemClient : SqsClient<NotaCanceladaMensagem>, INotaCanceladaMensagemClient
{
    public const string SQS_NOTA_CANCELADA_QUEUE_NAME = "NOTAS_CANCELADAS_QUEUE";
    public NotaCanceladaMensagemClient(ISqsContexto sqsContexto, ILogger<NotaCanceladaMensagemClient> logger, ITenantProvider tenantProvider) 
        : base(sqsContexto, SQS_NOTA_CANCELADA_QUEUE_NAME, logger, tenantProvider)
    {
    }
}
