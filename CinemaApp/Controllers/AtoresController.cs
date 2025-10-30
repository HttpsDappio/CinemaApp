using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Data;
using CinemaApp.Models;

namespace CinemaApp.Controllers
{
    public class AtoresController : Controller
    {
        private readonly AppDbContext _context;

        public AtoresController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Atores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Atores.ToListAsync());
        }

        // GET: Atores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ator = await _context.Atores
                .FirstOrDefaultAsync(m => m.AtorId == id);
            if (ator == null)
            {
                return NotFound();
            }

            return View(ator);
        }

        // GET: Atores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Atores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AtorId,Nome,Nacionalidade")] Ator ator)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ator);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ator);
        }

        // GET: Atores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ator = await _context.Atores.FindAsync(id);
            if (ator == null)
            {
                return NotFound();
            }
            return View(ator);
        }

        // POST: Atores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AtorId,Nome,Nacionalidade")] Ator ator)
        {
            if (id != ator.AtorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AtorExists(ator.AtorId))
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
            return View(ator);
        }

        // GET: Atores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ator = await _context.Atores
                .FirstOrDefaultAsync(m => m.AtorId == id);
            if (ator == null)
            {
                return NotFound();
            }

            return View(ator);
        }

        // POST: Atores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ator = await _context.Atores.FindAsync(id);
            if (ator != null)
            {
                _context.Atores.Remove(ator);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AtorExists(int id)
        {
            return _context.Atores.Any(e => e.AtorId == id);
        }
    }
}