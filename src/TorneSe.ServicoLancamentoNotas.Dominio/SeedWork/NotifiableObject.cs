using FluentValidation;

namespace TorneSe.ServicoLancamentoNotas.Dominio.SeedWork;

public abstract class NotifiableObject
{
    public bool EhValida { get; set; }
    public List<Notificacao> Notificacoes { get; set; } = new();

    public void Notificar(Notificacao notificacao)
        => Notificacoes.Add(notificacao);

    public void Validar<TModel>(TModel objetoNotificavel, AbstractValidator<TModel> validador)
    {
        var validadorResultado = validador.Validate(objetoNotificavel);
        EhValida = validadorResultado.IsValid;
        validadorResultado.Errors.ForEach(x => Notificar(new(x.PropertyName, x.ErrorMessage)));
    }
}
