namespace TorneSe.ServicoLancamentoNotas.Aplicacao.Contantes;

public static class ConstantesAplicacao
{
    public static class MensagensErro
    {
        public const string RECURSO_NAO_ENCONTRADO = "O recurso procurado não foi encontrado";
        public const string NOTA_INVALIDA = "A nota está inválida para lançamento";
        public const string ERRO_INESPERADO = "Ocorreu um erro inesperado";
        public const string INPUT_NOTA_INVALIDO = "Alguma(s) das informação(es) de entrada estão inválida(s)";
    }

    public static class TenantsAplicacao
    {
        public const string TORNE_SE_CSHARP = "torne-se-csharp";
        public const string TORNE_SE_JAVA = "torne-se-java";
        public const string TORNE_SE_JAVASCRIPT = "torne-se-javascript";
    }

    public static class MensagensValidacao
    {
        public const string ALUNO_ID_INVALIDO = "O campo com o id aluno informado está inválido!";
        public const string PROFESSOR_ID_INVALIDO = "O campo com o id professor informado está inválido!";
        public const string ATIVIDADE_ID_INVALIDO = "O campo com o id da atividade informada está inválida";
        public const string NOTA_INVALIDA = "O campo com valor da nota informada está inválida";
    }
}
