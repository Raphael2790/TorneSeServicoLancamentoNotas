using Microsoft.Extensions.Logging;
using TorneSe.ServicoLancamentoNotas.Dominio.Clients;
using TorneSe.ServicoLancamentoNotas.Dominio.Mensagens;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.SQS.Base;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.SQS.Contexto.Interface;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Providers.Interfaces;

namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.SQS;

public class NotaAtualizadaMensagemClient : SqsClient<NotaAtualizadaMensagem>, INotaAtualizadaMensagemClient
{
    public const string SQS_NOTA_ATUALIZADA_QUEUE_NAME = "NOTAS_ATUALIZADAS_QUEUE";
    public NotaAtualizadaMensagemClient(ISqsContexto sqsContexto, ILogger<NotaAtualizadaMensagemClient> logger, ITenantProvider tenantProvider) 
        : base(sqsContexto, SQS_NOTA_ATUALIZADA_QUEUE_NAME , logger, tenantProvider)
    {
    }
}
