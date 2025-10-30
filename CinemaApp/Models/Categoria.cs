namespace CinemaApp.Models;

public class Categoria
{
    public int CategoriaId { get; set; }
    public string? Nome { get; set; }
    public string? Descricao { get; set; }

    // Navegação
    public List<Filme>? Filmes { get; set; }
}