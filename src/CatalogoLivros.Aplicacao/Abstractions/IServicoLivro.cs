using CatalogoLivros.Aplicacao.DTOs;

namespace CatalogoLivros.Aplicacao.Abstractions;

public interface IServicoLivro
{
    Task<IReadOnlyCollection<LivroDto>> BuscarTodosAsync();
    Task<LivroDto?> BuscarPorIdAsync(Guid id);
    Task<LivroDto> CriarAsync(CriarLivroRequest request);
    Task<LivroDto?> AtualizarParcialAsync(Guid id, AtualizarLivroRequest request);
    Task<bool> RemoverAsync(Guid id);
}