using System.Text.Json.Serialization;
using TorneSe.ServicoLancamentoNotas.Aplicacao.SerializerContext;

namespace TorneSe.ServicoLancamentoNotas.API.Extensions;

public static class MvcBuilderExtensions
{
    public static void AdicionarSerializerContext(this IMvcBuilder mvcBuilder)
    {
        mvcBuilder.AddJsonOptions(options => {
            options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.AddContext<ApiSerializerContext>();
        });
    }
}
