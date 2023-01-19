using System.Text.Json.Serialization;
using ValueObjects = TorneSe.ServicoLancamentoNotas.Dominio.ValueObjects;

namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Clients.SerializerContext;

[JsonSerializable(typeof(IEnumerable<ValueObjects.Curso>))]
[JsonSerializable(typeof(ValueObjects.Curso))]
[JsonSerializable(typeof(ValueObjects.Atividade))]
[JsonSerializable(typeof(ValueObjects.Aluno))]
[JsonSerializable(typeof(ValueObjects.Professor))]
public partial class CursoSerializerContext : JsonSerializerContext
{
}
