namespace TorneSe.ServicoLancamentoNotas.Aplicacao.Contantes;

public static class ConstantesAplicacao
{
    public static class MensagensErro
    {
        public const string RECURSO_NAO_ENCONTRADO = "O recurso procurado não foi encontrado";
        public const string NOTA_INVALIDA = "A nota está inválida para lançamento";
        public const string ERRO_INESPERADO = "Ocorreu um erro inesperado";
        public const string INPUT_NOTA_INVALIDO = "Alguma(s) das informação(es) de entrada estão inválida(s)";
        public const string NAO_FOI_POSSIVEL_VALIDAR_VINCULOS_CURSO = "Não foi possível validar vinculos para o curso";
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
        public const string MOTIVO_CANCELAMENTO = "O motivo do cancelamento da nota deve ser informado";
        public const string ALUNO_CURSO_INVALIDO = "Aluno não está registrado em cursos";
        public const string PROFESSOR_CURSO_INVALIDO = "Professor não está vinculado a nenhuma atividade";
        public const string ATIVIDADE_CURSO_INVALIDA = "Não foi possível encontrar a atividade dentro do curso do aluno";
    }
}
