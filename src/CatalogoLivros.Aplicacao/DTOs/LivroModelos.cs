using System.ComponentModel.DataAnnotations;

namespace CatalogoLivros.Aplicacao.DTOs;

public sealed record LivroDto(Guid Id, string Titulo, string Autor, int AnoLancamento, string? UrlCapa);

public sealed class CriarLivroRequest
{
    [Required(ErrorMessage = "Titulo e obrigatorio.")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Titulo deve ter entre 1 e 200 caracteres.")]
    [RegularExpression(@".*\S.*", ErrorMessage = "Titulo e obrigatorio.")]
    public string Titulo { get; init; } = string.Empty;

    [Required(ErrorMessage = "Autor e obrigatorio.")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Autor deve ter entre 1 e 200 caracteres.")]
    [RegularExpression(@".*\S.*", ErrorMessage = "Autor e obrigatorio.")]
    public string Autor { get; init; } = string.Empty;

    [Range(1450, 3000, ErrorMessage = "Ano de lancamento invalido. Use entre 1450 e 3000.")]
    public int AnoLancamento { get; init; }
}

public sealed class AtualizarLivroRequest
{
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Titulo deve ter entre 1 e 200 caracteres.")]
    [RegularExpression(@".*\S.*", ErrorMessage = "Titulo nao pode ser vazio.")]
    public string? Titulo { get; init; }

    [StringLength(200, MinimumLength = 1, ErrorMessage = "Autor deve ter entre 1 e 200 caracteres.")]
    [RegularExpression(@".*\S.*", ErrorMessage = "Autor nao pode ser vazio.")]
    public string? Autor { get; init; }

    [Range(1450, 3000, ErrorMessage = "Ano de lancamento invalido. Use entre 1450 e 3000.")]
    public int? AnoLancamento { get; init; }
}
