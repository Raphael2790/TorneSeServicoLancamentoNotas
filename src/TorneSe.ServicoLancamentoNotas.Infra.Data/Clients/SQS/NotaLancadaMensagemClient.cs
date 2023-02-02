using Microsoft.Extensions.Logging;
using TorneSe.ServicoLancamentoNotas.Dominio.Clients;
using TorneSe.ServicoLancamentoNotas.Dominio.Mensagens;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.SQS.Base;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.SQS.Contexto.Interface;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Providers.Interfaces;

namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.SQS;

public class NotaLancadaMensagemClient : SqsClient<NotaLancadaMensagem>, INotaLancadaMensagemClient
{
    const string SQS_NOTA_LANCADA_QUEUE_NAME = "NOTAS_LANCADAS_QUEUE";
    public NotaLancadaMensagemClient(ISqsContexto sqsContexto, ILogger<NotaLancadaMensagemClient> logger, ITenantProvider tenantProvider) 
        : base(sqsContexto, SQS_NOTA_LANCADA_QUEUE_NAME, logger, tenantProvider)
    {
    }
}
