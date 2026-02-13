using CatalogoLivros.Aplicacao.Abstractions;
using CatalogoLivros.Aplicacao.Services;
using CatalogoLivros.Infraestrutura.Persistence;
using CatalogoLivros.Infraestrutura.Repositories;
using Microsoft.EntityFrameworkCore;
using Amazon.S3;
using CatalogoLivros.Infraestrutura.Storage;

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
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var contexto = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    contexto.Database.Migrate();
}

app.MapControllers();
app.Run();
