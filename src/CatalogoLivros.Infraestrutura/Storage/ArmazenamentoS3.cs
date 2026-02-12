using Amazon.S3;
using Amazon.S3.Model;
using CatalogoLivros.Aplicacao.Abstractions;

namespace CatalogoLivros.Infraestrutura.Storage;

public sealed class ArmazenamentoS3 : IArmazenamentoArquivo
{
    private readonly IAmazonS3 _s3;
    private readonly string _bucket;

    public ArmazenamentoS3(IAmazonS3 s3, string bucket)
    {
        _s3 = s3;
        _bucket = bucket;
    }

    public async Task<string> EnviarAsync(Stream arquivo, string nomeArquivo, string contentType)
    {
        var ext = Path.GetExtension(Path.GetFileName(nomeArquivo));
        var chave = $"capas/{Guid.NewGuid()}{ext}";

        var request = new PutObjectRequest
        {
            BucketName = _bucket,
            Key = chave,
            InputStream = arquivo,
            ContentType = string.IsNullOrWhiteSpace(contentType) ? "application/octet-stream" : contentType
        };

        await _s3.PutObjectAsync(request);
        return $"https://{_bucket}.s3.amazonaws.com/{chave}";
    }
}
