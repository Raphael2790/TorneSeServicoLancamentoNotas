namespace TorneSe.ServicoLancamentoNotas.Dominio.SeedWork.BuscaRepository;

public record struct BuscaOutput<TAgregado>
    where TAgregado : IRaizAgregacao
{
    public int Pagina { get; set; }
    public int PorPagina { get; set; }
    public int Total { get; set; }
    public IReadOnlyList<TAgregado> Items { get; set;}

    public BuscaOutput(int pagina, int porPagina, int total, IReadOnlyList<TAgregado> items)
    {
        Pagina = pagina;
        PorPagina = porPagina;
        Total = total;
        Items = items;
    }
}
