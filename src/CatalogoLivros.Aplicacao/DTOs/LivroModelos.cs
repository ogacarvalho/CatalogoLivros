namespace CatalogoLivros.Aplicacao.DTOs;

public sealed record LivroDto(Guid Id, string Titulo, string Autor, int AnoLancamento);

public sealed class CriarLivroRequest
{
    public string Titulo { get; init; } = string.Empty;
    public string Autor { get; init; } = string.Empty;
    public int AnoLancamento { get; init; }
}

public sealed class AtualizarLivroRequest
{
    public string? Titulo { get; init; }
    public string? Autor { get; init; }
    public int? AnoLancamento { get; init; }
}