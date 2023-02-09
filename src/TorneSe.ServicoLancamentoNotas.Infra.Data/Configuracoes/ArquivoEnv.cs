using DotNetEnv;

namespace TorneSe.ServicoLancamentoNotas.Infra.Data.Configuracoes;

public static class ArquivoEnv
{
    public static void CarregarVariaveis(LoadOptions? loadOptions = null)
    {
        ConfigurarEnv();

        const string fileName = ".env";
        var assemblyDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var file = Path.Combine(assemblyDirectory, fileName);

        if (File.Exists(file))
        {
            Env.Load(file, loadOptions);
        }
    }

    private static void ConfigurarEnv()
    {
        const string fileName = ".env";

        var sourcePath = ObterDiretorio(fileName)?.FullName ?? string.Empty;
        var sourceFile = Path.Combine(sourcePath, fileName);

        var destPath = AppDomain.CurrentDomain.BaseDirectory;
        var destFile = Path.Combine(destPath, fileName);

        if (!File.Exists(sourceFile)) return;

        var sr = File.OpenText(sourceFile);
        var fileTxt = sr.ReadToEnd();
        sr.Close();


        var fileInfo = new FileInfo(destFile);
        var sw = fileInfo.CreateText();
        sw.Write(fileTxt);
        sw.Close();
    }

    public static DirectoryInfo ObterDiretorio(string filename)
    {
        var diretorio = Directory.GetParent(Directory.GetCurrentDirectory());
        while (diretorio != null && !diretorio.GetFiles(filename).Any())
        {
            diretorio = diretorio.Parent;
        }

        return diretorio;
    }
}
