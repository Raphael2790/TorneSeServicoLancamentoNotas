using TorneSe.ServicoLancamentoNotas.Aplicacao.Comum;
using TorneSe.ServicoLancamentoNotas.Aplicacao.Contantes;
using TorneSe.ServicoLancamentoNotas.Dominio.Clients;

namespace TorneSe.ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;

public abstract class NotaHandler
{
	private ICursoClient _cursoClient;

    public NotaHandler(ICursoClient cursoClient) 
        => _cursoClient = cursoClient;

    public async Task<(bool,List<DetalheErro>)> ValidarInformacoesAlunoCurso(ValidacaoCursoInput input, CancellationToken cancellationToken)
    {
        var detalhesErros = new List<DetalheErro>();

        var cursos = await _cursoClient.ObterInformacoesCursoAluno(input.AlunoId, input.ProfessorId, input.AtividadeId, cancellationToken);

        var cursoAluno = cursos?.FirstOrDefault(curso => curso.Alunos.Any(aluno => aluno.Id == input.AlunoId));

        if(cursoAluno is null)
        {
            detalhesErros.Add(new DetalheErro("AlunoId", ConstantesAplicacao.MensagensValidacao.ALUNO_CURSO_INVALIDO));
            return (false, detalhesErros);
        }

        if (cursoAluno.Atividades.All(ativ => ativ.Id != input.AtividadeId))
        {
            detalhesErros.Add(new DetalheErro("AtividadeId", ConstantesAplicacao.MensagensValidacao.ATIVIDADE_CURSO_INVALIDA));
            return (false, detalhesErros);
        }

        if(cursoAluno.Atividades.Select(ativ => ativ.Professor).All(professor => professor.Id != input.ProfessorId))
        {
            detalhesErros.Add(new DetalheErro("ProfessorId", ConstantesAplicacao.MensagensValidacao.PROFESSOR_CURSO_INVALIDO));
            return (false, detalhesErros);
        }

        return (true, detalhesErros);
    }
}
