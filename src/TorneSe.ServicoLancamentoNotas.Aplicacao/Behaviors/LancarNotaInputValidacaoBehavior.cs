using FluentValidation;
using MediatR;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Enums;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.Behaviors;

public class LancarNotaInputValidacaoBehavior
    : IPipelineBehavior<LancarNotaInput, Resultado<NotaOutputModel>>
{
    private readonly IValidator<LancarNotaInput>? _validador;

    public LancarNotaInputValidacaoBehavior(IValidator<LancarNotaInput>? validador) 
        => _validador = validador;

    public async Task<Resultado<NotaOutputModel>> Handle(LancarNotaInput request, RequestHandlerDelegate<Resultado<NotaOutputModel>> next, CancellationToken cancellationToken)
    {
        if (_validador is null)
            return await next();

        var detalhesErros = (await _validador.ValidateAsync(request, cancellationToken))
                            ?.Errors
                            .Select(x => new DetalheErro(x.PropertyName, x.ErrorMessage))
                            .Distinct()
                            .ToList();

        if (detalhesErros is not null && detalhesErros.Any())
            return Resultado<NotaOutputModel>.RetornaResultadoErro(TipoErro.InputNotaInvalido, detalhesErros);

        return await next();
    }
}
