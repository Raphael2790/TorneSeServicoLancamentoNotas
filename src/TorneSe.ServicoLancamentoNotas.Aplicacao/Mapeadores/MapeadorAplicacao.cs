using System;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.Params;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.Mapeadores;

public static class MapeadorAplicacao
{
    public static Nota LancarNotaInputEmNota(LancarNotaInput input)
    {
        var notaParams = new NotaParams(input.AlunoId, input.AtividadeId, input.ValorNota, DateTime.Now);

        return new(notaParams);
    }

    public static NotaOutputModel NotaEmNotaOuputModel(Nota nota)
        => new(nota.AlunoId, nota.AtividadeId, nota.ValorNota, nota.DataLancamento, nota.Cancelada, nota.MotivoCancelamento!,
            nota.StatusIntegracao);
}
