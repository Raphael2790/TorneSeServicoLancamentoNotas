using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;

namespace TorneSe.ServicoLancamentoNotas.API.Filters;

public class ApiGlobalExceptionFilter : IExceptionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public ApiGlobalExceptionFilter(IServiceProvider serviceProvider) 
        => _serviceProvider = serviceProvider;

    public void OnException(ExceptionContext context)
    {
        var resultado = Resultado<object>
            .RetornaResultadoErro(Aplicacao.Enums.TipoErro.ErroInesperado);

        _serviceProvider
            .GetService<ILogger<ApiGlobalExceptionFilter>>()?
            .LogError("Ocorreu um erro inesperado.{@Exception}", context.Exception);

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(resultado);
        context.ExceptionHandled = true;
    }
}
