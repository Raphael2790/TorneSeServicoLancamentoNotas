using System.Text.Json.Serialization;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.SerializerContext;

[JsonSerializable(typeof(Resultado<NotaOutputModel>))]
[JsonSerializable(typeof(Resultado<ListaNotaOutput>))]
[JsonSerializable(typeof(CancelarNotaInput))]
[JsonSerializable(typeof(ListaNotaInput))]
[JsonSerializable(typeof(AtualizarNotaInput))]
[JsonSerializable(typeof(LancarNotaInput))]
public partial class ApiSerializerContext : JsonSerializerContext
{
}
