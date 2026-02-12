namespace CatalogoLivros.Aplicacao.Abstractions;

public interface IArmazenamentoArquivo
{
    Task<string> EnviarAsync(Stream arquivo, string nomeArquivo, string contentType);
}