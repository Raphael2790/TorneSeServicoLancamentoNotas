namespace TorneSe.ServicoLancamentoNotas.Dominio.Constantes;

public static class ConstantesDominio
{
    public static class MensagensValidacoes
    {
        public const string ERRO_VALOR_NOTA_INVALIDO = "O valor da nota deve estar no intervalo de 0 a 10";
        public const string ERRO_USUARIO_INVALIDO = "O identificador do usuário deve ser maior que 0";
        public const string ERRO_ALUNO_INVALIDO = "O identificador do aluno deve ser maior que 0";
        public const string ERRO_ATIVIDADE_INVALIDA = "O identificador da atividade deve ser maior que 0";
        public const string ERRO_MOTIVO_CANCELAMENTO_EXTENSO = "O motivo do cancelamento deve conter no maximo 500 caracteres";
        public const string ERRO_MOTIVO_CANCELAMENTO_NAO_INFORMADO = "O motivo do cancelamento deve ser informado";
        public const string ERRO_FLAG_CANCELADA_INATIVA = "A flag de cancelamento deve ser marcada";
    }
}
