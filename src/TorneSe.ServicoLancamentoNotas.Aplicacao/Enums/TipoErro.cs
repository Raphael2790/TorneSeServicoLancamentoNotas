using System.ComponentModel;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Contantes;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.Enums;

public enum TipoErro
{
    [Description(ConstantesAplicacao.MensagensErro.NOTA_NAO_ENCONTRADA)]
    NotaNaoEncontrada = 100,
    [Description(ConstantesAplicacao.MensagensErro.NOTA_INVALIDA)]
    NotaInvalida = 101,

    [Description(ConstantesAplicacao.MensagensErro.ERRO_INESPERADO)]
    ErroInesperado = 500
}
