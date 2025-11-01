using System.ComponentModel;

namespace CinemaApp.Models;

public class Produtora
{
    public int ProdutoraId { get; set; }
    public string? Nome { get; set; }
    [DisplayName("País Origem")]
    public string? PaisOrigem { get; set; }

    // Navegação
    public List<Filme>? Filmes { get; set; }
}