using CinemaApp.Models;

namespace CinemaApp.Repositories;

public interface IFilmeRepository
{
    Task<List<Filme>> GetAllFilmesAsync();
    Task<Filme?> GetFilmeByIdAsync(int id);
    Task AddFilmeAsync(Filme filme);
    Task UpdateFilmeAsync(Filme filme);
    Task DeleteFilmeAsync(int id);

    // CONSULTAS LINQ
    Task<List<object>> Consulta1_FilmesComCategoriaAsync();        // JOIN entre 2 classes
    Task<List<object>> Consulta2_FilmesPorCategoriaAsync();        // GROUP BY
    Task<List<object>> Consulta3_FilmesAntigosPorProdutoraAsync(); // WHERE + HAVING

    // Métodos auxiliares
    Task<List<Categoria>> GetCategoriasAsync();
    Task<List<Produtora>> GetProdutorasAsync();
}