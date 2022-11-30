using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Contantes;

namespace TorneSe.ServicoLancamentoNotas.API.Configurations.Swagger;

public class HeaderObrigatorioFilter : IOperationFilter
{
    private const string BUSCA = "/buscar/{origem}";

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var endpoint = context?.ApiDescription?.RelativePath;

        if (!string.IsNullOrEmpty(endpoint) && !endpoint.EndsWith(BUSCA))
        {
            operation.Parameters ??= new List<OpenApiParameter>();
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "origem",
                In = ParameterLocation.Header,
                Description = "Origem da requisição",
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Enum = new List<IOpenApiAny>
                    {
                        new OpenApiString(ConstantesAplicacao.TenantsAplicacao.TORNE_SE_CSHARP),
                        new OpenApiString(ConstantesAplicacao.TenantsAplicacao.TORNE_SE_JAVASCRIPT),
                        new OpenApiString(ConstantesAplicacao.TenantsAplicacao.TORNE_SE_JAVA)
                    }
                }
            });
        }
        else
        {
            var parameter = operation.Parameters?.FirstOrDefault(p => p.Name.Equals("bandeira"));

            if (parameter is not null)
            {
                parameter.Description = "Origem da requisição";
                parameter.Schema = new OpenApiSchema
                {
                    Type = "string",
                    Enum = new List<IOpenApiAny>
                    {
                        new OpenApiString(ConstantesAplicacao.TenantsAplicacao.TORNE_SE_CSHARP),
                        new OpenApiString(ConstantesAplicacao.TenantsAplicacao.TORNE_SE_JAVASCRIPT),
                        new OpenApiString(ConstantesAplicacao.TenantsAplicacao.TORNE_SE_JAVA)
                    }
                };
            }
        }
    }
}
