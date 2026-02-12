using CatalogoLivros.Aplicacao.Abstractions;
using CatalogoLivros.Aplicacao.DTOs;
using CatalogoLivros.Aplicacao.Services;
using CatalogoLivros.Dominio.Entities;
using FluentAssertions;
using Xunit;

namespace CatalogoLivros.Aplicacao.Tests;

public class ServicoLivroTests
{
    [Fact]
    public async Task CriarAsync_DeveCriarLivro_QuandoDadosValidos()
    {
        var repositorio = new RepositorioLivroEmMemoria();
        var armazenamento = new ArmazenamentoArquivoFake();
        var servico = new ServicoLivro(repositorio, armazenamento);

        var request = new CriarLivroRequest
        {
            Titulo = "Codigo Limpo",
            Autor = "Robert C. Martin",
            AnoLancamento = 2008
        };

        var livro = await servico.CriarAsync(request);

        livro.Titulo.Should().Be("Codigo Limpo");
        livro.Autor.Should().Be("Robert C. Martin");
        livro.AnoLancamento.Should().Be(2008);
        repositorio.Itens.Should().HaveCount(1);
    }

    [Fact]
    public async Task AtualizarParcialAsync_DeveAtualizarApenasOCampoInformado()
    {
        var repositorio = new RepositorioLivroEmMemoria();
        var armazenamento = new ArmazenamentoArquivoFake();
        var servico = new ServicoLivro(repositorio, armazenamento);

        var criado = await servico.CriarAsync(new CriarLivroRequest
        {
            Titulo = "Refactoring",
            Autor = "Martin Fowler",
            AnoLancamento = 1999
        });

        var atualizado = await servico.AtualizarParcialAsync(criado.Id, new AtualizarLivroRequest { Titulo = "Refactoring 2" });

        atualizado.Should().NotBeNull();
        atualizado!.Titulo.Should().Be("Refactoring 2");
        atualizado.Autor.Should().Be("Martin Fowler");
        atualizado.AnoLancamento.Should().Be(1999);
    }

    [Fact]
    public async Task RemoverAsync_DeveRetornarFalse_QuandoNaoExistirLivro()
    {
        var repositorio = new RepositorioLivroEmMemoria();
        var armazenamento = new ArmazenamentoArquivoFake();
        var servico = new ServicoLivro(repositorio, armazenamento);

        var removido = await servico.RemoverAsync(Guid.NewGuid());

        removido.Should().BeFalse();
    }

    private sealed class RepositorioLivroEmMemoria : ILivroRepositorio
    {
        public List<Livro> Itens { get; } = new();

        public Task<IReadOnlyCollection<Livro>> BuscarTodosAsync()
        {
            return Task.FromResult((IReadOnlyCollection<Livro>)Itens.ToArray());
        }

        public Task<Livro?> BuscarPorIdAsync(Guid id)
        {
            return Task.FromResult(Itens.FirstOrDefault(x => x.Id == id));
        }

        public Task AdicionarAsync(Livro livro)
        {
            Itens.Add(livro);
            return Task.CompletedTask;
        }

        public void Remover(Livro livro)
        {
            Itens.Remove(livro);
        }

        public Task SalvarAlteracoesAsync()
        {
            return Task.CompletedTask;
        }
    }

    private sealed class ArmazenamentoArquivoFake : IArmazenamentoArquivo
    {
        public Task<string> EnviarAsync(Stream arquivo, string nomeArquivo, string contentType)
        {
            return Task.FromResult("http://localhost:8080/capas/fake.jpg");
        }
    }
}
