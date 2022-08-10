using FluentAssertions;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;
using TorneSe.ServicoLancamentoNotas.Dominio.Validacoes;
using TorneSe.ServicoLancamentoNotas.Testes.Dominio.Entidades;
using TorneSe.ServicoLancamentoNotas.Testes.Fakes;

namespace TorneSe.ServicoLancamentoNotas.Testes.Validacoes;

[Collection(nameof(NotaTestesFixture))]
public class ValidacoesDominioTestes
{
    private readonly NotaTestesFixture _fixture;
    public ValidacoesDominioTestes(NotaTestesFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(DeveEstarEntre_QuandoValorEstaForaDoIntervalo_DeveNotificarObjeto))]
    [Trait("Dominio", "ValidacoesDominio - Validacoes")]
    [InlineData(-1)]
    [InlineData(11)]
    public void DeveEstarEntre_QuandoValorEstaForaDoIntervalo_DeveNotificarObjeto(double valor)
    {
        //Arrange
        var valorInicialIntervalo = default(double);
        var valorFinalIntervalo = 10;
        var mensagem = "O valor está fora do intervalo";
        NotifiableObject objetoNotificavel = new NotaFake();
        var nomeCampo = "ValorNota";

        //Act
        ValidacoesDominio.DeveEstarEntre(valor, valorInicialIntervalo, valorFinalIntervalo, objetoNotificavel, nomeCampo, mensagem);

        //Assert
        objetoNotificavel.Notificacoes.Should().NotBeEmpty();
        objetoNotificavel.Notificacoes.Should().HaveCount(1);
        objetoNotificavel.Notificacoes.First().Campo.Should().Be(nomeCampo);
        objetoNotificavel.Notificacoes.First().Mensagem.Should().Be(mensagem);
    }

    [Theory(DisplayName = nameof(DeveEstarEntre_QuandoValorEstaNoIntervalo_NaoDeveNotificarObjeto))]
    [Trait("Dominio", "ValidacoesDominio - Validacoes")]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(10)]
    public void DeveEstarEntre_QuandoValorEstaNoIntervalo_NaoDeveNotificarObjeto(double valor)
    {
        //Arrange
        var valorInicialIntervalo = default(double);
        var valorFinalIntervalo = 10;
        NotifiableObject objetoNotificavel = new NotaFake();

        //Act
        ValidacoesDominio.DeveEstarEntre(valor, valorInicialIntervalo, valorFinalIntervalo, objetoNotificavel, null!, null!);

        //Assert
        objetoNotificavel.Notificacoes.Should().BeEmpty();
        objetoNotificavel.Notificacoes.Should().HaveCount(default(int));
    }

    [Theory(DisplayName = nameof(MaiorQue_QuandoValorMenorOuIgual_DeveNotificarObjeto))]
    [Trait("Dominio", "ValidacoesDominio - Validacoes")]
    [InlineData(-1)]
    [InlineData(0)]
    public void MaiorQue_QuandoValorMenorOuIgual_DeveNotificarObjeto(int valor)
    {
        //Arrange
        var valorMinimo = default(int);
        var mensagem = "O usuario está inválido";
        NotifiableObject objetoNotificavel = new NotaFake();
        var nomeCampo = "UsuarioId";

        //Act
        ValidacoesDominio.MaiorQue(valor, valorMinimo, objetoNotificavel, nomeCampo, mensagem);

        //Assert
        objetoNotificavel.Notificacoes.Should().NotBeEmpty();
        objetoNotificavel.Notificacoes.Should().HaveCount(1);
        objetoNotificavel.Notificacoes.First().Campo.Should().Be(nomeCampo);
        objetoNotificavel.Notificacoes.First().Mensagem.Should().Be(mensagem);
    }

    [Theory(DisplayName = nameof(MaiorQue_QuandoValorMenorOuIgual_DeveNotificarObjeto))]
    [Trait("Dominio", "ValidacoesDominio - Validacoes")]
    [InlineData(10)]
    [InlineData(9917817)]
    public void MaiorQue_QuandoValorMaior_NaoDeveNotificarObjeto(int valor)
    {
        //Arrange
        var valorMinimo = default(int);
        var mensagem = "O usuario está inválido";
        NotifiableObject objetoNotificavel = new NotaFake();
        var nomeCampo = "UsuarioId";

        //Act
        ValidacoesDominio.MaiorQue(valor, valorMinimo, objetoNotificavel, nomeCampo, mensagem);

        //Assert
        objetoNotificavel.Notificacoes.Should().BeEmpty();
        objetoNotificavel.Notificacoes.Should().HaveCount(default(int));
    }

    [Fact(DisplayName = "")]
    [Trait("Dominio", "ValidacoesDominio - Validacoes")]
    public void NumeroMaximoCaracteres_QuandoValorExcedeQuantidadeCaracteres_DeveNotificarObjeto()
    {
        //Arrange
        var texto = _fixture.Faker.Lorem.Text();
        while (texto.Length <= 500)
            texto += _fixture.Faker.Lorem.Text();
        var numeroMaximoCaracteres = 500;
        var mensagem = "A quantidade de caracteres foi excedida";
        NotifiableObject objetoNotificavel = new NotaFake();
        var nomeCampo = "MotivoCancelamento";

        //Act
        ValidacoesDominio.NumeroMaximoCaracteres(texto,numeroMaximoCaracteres,objetoNotificavel,nomeCampo,mensagem);

        objetoNotificavel.Notificacoes.Should().NotBeEmpty();
        objetoNotificavel.Notificacoes.Should().HaveCount(1);
        objetoNotificavel.Notificacoes.First().Campo.Should().Be(nomeCampo);
        objetoNotificavel.Notificacoes.First().Mensagem.Should().Be(mensagem);
    }

    [Fact(DisplayName = "")]
    [Trait("Dominio", "ValidacoesDominio - Validacoes")]
    public void NumeroMaximoCaracteres_QuandoValorNaoExcedeQuantidadeCaracteres_NaoDeveNotificarObjeto()
    {
        //Arrange
        var texto = _fixture.Faker.Lorem.Text();
        var numeroMaximoCaracteres = 500;
        var mensagem = "A quantidade de caracteres foi excedida";
        NotifiableObject objetoNotificavel = new NotaFake();
        var nomeCampo = "MotivoCancelamento";

        //Act
        ValidacoesDominio.NumeroMaximoCaracteres(texto, numeroMaximoCaracteres, objetoNotificavel, nomeCampo, mensagem);

        objetoNotificavel.Notificacoes.Should().BeEmpty();
        objetoNotificavel.Notificacoes.Should().HaveCount(default(int));
    }
}
