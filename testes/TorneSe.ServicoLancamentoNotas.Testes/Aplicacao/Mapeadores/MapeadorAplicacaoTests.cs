using FluentAssertions;
using TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Mapeadores;
using TorneSe.ServicoLancamentoNotas.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Dominio.Enums;

namespace TorneSe.ServicoLancamentoNotas.Testes.Aplicacao.Mapeadores;

[Collection(nameof(MapeadorAplicacaoFixture))]
public class MapeadorAplicacaoTests
{
    private readonly MapeadorAplicacaoFixture _fixture;

    public MapeadorAplicacaoTests(MapeadorAplicacaoFixture fixture) 
        => _fixture = fixture;

    [Fact(DisplayName = nameof(LancarNotaInputEmNota_QuandoConvertido_DeveRetornarNota))]
    [Trait("Aplicacao", "Mapeadores - LancarNotaInput")]
    public void LancarNotaInputEmNota_QuandoConvertido_DeveRetornarNota()
    {
        //Assert
        var input = _fixture.DevolveNotaInputValido();

        //Act
        var nota = MapeadorAplicacao.LancarNotaInputEmNota(input);

        //Arrange
        nota.Should().BeOfType<Nota>();
        nota.ValorNota.Should().Be(input.ValorNota);
        nota.AlunoId.Should().Be(input.AlunoId);
        nota.AtividadeId.Should().Be(input.AtividadeId);
        nota.StatusIntegracao.Should().Be(StatusIntegracao.AguardandoIntegracao);
    }

    [Fact(DisplayName = nameof(NotaEmNotaOuputModel_QuandoConvertido_DeveRetornarNota))]
    [Trait("Aplicacao", "Mapeadores - NotaOuputModel")]
    public void NotaEmNotaOuputModel_QuandoConvertido_DeveRetornarNota()
    {
        //Assert
        var nota = _fixture.RetornaNotaValida();

        //Act
        var outputModel = MapeadorAplicacao.NotaEmNotaOuputModel(nota);

        //Arrange
        outputModel.Should().BeOfType<NotaOutputModel>();
        outputModel.ValorNota.Should().Be(nota.ValorNota);
        outputModel.AlunoId.Should().Be(nota.AlunoId);
        outputModel.AtividadeId.Should().Be(nota.AtividadeId);
        outputModel.StatusIntegracao.Should().Be(nota.StatusIntegracao);
        outputModel.DataLancamento.Should().Be(nota.DataLancamento);
        outputModel.Cancelada.Should().Be(nota.Cancelada);
        outputModel.MotivoCancelamento.Should().Be(nota.MotivoCancelamento);
    }
}
