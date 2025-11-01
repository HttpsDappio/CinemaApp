using System.ComponentModel;

namespace CinemaApp.Models;

public class Ator
{
    public int AtorId { get; set; }
    public string? Nome { get; set; }
    public string? Nacionalidade { get; set; }

    // Navegação
    public List<FilmeAtor>? FilmesAtores { get; set; }
}