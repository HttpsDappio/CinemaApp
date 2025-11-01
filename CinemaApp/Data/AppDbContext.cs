using Microsoft.EntityFrameworkCore;
using CinemaApp.Models;

namespace CinemaApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Filme> Filmes { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Produtora> Produtoras { get; set; }
    public DbSet<Ator> Atores { get; set; }
    public DbSet<FilmeAtor> FilmesAtores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<FilmeAtor>()
            .HasKey(fa => new { fa.FilmeId, fa.AtorId });

        modelBuilder.Entity<FilmeAtor>()
            .HasOne(fa => fa.Filme)
            .WithMany(f => f.FilmesAtores)
            .HasForeignKey(fa => fa.FilmeId);

        modelBuilder.Entity<FilmeAtor>()
            .HasOne(fa => fa.Ator)
            .WithMany(a => a.FilmesAtores)
            .HasForeignKey(fa => fa.AtorId);

        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { CategoriaId = 1, Nome = "Ação", Descricao = "Filmes de ação e aventura" },
            new Categoria { CategoriaId = 2, Nome = "Drama", Descricao = "Filmes dramáticos" },
            new Categoria { CategoriaId = 3, Nome = "Comédia", Descricao = "Filmes cômicos" },
            new Categoria { CategoriaId = 4, Nome = "Ficção Científica", Descricao = "Filmes de ficção científica" }
        );

        modelBuilder.Entity<Produtora>().HasData(
            new Produtora { ProdutoraId = 1, Nome = "Warner Bros", PaisOrigem = "EUA" },
            new Produtora { ProdutoraId = 2, Nome = "Universal Pictures", PaisOrigem = "EUA" },
            new Produtora { ProdutoraId = 3, Nome = "Paramount", PaisOrigem = "EUA" },
            new Produtora { ProdutoraId = 4, Nome = "Globo Filmes", PaisOrigem = "Brasil" }
        );

        modelBuilder.Entity<Ator>().HasData(
            new Ator { AtorId = 1, Nome = "Tom Hanks", Nacionalidade = "Americano" },
            new Ator { AtorId = 2, Nome = "Meryl Streep", Nacionalidade = "Americana" },
            new Ator { AtorId = 3, Nome = "Leonardo DiCaprio", Nacionalidade = "Americano" },
            new Ator { AtorId = 4, Nome = "Fernanda Montenegro", Nacionalidade = "Brasileira" }
        );

        modelBuilder.Entity<Filme>().HasData(
            new Filme { FilmeId = 1, Titulo = "Forrest Gump", AnoLancamento = 1994, CategoriaId = 2, ProdutoraId = 1 },
            new Filme { FilmeId = 2, Titulo = "Matrix", AnoLancamento = 1999, CategoriaId = 1, ProdutoraId = 1 },
            new Filme { FilmeId = 3, Titulo = "Titanic", AnoLancamento = 1997, CategoriaId = 2, ProdutoraId = 2 },
            new Filme { FilmeId = 4, Titulo = "O Poderoso Chefão", AnoLancamento = 1972, CategoriaId = 2, ProdutoraId = 3 },
            new Filme { FilmeId = 5, Titulo = "Central do Brasil", AnoLancamento = 1998, CategoriaId = 2, ProdutoraId = 4 }
        );

        modelBuilder.Entity<FilmeAtor>().HasData(
            new FilmeAtor { FilmeId = 1, AtorId = 1, Personagem = "Forrest Gump" },
            new FilmeAtor { FilmeId = 2, AtorId = 1, Personagem = "Personagem em Matrix" },
            new FilmeAtor { FilmeId = 3, AtorId = 3, Personagem = "Jack Dawson" },
            new FilmeAtor { FilmeId = 5, AtorId = 4, Personagem = "Dora" }
        );
    }
}