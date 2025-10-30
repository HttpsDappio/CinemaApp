using Microsoft.EntityFrameworkCore;
using CinemaApp.Data;
using CinemaApp.Models;

namespace CinemaApp.Repositories;

public class FilmeRepository : IFilmeRepository
{
    private readonly AppDbContext _context;

    public FilmeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Filme>> GetAllFilmesAsync()
    {
        return await _context.Filmes
            .Include(f => f.Categoria)
            .Include(f => f.Produtora)
            .Include(f => f.FilmesAtores!)
                .ThenInclude(fa => fa.Ator)
            .OrderBy(f => f.Titulo)
            .ToListAsync();
    }

    public async Task<Filme?> GetFilmeByIdAsync(int id)
    {
        return await _context.Filmes
            .Include(f => f.Categoria)
            .Include(f => f.Produtora)
            .Include(f => f.FilmesAtores!)
                .ThenInclude(fa => fa.Ator)
            .FirstOrDefaultAsync(f => f.FilmeId == id);
    }

    public async Task AddFilmeAsync(Filme filme)
    {
        _context.Filmes.Add(filme);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateFilmeAsync(Filme filme)
    {
        _context.Filmes.Update(filme);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFilmeAsync(int id)
    {
        var filme = await GetFilmeByIdAsync(id);
        if (filme != null)
        {
            _context.Filmes.Remove(filme);
            await _context.SaveChangesAsync();
        }
    }

    // CONSULTA 1: JOIN entre Filmes e Categorias (2 classes)
    public async Task<List<object>> Consulta1_FilmesComCategoriaAsync()
    {
        var resultado = await _context.Filmes
            .Join(_context.Categorias,
                filme => filme.CategoriaId,
                categoria => categoria.CategoriaId,
                (filme, categoria) => new
                {
                    FilmeId = filme.FilmeId,
                    Titulo = filme.Titulo,
                    Ano = filme.AnoLancamento,
                    Idade = DateTime.Now.Year - filme.AnoLancamento,
                    Categoria = categoria.Nome,
                    DescricaoCategoria = categoria.Descricao
                })
            .OrderBy(x => x.Categoria)
            .ThenBy(x => x.Titulo)
            .ToListAsync();

        return resultado.Cast<object>().ToList();
    }

    // CONSULTA 2: GROUP BY - Filmes agrupados por categoria
    public async Task<List<object>> Consulta2_FilmesPorCategoriaAsync()
    {
        var resultado = await _context.Filmes
            .Include(f => f.Categoria)
            .GroupBy(f => f.Categoria!.Nome)
            .Select(grupo => new
            {
                Categoria = grupo.Key,
                QuantidadeFilmes = grupo.Count(),
                AnoMaisRecente = grupo.Max(f => f.AnoLancamento),
                AnoMaisAntigo = grupo.Min(f => f.AnoLancamento),
                MediaAno = grupo.Average(f => f.AnoLancamento),
                Filmes = grupo.Select(f => f.Titulo).ToList()
            })
            .OrderByDescending(g => g.QuantidadeFilmes)
            .ToListAsync();

        return resultado.Cast<object>().ToList();
    }

    // CONSULTA 3: WHERE + HAVING - Filmes antigos por produtora
    public async Task<List<object>> Consulta3_FilmesAntigosPorProdutoraAsync()
    {
        var resultado = await _context.Filmes
            .Include(f => f.Produtora)
            .Where(f => f.AnoLancamento < 2000) // WHERE - filtro principal
            .GroupBy(f => f.Produtora!.Nome)
            .Where(grupo => grupo.Count() >= 1) // HAVING - filtro do grupo
            .Select(grupo => new
            {
                Produtora = grupo.Key,
                QuantidadeFilmesAntigos = grupo.Count(),
                MediaAno = grupo.Average(f => f.AnoLancamento),
                AnoMaisAntigo = grupo.Min(f => f.AnoLancamento),
                Filmes = grupo.Select(f => new
                {
                    Titulo = f.Titulo,
                    Ano = f.AnoLancamento,
                    Idade = DateTime.Now.Year - f.AnoLancamento
                }).ToList()
            })
            .OrderByDescending(g => g.QuantidadeFilmesAntigos)
            .ToListAsync();

        return resultado.Cast<object>().ToList();
    }

    public async Task<List<Categoria>> GetCategoriasAsync()
    {
        return await _context.Categorias.OrderBy(c => c.Nome).ToListAsync();
    }

    public async Task<List<Produtora>> GetProdutorasAsync()
    {
        return await _context.Produtoras.OrderBy(p => p.Nome).ToListAsync();
    }
}