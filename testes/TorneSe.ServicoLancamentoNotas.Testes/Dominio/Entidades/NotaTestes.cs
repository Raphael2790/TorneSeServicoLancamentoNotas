using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.Enums;

namespace TorneSe.ServicoLancamentoNotas.Testes.Dominio.Entidades;

public class NotaTestes
{
    [Fact(DisplayName = nameof(InstanciarNota))]
    [Trait("Dominio", "Nota - Agregado")]
    public void InstanciarNota()
    {
        //Arrange
        var valoresEntrada = new 
        { 
            AlunoId = 1234, 
            AtividadeId = 34566, 
            ValorNota = 10.00, 
            DataLancamento = DateTime.Now,
            UsuarioId = 34566
        };

        //Act
        var nota = new Nota(valoresEntrada.AlunoId, valoresEntrada.AtividadeId, 
                        valoresEntrada.ValorNota, valoresEntrada.DataLancamento, valoresEntrada.UsuarioId);

        //Assert
        Assert.NotNull(nota);
        Assert.Equal(valoresEntrada.AlunoId, nota.AlunoId);
        Assert.Equal(valoresEntrada.AtividadeId, nota.AtividadeId);
        Assert.Equal(valoresEntrada.ValorNota, nota.ValorNota);
        Assert.Equal(valoresEntrada.DataLancamento, nota.DataLancamento);
        Assert.NotEqual(default, nota.DataLancamento);
        Assert.Equal(valoresEntrada.UsuarioId, nota.UsuarioId);
        Assert.False(nota.CanceladaPorRetentativa);
        Assert.Equal(StatusIntegracao.AguardandoIntegracao, nota.StatusIntegracao);
        Assert.Null(nota.MotivoCancelamento);
    }

    //[Fact(DisplayName = "")]
    //[Trait("Dominio", "Nota - Agregado")]

    //Preciso controlar se a nota lançada já foi integrada
    //Caso uma nota venha a ser cancelada preciso de um motivo para o cancelamento
}
