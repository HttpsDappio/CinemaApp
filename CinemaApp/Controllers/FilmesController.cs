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

        // =============================================
        // CONSULTAS LINQ - ATENDENDO OS PEDIDOS
        // =============================================

        // CONSULTA 1: JOIN entre 2 classes (Filme + Categoria)
        public async Task<IActionResult> Consulta1()
        {
            var resultado = await _context.Filmes
                .Join(_context.Categorias,
                    filme => filme.CategoriaId,
                    categoria => categoria.CategoriaId,
                    (filme, categoria) => new
                    {
                        FilmeTitulo = filme.Titulo,
                        Ano = filme.AnoLancamento,
                        Idade = DateTime.Now.Year - filme.AnoLancamento,
                        Categoria = categoria.Nome,
                        Descricao = categoria.Descricao
                    })
                .OrderBy(x => x.Categoria)
                .ThenBy(x => x.FilmeTitulo)
                .ToListAsync();

            ViewBag.Titulo = "Consulta 1: JOIN entre Filmes e Categorias";
            ViewBag.Descricao = "Demonstra JOIN entre duas tabelas relacionadas";
            return View("ConsultaLinq", resultado);
        }

        // CONSULTA 2: GROUP BY com funções de grupo
        public async Task<IActionResult> Consulta2()
        {
            var resultado = await _context.Filmes
                .Include(f => f.Categoria)
                .GroupBy(f => f.Categoria.Nome)
                .Select(grupo => new
                {
                    Categoria = grupo.Key,
                    QuantidadeFilmes = grupo.Count(),
                    AnoMaisRecente = grupo.Max(f => f.AnoLancamento),
                    AnoMaisAntigo = grupo.Min(f => f.AnoLancamento),
                    MediaAno = grupo.Average(f => f.AnoLancamento),
                    TotalFilmes = grupo.Count()
                })
                .OrderByDescending(g => g.QuantidadeFilmes)
                .ToListAsync();

            ViewBag.Titulo = "Consulta 2: GROUP BY - Filmes por Categoria";
            ViewBag.Descricao = "Demonstra GROUP BY com funções de agregação (COUNT, MAX, MIN, AVG)";
            return View("ConsultaLinq", resultado);
        }

        // CONSULTA 3: WHERE + HAVING
        public async Task<IActionResult> Consulta3()
        {
            var resultado = await _context.Filmes
                .Include(f => f.Produtora)
                .Where(f => f.AnoLancamento < 2000) // WHERE - filtro principal
                .GroupBy(f => f.Produtora.Nome)
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
                        Ano = f.AnoLancamento
                    }).ToList()
                })
                .OrderByDescending(g => g.QuantidadeFilmesAntigos)
                .ToListAsync();

            ViewBag.Titulo = "Consulta 3: WHERE + HAVING - Filmes Antigos por Produtora";
            ViewBag.Descricao = "Demonstra WHERE (filtro individual) + HAVING (filtro de grupo)";
            return View("ConsultaLinq", resultado);
        }
    }
}