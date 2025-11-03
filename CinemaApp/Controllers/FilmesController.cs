using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Data;
using CinemaApp.Models;

namespace CinemaApp.Controllers
{
    public class FilmesController : Controller
    {
        private readonly AppDbContext _context;

        public FilmesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Filmes
        public async Task<IActionResult> Index()
        {
            var filmes = await _context.Filmes
                .Include(f => f.Categoria)
                .Include(f => f.Produtora)
                .ToListAsync();
            return View(filmes);
        }

        // GET: Filmes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filme = await _context.Filmes
                .Include(f => f.Categoria)
                .Include(f => f.Produtora)
                .FirstOrDefaultAsync(m => m.FilmeId == id);
            if (filme == null)
            {
                return NotFound();
            }

            return View(filme);
        }

        // GET: Filmes/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nome");
            ViewData["ProdutoraId"] = new SelectList(_context.Produtoras, "ProdutoraId", "Nome");
            return View();
        }

        // POST: Filmes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FilmeId,Titulo,AnoLancamento,CategoriaId,ProdutoraId")] Filme filme)
        {
            if (ModelState.IsValid)
            {
                _context.Add(filme);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nome", filme.CategoriaId);
            ViewData["ProdutoraId"] = new SelectList(_context.Produtoras, "ProdutoraId", "Nome", filme.ProdutoraId);
            return View(filme);
        }

        // GET: Filmes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filme = await _context.Filmes.FindAsync(id);
            if (filme == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nome", filme.CategoriaId);
            ViewData["ProdutoraId"] = new SelectList(_context.Produtoras, "ProdutoraId", "Nome", filme.ProdutoraId);
            return View(filme);
        }

        // POST: Filmes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FilmeId,Titulo,AnoLancamento,CategoriaId,ProdutoraId")] Filme filme)
        {
            if (id != filme.FilmeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(filme);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmeExists(filme.FilmeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nome", filme.CategoriaId);
            ViewData["ProdutoraId"] = new SelectList(_context.Produtoras, "ProdutoraId", "Nome", filme.ProdutoraId);
            return View(filme);
        }

        // GET: Filmes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filme = await _context.Filmes
                .Include(f => f.Categoria)
                .Include(f => f.Produtora)
                .FirstOrDefaultAsync(m => m.FilmeId == id);
            if (filme == null)
            {
                return NotFound();
            }

            return View(filme);
        }

        // POST: Filmes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var filme = await _context.Filmes.FindAsync(id);
            if (filme != null)
            {
                _context.Filmes.Remove(filme);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmeExists(int id)
        {
            return _context.Filmes.Any(e => e.FilmeId == id);
        }


// CONSULTAS LINQ 
        
        // CONSULTA 1: Dados de duas classes (Filmes + Categorias)
        public async Task<IActionResult> Consulta1(int? categoriaId)
        {
            // Buscar categorias para o select
            ViewBag.Categorias = new SelectList(await _context.Categorias.ToListAsync(), "CategoriaId", "Nome");

            var query = _context.Filmes
                .Include(f => f.Categoria)
                .Include(f => f.Produtora)
                .AsQueryable();

            // Aplicar filtro de categoria se selecionado
            if (categoriaId.HasValue)
            {
                query = query.Where(f => f.CategoriaId == categoriaId.Value);
            }

            var resultado = await query
                .Select(f => new
                {
                    Filme = f.Titulo,
                    Ano = f.AnoLancamento,
                    Idade = DateTime.Now.Year - f.AnoLancamento, 
                    Categoria = f.Categoria.Nome,
                    DescricaoCategoria = f.Categoria.Descricao,
                    Produtora = f.Produtora.Nome
                })
                .OrderBy(f => f.Categoria)
                .ThenBy(f => f.Filme)
                .ToListAsync();

            var resultadoConvertido = resultado.Cast<object>().ToList();

            ViewBag.Titulo = "Consulta 1 - Filmes por Categoria";
            ViewBag.Descricao = "Dados combinados de Filmes e Categorias";
            ViewBag.CategoriaSelecionada = categoriaId;

            return View("Consulta1", resultadoConvertido);
        }

        // CONSULTA 2: Funções de grupo (GROUP BY)
        public async Task<IActionResult> Consulta2(string grupoPor)
        {
            var opcoesGrupo = new List<SelectListItem>
    {
        new SelectListItem { Value = "categoria", Text = "Categoria" },
        new SelectListItem { Value = "produtora", Text = "Produtora" },
        new SelectListItem { Value = "ano", Text = "Ano de Lançamento" }
    };
            ViewBag.GrupoPor = new SelectList(opcoesGrupo, "Value", "Text", grupoPor);

            // Definir view baseada no grupo
            ViewBag.Titulo = "Consulta 2 - Estatísticas por Grupo";
            ViewBag.Descricao = $"Agrupado por: {(string.IsNullOrEmpty(grupoPor) ? "Categoria" : grupoPor)}";
            ViewBag.GrupoSelecionado = grupoPor;

            if (string.IsNullOrEmpty(grupoPor))
            {
                return View("Consulta2", new List<object>());
            }

            // Fazer a consulta e passa diretamente para a view
            switch (grupoPor)
            {
                case "categoria":
                    var resultadoCategoria = await _context.Filmes
                        .Include(f => f.Categoria)
                        .GroupBy(f => f.Categoria.Nome)
                        .Select(g => new
                        {
                            Grupo = g.Key,
                            TotalFilmes = g.Count(),
                            AnoMaisRecente = g.Max(f => f.AnoLancamento),
                            AnoMaisAntigo = g.Min(f => f.AnoLancamento),
                            AnoMedio = g.Average(f => f.AnoLancamento)
                        })
                        .OrderByDescending(x => x.TotalFilmes)
                        .ToListAsync();
                    return View("Consulta2", resultadoCategoria);

                case "produtora":
                    var resultadoProdutora = await _context.Filmes
                        .Include(f => f.Produtora)
                        .GroupBy(f => f.Produtora.Nome)
                        .Select(g => new
                        {
                            Grupo = g.Key,
                            TotalFilmes = g.Count(),
                            AnoMaisRecente = g.Max(f => f.AnoLancamento),
                            AnoMaisAntigo = g.Min(f => f.AnoLancamento),
                            AnoMedio = g.Average(f => f.AnoLancamento)
                        })
                        .OrderByDescending(x => x.TotalFilmes)
                        .ToListAsync();
                    return View("Consulta2", resultadoProdutora);

                case "ano":
                    var resultadoAno = await _context.Filmes
                        .GroupBy(f => f.AnoLancamento)
                        .Select(g => new
                        {
                            Grupo = g.Key.ToString(),
                            TotalFilmes = g.Count(),
                            AnoMaisRecente = g.Key,
                            AnoMaisAntigo = g.Key, 
                            AnoMedio = g.Average(f => f.AnoLancamento)
                        })
                        .OrderByDescending(x => x.TotalFilmes)
                        .ToListAsync();
                    return View("Consulta2", resultadoAno);

                default:
                    return View("Consulta2", new List<object>());
            }
        }

        // CONSULTA 3: WHERE + HAVING
        public async Task<IActionResult> Consulta3(string filtroWhere, int? quantidadeMinima)
        {
            var opcoesWhere = new List<SelectListItem>
    {
        new SelectListItem { Value = "antigos", Text = "Filmes Antigos (antes de 2000)" },
        new SelectListItem { Value = "recentes", Text = "Filmes Recentes (após 2010)" },
        new SelectListItem { Value = "classicos", Text = "Filmes Clássicos (1980-1999)" },
        new SelectListItem { Value = "modernos", Text = "Filmes Modernos (2000-2010)" }
    };
            ViewBag.FiltroWhere = new SelectList(opcoesWhere, "Value", "Text", filtroWhere);

            var opcoesHaving = new List<SelectListItem>
    {
        new SelectListItem { Value = "1", Text = "Pelo menos 1 filme" },
        new SelectListItem { Value = "2", Text = "Pelo menos 2 filmes" },
        new SelectListItem { Value = "3", Text = "Pelo menos 3 filmes" }
    };
            ViewBag.QuantidadeMinima = new SelectList(opcoesHaving, "Value", "Text", quantidadeMinima?.ToString());

            var query = _context.Filmes.AsQueryable();

            // WHERE - Filtro principal
            switch (filtroWhere)
            {
                case "antigos":
                    query = query.Where(f => f.AnoLancamento < 2000);
                    break;
                case "recentes":
                    query = query.Where(f => f.AnoLancamento > 2010);
                    break;
                case "classicos":
                    query = query.Where(f => f.AnoLancamento >= 1980 && f.AnoLancamento <= 1999);
                    break;
                case "modernos":
                    query = query.Where(f => f.AnoLancamento >= 2000 && f.AnoLancamento <= 2010);
                    break;
            }

            // GROUP BY + HAVING 
            var resultado = await query
                .Include(f => f.Categoria)
                .GroupBy(f => f.Categoria.Nome)
                .Where(g => !quantidadeMinima.HasValue || g.Count() >= quantidadeMinima.Value) // HAVING
                .Select(g => new
                {
                    Categoria = g.Key,
                    QuantidadeFilmes = g.Count(),
                    AnoMedio = g.Average(f => f.AnoLancamento),
                    IdadeMedia = g.Average(f => DateTime.Now.Year - f.AnoLancamento),
                    Filmes = g.Select(f => new { f.Titulo, f.AnoLancamento }).ToList()
                })
                .OrderByDescending(g => g.QuantidadeFilmes)
                .ToListAsync();

            var resultadoConvertido = resultado.Cast<object>().ToList();

            ViewBag.Titulo = "Consulta 3 - Filtros WHERE + HAVING";
            ViewBag.Descricao = "Filtre filmes por ano e agrupe por categoria com quantidade mínima";
            ViewBag.FiltroSelecionado = filtroWhere;
            ViewBag.QuantidadeSelecionada = quantidadeMinima;

            return View("Consulta3", resultadoConvertido);
        }
    }
}