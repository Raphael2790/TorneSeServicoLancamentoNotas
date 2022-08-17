using FluentValidation;
using TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;

namespace TorneSe.ServicoLancamentoNotas.Dominio.Validacoes;

public static class ValidacoesDominio
{
    public static void DeveEstarEntre(double valor, double valorInicialIntervalo,
                                        double valorFinalIntervalo, NotifiableObject objetoNotificavel, string nomeCampo, string mensagem)
    {
        if (valor < valorInicialIntervalo || valor > valorFinalIntervalo)
            objetoNotificavel.Notificar(new Notificacao(nomeCampo, mensagem));
    }

    public static void MaiorQue(int valor, int valorMinimo, NotifiableObject objetoNotificavel, string nomeCampo, string mensagem)
    {
        if(valor <= valorMinimo)
            objetoNotificavel.Notificar(new Notificacao(nomeCampo, mensagem));
    }

    public static void NumeroMaximoCaracteres(string texto, int numeroMaximoCaracteres, NotifiableObject objetoNotificavel, string nomeCampo, string mensagem)
    {
        if(texto.Length > numeroMaximoCaracteres)
            objetoNotificavel.Notificar(new Notificacao(nomeCampo, mensagem));
    }

    public static void Validar<TModel>(TModel objetoNotificavel, AbstractValidator<TModel> validador)
        where TModel : NotifiableObject
    {
        var validadorResultado = validador.Validate(objetoNotificavel);
        objetoNotificavel.EhValida = validadorResultado.IsValid;
        validadorResultado.Errors.ForEach(x => objetoNotificavel.Notificar(new(x.PropertyName, x.ErrorMessage)));
    }
}
