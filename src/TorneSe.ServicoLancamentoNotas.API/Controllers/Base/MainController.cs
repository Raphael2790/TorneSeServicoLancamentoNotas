using Microsoft.AspNetCore.Mvc;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;

namespace TorneSe.ServicoLancamentoNotas.API.Controllers.Base;

[ApiController]
public abstract class MainController : ControllerBase
{
    protected ActionResult RespostaCustomizada<T>(Resultado<T> resultado)
    {
        if(resultado.Sucesso)
            return Ok(resultado);

        if (resultado.Erro is Aplicacao.Enums.TipoErro.RecursoNaoEncontrado)
            return NotFound(resultado);

        return BadRequest(resultado);
    }
}
