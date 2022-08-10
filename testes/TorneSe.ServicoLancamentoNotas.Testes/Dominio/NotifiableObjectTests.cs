using FluentAssertions;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;
using TorneSe.ServicoLancamentoNotas.Testes.Fakes;

namespace TorneSe.ServicoLancamentoNotas.Testes.Dominio;

public class NotifiableObjectTests
{
    [Fact(DisplayName = nameof(Notificar_DeveAdicionar_NotificacaoNaLista))]
    [Trait("Dominio", "NotifiableObject - Notificacao")]
    public void Notificar_DeveAdicionar_NotificacaoNaLista()
    {
        //Arrange
        string nomeCampo = "UsuarioId";
        string mensagem = "mensagem teste";
        NotifiableObject objetoNotificavel = new NotaFake();

        //Act
        objetoNotificavel.Notificar(new Notificacao(nomeCampo,mensagem));

        //Arrange
        objetoNotificavel.Notificacoes.Should().NotBeEmpty();
        objetoNotificavel.Notificacoes.Should().HaveCount(1);
        objetoNotificavel.Notificacoes.First().Campo.Should().Be(nomeCampo);
        objetoNotificavel.Notificacoes.First().Mensagem.Should().Be(mensagem);
    }
}

