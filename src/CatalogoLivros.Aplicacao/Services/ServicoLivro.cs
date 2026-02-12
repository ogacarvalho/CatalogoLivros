using CatalogoLivros.Aplicacao.Abstractions;
using CatalogoLivros.Aplicacao.DTOs;
using CatalogoLivros.Dominio.Entities;

namespace CatalogoLivros.Aplicacao.Services;

public sealed class ServicoLivro : IServicoLivro
{
    private readonly ILivroRepositorio _repositorio;

    public ServicoLivro(ILivroRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IReadOnlyCollection<LivroDto>> BuscarTodosAsync()
    {
        var livros = await _repositorio.BuscarTodosAsync();
        return livros.Select(Mapear).ToArray();
    }

    public async Task<LivroDto?> BuscarPorIdAsync(Guid id)
    {
        var livro = await _repositorio.BuscarPorIdAsync(id);
        return livro is null ? null : Mapear(livro);
    }

    public async Task<LivroDto> CriarAsync(CriarLivroRequest request)
    {
        Validar(request.Titulo, request.Autor, request.AnoLancamento);

        var livro = new Livro(request.Titulo, request.Autor, request.AnoLancamento);
        await _repositorio.AdicionarAsync(livro);
        await _repositorio.SalvarAlteracoesAsync();

        return Mapear(livro);
    }

    public async Task<LivroDto?> AtualizarParcialAsync(Guid id, AtualizarLivroRequest request)
    {
        var livro = await _repositorio.BuscarPorIdAsync(id);
        if (livro is null)
        {
            return null;
        }

        var titulo = request.Titulo?.Trim() ?? livro.Titulo;
        var autor = request.Autor?.Trim() ?? livro.Autor;
        var anoLancamento = request.AnoLancamento ?? livro.AnoLancamento;

        Validar(titulo, autor, anoLancamento);

        livro.Atualizar(titulo, autor, anoLancamento);
        await _repositorio.SalvarAlteracoesAsync();

        return Mapear(livro);
    }

    public async Task<bool> RemoverAsync(Guid id)
    {
        var livro = await _repositorio.BuscarPorIdAsync(id);
        if (livro is null)
        {
            return false;
        }

        _repositorio.Remover(livro);
        await _repositorio.SalvarAlteracoesAsync();
        return true;
    }

    private static LivroDto Mapear(Livro livro) =>
        new(livro.Id, livro.Titulo, livro.Autor, livro.AnoLancamento);

    private static void Validar(string titulo, string autor, int anoLancamento)
    {
        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new ArgumentException("Titulo e obrigatorio.");
        }

        if (string.IsNullOrWhiteSpace(autor))
        {
            throw new ArgumentException("Autor e obrigatorio.");
        }

        var anoAtual = DateTime.UtcNow.Year;
        if (anoLancamento < 1450 || anoLancamento > anoAtual)
        {
            throw new ArgumentException($"Ano de lancamento invalido. Use entre 1450 e {anoAtual}.");
        }
    }
}