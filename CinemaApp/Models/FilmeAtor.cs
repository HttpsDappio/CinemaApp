namespace CinemaApp.Models;

public class FilmeAtor
{
    public int FilmeId { get; set; }
    public int AtorId { get; set; }
    public string? Personagem { get; set; }

    // Navegação
    public Filme? Filme { get; set; }
    public Ator? Ator { get; set; }
}