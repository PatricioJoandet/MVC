using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Context;
using MVC.Models;

namespace MVC.Controllers
{
    public class TablerosController : Controller
    {
        private readonly WebDatabaseContext _context;

        public TablerosController(WebDatabaseContext context)
        {
            _context = context;
        }

        // GET: Tableros
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tableros.ToListAsync());
        }

        // GET: Tableros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tablero = await _context.Tableros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tablero == null)
            {
                return NotFound();
            }

            return View(tablero);
        }

        // GET: Tableros/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tableros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Subtitulo,Color")] Tablero tablero)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tablero);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tablero);
        }

        // GET: Tableros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tablero = await _context.Tableros.FindAsync(id);
            if (tablero == null)
            {
                return NotFound();
            }
            return View(tablero);
        }

        // POST: Tableros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Subtitulo,Color")] Tablero tablero)
        {
            if (id != tablero.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tablero);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TableroExists(tablero.Id))
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
            return View(tablero);
        }

        // GET: Tableros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tablero = await _context.Tableros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tablero == null)
            {
                return NotFound();
            }

            return View(tablero);
        }

        // POST: Tableros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tablero = await _context.Tableros.FindAsync(id);
            if (tablero != null)
            {
                _context.Tableros.Remove(tablero);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TableroExists(int id)
        {
            return _context.Tableros.Any(e => e.Id == id);
        }
    }
}
