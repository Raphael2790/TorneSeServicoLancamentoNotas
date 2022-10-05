using FluentAssertions;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Enums;

namespace TorneSe.ServicoLancamentoNotas.Testes.Aplicacao.Comum;

public class ResultadoTests
{
    [Fact(DisplayName = nameof(RetornaSucesso_DeveSetarSucesso))]
    [Trait("Aplicacao", "Resultado - Comum")]
    public void RetornaSucesso_DeveSetarSucesso()
    {
        var dto = new ResultadoDTO("Nome", "Descrição");

        var resultado = Resultado<ResultadoDTO>.RetornaResultadoSucesso(dto);

        resultado.Sucesso.Should().BeTrue();
        resultado.Dado.Should().NotBeNull();
        resultado.Dado.Should().BeAssignableTo<ResultadoDTO>();
        resultado.Erro.Should().BeNull();
        resultado.DescricaoErro.Should().BeNull();
        resultado.DetalhesErros.Should().BeNull();
    }

    [Fact(DisplayName = nameof(RetornaResultadoErro_DeveSetarErro))]
    [Trait("Aplicacao", "Resultado - Comum")]
    public void RetornaResultadoErro_DeveSetarErro()
    {
        var resultado = Resultado<ResultadoDTO>.RetornaResultadoErro(TipoErro.NotaNaoEncontrada);

        resultado.Sucesso.Should().BeFalse();
        resultado.Dado.Should().BeNull();
        resultado.Erro.Should().Be(TipoErro.NotaNaoEncontrada);
        resultado.DescricaoErro.Should().NotBeNull();
        resultado.DetalhesErros.Should().BeNull();
    }

    [Fact(DisplayName = nameof(RetornaResultadoErro_DeveSetarErro))]
    [Trait("Aplicacao", "Resultado - Comum")]
    public void RetornaResultadoErro_DeveSetarErroEDetalhesErro()
    {
        var detalhesErro = new List<DetalheErro> { new("Nome","Campo inválido") };

        var resultado = Resultado<ResultadoDTO>.RetornaResultadoErro(TipoErro.NotaInvalida, detalhesErro);

        resultado.Sucesso.Should().BeFalse();
        resultado.Dado.Should().BeNull();
        resultado.Erro.Should().Be(TipoErro.NotaInvalida);
        resultado.DescricaoErro.Should().NotBeNull();
        resultado.DetalhesErros.Should().NotBeEmpty();
        resultado.DetalhesErros.Should().HaveCount(1);
    }
}

public class ResultadoDTO
{
    public string Nome { get; set; }
    public string Descricao { get; set; }


    public ResultadoDTO(string nome, string descricao)
    {
        Nome = nome;
        Descricao = descricao;
    }
}
