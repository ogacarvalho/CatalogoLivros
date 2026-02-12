using CatalogoLivros.Dominio.Entities;

namespace CatalogoLivros.Aplicacao.Abstractions;

public interface ILivroRepositorio
{
    Task<IReadOnlyCollection<Livro>> BuscarTodosAsync();
    Task<Livro?> BuscarPorIdAsync(Guid id);
    Task AdicionarAsync(Livro livro);
    void Remover(Livro livro);
    Task SalvarAlteracoesAsync();
}