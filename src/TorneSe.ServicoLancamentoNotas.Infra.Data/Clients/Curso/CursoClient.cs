using Microsoft.Extensions.Logging;
using ValueObjects = TorneSe.ServicoLancamentoNotas.Dominio.ValueObjects;
using TorneSe.ServicoLancamentoNotas.Dominio.Clients;
using System.Text.Json;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Providers.Interfaces;
using TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.SerializerContext;

namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.Curso;

public class CursoClient : ICursoClient
{
	private readonly HttpClient _client;
	private readonly ILogger<CursoClient> _logger;
    private readonly IVariaveisAmbienteProvider _variaveisAmbienteProvider;
    private readonly CursoSerializerContext _serializerContext;

    public CursoClient(
        HttpClient client, 
        ILogger<CursoClient> logger, 
        IVariaveisAmbienteProvider variaveisAmbienteProvider,
        CursoSerializerContext serializerContext
        )
    {
        _client = client;
        _logger = logger;
        _variaveisAmbienteProvider = variaveisAmbienteProvider;
        _serializerContext = serializerContext;
    }

    public async Task<IEnumerable<ValueObjects.Curso>>? ObterInformacoesCursoAluno(int alunoId, int professorId, 
        int atividadeId, CancellationToken cancellationToken)
    {
        var queryString = $"?alunoId={alunoId}&professorId={professorId}&atividadeId={atividadeId}";
        var request = new HttpRequestMessage(HttpMethod.Get, 
            $"{_variaveisAmbienteProvider.UrlBaseCursos}{_variaveisAmbienteProvider.PathObterCursos}{queryString}");

        var resultado = await _client.SendAsync(request, cancellationToken);

        if (!resultado.IsSuccessStatusCode)
        {
            _logger
                .LogError($"Ocorreu um erro ao consultar o curso do aluno {alunoId}. Informações resposta {{@resultado}}", 
                resultado);
            return default;
        }

        return JsonSerializer
            .Deserialize
            (await resultado.Content.ReadAsStreamAsync(cancellationToken), _serializerContext.IEnumerableCurso);
    }
}
