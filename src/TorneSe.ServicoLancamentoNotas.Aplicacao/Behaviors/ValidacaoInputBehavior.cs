using FluentValidation;
using MediatR;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Enums;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.Behaviors;

public class ValidacaoInputBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Resultado
{
    private readonly IValidator<TRequest>? _validador;

    public ValidacaoInputBehavior(IValidator<TRequest>? validador) 
        => _validador = validador;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validador is null)
            return await next();

        var detalhesErros = (await _validador.ValidateAsync(request, cancellationToken))
                            ?.Errors
                            .Select(x => new DetalheErro(x.PropertyName, x.ErrorMessage))
                            .Distinct()
                            .ToList();

        if (detalhesErros is null || !detalhesErros.Any())
            return await next();

        var resultado = typeof(Resultado<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResponse).GenericTypeArguments[0])
            .GetMethod(nameof(Resultado<TRequest>.RetornaResultadoErro), new[] { typeof(TipoErro), typeof(List<DetalheErro>) })!
            .Invoke(null, new object?[] {TipoErro.InputNotaInvalido, detalhesErros});

        return (TResponse)resultado!;
    }
}
