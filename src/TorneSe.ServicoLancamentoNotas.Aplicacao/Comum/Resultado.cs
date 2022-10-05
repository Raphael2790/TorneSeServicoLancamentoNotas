using TorneSe.ServicoLancamentoNotas.Aplicacao.Enums;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;

public abstract class Resultado
{
    public bool Sucesso { get; protected set; }

    public TipoErro? Erro { get; protected set; }

    public string DescricaoErro
    {
        get
        {
            return Erro?.ToString();
        }
        set
        {

        }
    }
    
    public IReadOnlyCollection<DetalheErro> DetalhesErros { get; protected set; }

    public Resultado SetarSucesso()
    {
        Sucesso = true;
        Erro = null;
        return this;
    }

    public Resultado SetarErro(TipoErro erro)
    {
        Sucesso = false;
        Erro = erro;
        return this;
    }

    public Resultado SetarErro(TipoErro erro, List<DetalheErro> detalheErros)
    {
        Sucesso = false;
        Erro = erro;
        DetalhesErros = detalheErros;
        return this;
    }
}

public sealed class Resultado<T> 
    : Resultado
{
    public T Dado { get; private set; }

    public new Resultado<T> SetarSucesso()
    {
        base.SetarSucesso();
        return this;
    }

    public Resultado<T> SetarSucesso(T resultado)
    {
        base.SetarSucesso();
        Dado = resultado;
        return this;
    }

    public new Resultado<T> SetarErro(TipoErro erro)
    {
        base.SetarErro(erro);
        return this;
    }

    public new Resultado<T> SetarErro(TipoErro erro, List<DetalheErro> detalheErros)
    {
        base.SetarErro(erro, detalheErros);
        return this;
    }

    public static Resultado<T> RetornaResultadoSucesso(T resultado)
        => new Resultado<T>().SetarSucesso(resultado);

    public static Resultado<T> RetornaResultadoErro(TipoErro erro)
        => new Resultado<T>().SetarErro(erro);

    public static Resultado<T> RetornaResultadoErro(TipoErro erro, List<DetalheErro> detalheErros)
        => new Resultado<T>().SetarErro(erro, detalheErros);
}