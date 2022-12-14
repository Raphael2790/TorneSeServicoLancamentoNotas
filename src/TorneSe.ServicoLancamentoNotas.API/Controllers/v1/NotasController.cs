using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TorneSe.ServicoLancamentoNotas.API.Controllers.Base;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Interfaces;

namespace TorneSe.ServicoLancamentoNotas.API.Controllers.v1;

[ApiVersion("1")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[Route("notas/v1")]
public class NotasController : MainController
{
    private readonly ILogger<NotasController> _logger;
    private readonly IMediatorHandler _handler;

    public NotasController(ILogger<NotasController> logger, IMediatorHandler handler)
    {
        _logger = logger;
        _handler = handler;
    }

    [HttpPost("lancar")]
    [ProducesResponseType(typeof(Resultado<NotaOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Resultado<NotaOutputModel>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Lancar([FromBody] LancarNotaInput input, CancellationToken cancellationToken)
    {
        var response = await _handler.EnviarRequest<Resultado<NotaOutputModel>, LancarNotaInput>(input, cancellationToken);

        _logger.LogInformation("Efetuado lançamento de nota do aluno {alunoId} para a atividade {atividadeId}. {@response}",
            input.AlunoId, input.AtividadeId, response);

       return RespostaCustomizada(response);
    }

    [HttpPatch("atualizar")]
    [ProducesResponseType(typeof(Resultado<NotaOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Resultado<NotaOutputModel>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Atualizar([FromBody] AtualizarNotaInput input, CancellationToken cancellationToken)
    {
        var response = await _handler.EnviarRequest<Resultado<NotaOutputModel>, AtualizarNotaInput>(input, cancellationToken);

        _logger.LogInformation("Efetuado atualização de nota do aluno {alunoId} para a atividade {atividadeId}. {@response}",
            input.AlunoId, input.AtividadeId, response);

        return RespostaCustomizada(response);
    }

    [HttpPatch("cancelar")]
    [ProducesResponseType(typeof(Resultado<NotaOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Resultado<NotaOutputModel>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Cancelar([FromBody] CancelarNotaInput input, CancellationToken cancellationToken)
    {
        var response = await _handler.EnviarRequest<Resultado<NotaOutputModel>, CancelarNotaInput>(input, cancellationToken);

        _logger.LogInformation("Efetuado cancelamento de nota do aluno {alunoId} para a atividade {atividadeId}. {@response}",
           input.AlunoId, input.AtividadeId, response);

        return RespostaCustomizada(response);
    }

    [HttpGet("buscar/{origem}")]
    [ProducesResponseType(typeof(Resultado<ListaNotaOutput>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Buscar([FromQuery] ListaNotaInput input, CancellationToken cancellationToken)
    {
        var response = await _handler.EnviarRequest<Resultado<ListaNotaOutput>, ListaNotaInput>(input, cancellationToken);

        return RespostaCustomizada(response);
    }

}
