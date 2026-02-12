using CatalogoLivros.Dominio.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogoLivros.Infraestrutura.Persistence;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Livro> Livros => Set<Livro>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Livro>(builder =>
        {
            builder.ToTable("livros");

            builder.HasKey(l => l.Id);
            builder.Property(l => l.Id).HasColumnName("id");

            builder.Property(l => l.Titulo)
                .HasColumnName("titulo")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(l => l.Autor)
                .HasColumnName("autor")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(l => l.AnoLancamento)
                .HasColumnName("ano_lancamento")
                .IsRequired();

            builder.Property(l => l.UrlCapa)
                .HasColumnName("url_capa")
                .HasMaxLength(2048);
        });
    }
}