namespace CatalogoLivros.Dominio.Entities;

public class Livro
{
    public Guid Id { get; private set; }
    public string Titulo { get; private set; }
    public string Autor { get; private set; }
    public int AnoLancamento { get; private set; }
    public string? UrlCapa { get; private set; }

    private Livro()
    {
        Titulo = string.Empty;
        Autor = string.Empty;
    }

    public Livro(string titulo, string autor, int anoLancamento)
    {
        Id = Guid.NewGuid();
        Titulo = titulo.Trim();
        Autor = autor.Trim();
        AnoLancamento = anoLancamento;
    }

    public void Atualizar(string titulo, string autor, int anoLancamento)
    {
        Titulo = titulo.Trim();
        Autor = autor.Trim();
        AnoLancamento = anoLancamento;
    }

    public void AtualizarCapa(string? urlCapa)
    {
        UrlCapa = urlCapa;
    }
}