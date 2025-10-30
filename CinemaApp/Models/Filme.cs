using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models;

public class Filme
{
    public int FilmeId { get; set; }

    [Required(ErrorMessage = "O título é obrigatório")]
    public string? Titulo { get; set; }

    [Range(1900, 2025, ErrorMessage = "Ano deve estar entre 1900 e 2025")]
    public int AnoLancamento { get; set; }

    public int CategoriaId { get; set; }
    public int ProdutoraId { get; set; }

    // Navegação
    public Categoria? Categoria { get; set; }
    public Produtora? Produtora { get; set; }
    public List<FilmeAtor>? FilmesAtores { get; set; }

    public void ExibirInformacoes()
    {
        Console.WriteLine($"Título: {Titulo}, Ano: {AnoLancamento}");
    }

    public int CalcularIdadeFilme()
    {
        return DateTime.Now.Year - AnoLancamento;
    }
}