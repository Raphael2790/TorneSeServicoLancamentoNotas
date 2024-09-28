using Amazon;
using Amazon.Runtime;
using Microsoft.Extensions.DependencyInjection;
using TorneSe.ServicoLancamentoNotas.Dominio.ValueObjects;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.SQS;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Providers;

namespace TorneSe.ServicoLancamentoNotas.Infra.HealthCheck;

public static class HealthCheckConfiguracao
{
    public static IServiceCollection AdicionarConfiguracoesHealthChecks(this IServiceCollection services)
    {
        //var variaveisProvider = VariaveisAmbienteProvider.Instance;

        //var builder = services.AddHealthChecks()
        //    .AddSqs(options =>
        //    {

        //        options.RegionEndpoint = RegionEndpoint.USEast1;
        //        options.Credentials = new BasicAWSCredentials(variaveisProvider.AwsAccessKey, variaveisProvider.AwsSecretAccessKey);
        //        options
        //            .AddQueue(variaveisProvider.ObterNomeFila(NotaAtualizadaMensagemClient.SQS_NOTA_ATUALIZADA_QUEUE_NAME)!);
        //        options
        //            .AddQueue(variaveisProvider.ObterNomeFila(NotaCanceladaMensagemClient.SQS_NOTA_CANCELADA_QUEUE_NAME)!);
        //        options
        //            .AddQueue(variaveisProvider.ObterNomeFila(NotaLancadaMensagemClient.SQS_NOTA_LANCADA_QUEUE_NAME)!);
        //    }, name: "AWS SQS", tags: new[] { "filas", "mensagens"});

        //foreach (var tenant in variaveisProvider.Tenants)
        //    builder.AddMySql(variaveisProvider.ObterConnectionStringPorTenant(new Tenant(tenant))!, 
        //        name: $"MySql-{tenant}", tags: new[] { "data", "db" });

        //builder
        //    .AddUrlGroup(new Uri($"{variaveisProvider.UrlBaseCursos}{variaveisProvider.PathObterCursos}?alunoId=1&professorId=1&atividadeId=1"), 
        //    name: "Api Cursos", tags: new[] { "api", "cursos"});
        
        return services;
    }
}
