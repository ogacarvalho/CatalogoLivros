using CatalogoLivros.Aplicacao.Abstractions;
using CatalogoLivros.Aplicacao.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoLivros.Api.Controllers;

[ApiController]
[Route("api/livros")]
public class LivrosController : ControllerBase
{
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
        try
        {
            var livro = await _servico.CriarAsync(request);
            return CreatedAtAction(nameof(ObterPorId), new { id = livro.Id }, livro);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, AtualizarLivroRequest request)
    {
        try
        {
            var livro = await _servico.AtualizarParcialAsync(id, request);
            if (livro is null)
            {
                return NotFound(new { mensagem = "Livro nao encontrado." });
            }

            return Ok(livro);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
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
}