using CatalogoLivros.Aplicacao.Abstractions;
using CatalogoLivros.Dominio.Entities;
using CatalogoLivros.Infraestrutura.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CatalogoLivros.Infraestrutura.Repositories;

public sealed class RepositorioLivro : ILivroRepositorio
{
    private readonly AppDbContext _contexto;

    public RepositorioLivro(AppDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IReadOnlyCollection<Livro>> BuscarTodosAsync()
    {
        return await _contexto.Livros
            .AsNoTracking()
            .OrderBy(l => l.Titulo)
            .ToListAsync();
    }

    public Task<Livro?> BuscarPorIdAsync(Guid id)
    {
        return _contexto.Livros.FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task AdicionarAsync(Livro livro)
    {
        await _contexto.Livros.AddAsync(livro);
    }

    public void Remover(Livro livro)
    {
        _contexto.Livros.Remove(livro);
    }

    public Task SalvarAlteracoesAsync()
    {
        return _contexto.SaveChangesAsync();
    }
}