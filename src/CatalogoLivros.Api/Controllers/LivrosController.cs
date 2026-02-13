using CatalogoLivros.Aplicacao.Abstractions;
using CatalogoLivros.Aplicacao.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoLivros.Api.Controllers;

[ApiController]
[Route("api/livros")]
public class LivrosController : ControllerBase
{
    private const long TamanhoMaximoCapaBytes = 5 * 1024 * 1024; // 5 MB
    private readonly IServicoLivro _servico;

    public LivrosController(IServicoLivro servico)
    {
        _servico = servico;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var livros = await _servico.BuscarTodosAsync();
        return Ok(livros);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var livro = await _servico.BuscarPorIdAsync(id);
        if (livro is null)
        {
            return NotFound(new { mensagem = "Livro nao encontrado." });
        }

        return Ok(livro);
    }

    [HttpPost]
    public async Task<IActionResult> Criar(CriarLivroRequest request)
    {
        var livro = await _servico.CriarAsync(request);
        return CreatedAtAction(nameof(ObterPorId), new { id = livro.Id }, livro);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, AtualizarLivroRequest request)
    {
        var livro = await _servico.AtualizarParcialAsync(id, request);
        if (livro is null)
        {
            return NotFound(new { mensagem = "Livro nao encontrado." });
        }

        return Ok(livro);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var removido = await _servico.RemoverAsync(id);
        if (!removido)
        {
            return NotFound(new { mensagem = "Livro nao encontrado." });
        }

        return NoContent();
    }

    [HttpPost("{id:guid}/capa")]
    public async Task<IActionResult> EnviarCapa(Guid id, IFormFile arquivo)
    {
        if (arquivo is null || arquivo.Length == 0)
        {
            return BadRequest(new { mensagem = "Arquivo invalido." });
        }

        if (!arquivo.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(new { mensagem = "A capa deve ser uma imagem." });
        }

        if (arquivo.Length > TamanhoMaximoCapaBytes)
        {
            return BadRequest(new { mensagem = "A capa deve ter no maximo 5 MB." });
        }
    
        await using var stream = arquivo.OpenReadStream();
        var livro = await _servico.EnviarCapaAsync(id, stream, arquivo.FileName, arquivo.ContentType);
    
        if (livro is null)
        {
            return NotFound(new { mensagem = "Livro nao encontrado." });
        }
    
        return Ok(livro);
    }
}
