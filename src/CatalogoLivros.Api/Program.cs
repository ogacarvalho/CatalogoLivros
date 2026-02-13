using CatalogoLivros.Aplicacao.Abstractions;
using CatalogoLivros.Aplicacao.Services;
using CatalogoLivros.Infraestrutura.Persistence;
using CatalogoLivros.Infraestrutura.Repositories;
using CatalogoLivros.Api.Middlewares;
using Microsoft.EntityFrameworkCore;
using Amazon.S3;
using CatalogoLivros.Infraestrutura.Storage;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var bucket = builder.Configuration["Storage:S3:BucketName"]
    ?? throw new InvalidOperationException("Storage:S3:BucketName nao configurado.");

builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddScoped<IArmazenamentoArquivo>(_ =>
    new ArmazenamentoS3(_.GetRequiredService<IAmazonS3>(), bucket));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' nao foi encontrada.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<ILivroRepositorio, RepositorioLivro>();
builder.Services.AddScoped<IServicoLivro, ServicoLivro>();
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var erros = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value!.Errors
                        .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? "Valor invalido." : e.ErrorMessage)
                        .ToArray());

            return new BadRequestObjectResult(new
            {
                mensagem = "Dados invalidos.",
                erros
            });
        };
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var contexto = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    contexto.Database.Migrate();
}

app.MapControllers();
app.Run();
