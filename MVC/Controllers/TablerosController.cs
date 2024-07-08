using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Context;
using MVC.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MVC.Controllers
{
    [Authorize]
    public class TablerosController : Controller
    {
        private readonly WebDatabaseContext _context;
        private readonly TareasController _tareasController;

        public TablerosController(WebDatabaseContext context, TareasController tareasController)
        {
            _context = context;
            _tareasController = tareasController;
        }

        // GET: Tableros
        public async Task<IActionResult> Index()
        {

            var userId = HttpContext.Session.GetInt32("UserId");

            var tableros = await _context.Tableros.Where(t => t.userId == userId).ToListAsync();
            var tableroIds = tableros.Select(tb => tb.Id).ToArray();

            // Si no hay tableros, devuelve una lista vacía de tareas
            List<Tarea> tareas = new List<Tarea>();
            if (tableroIds.Any())
            {
                // Construir una consulta básica usando SQL sin Entity Framework
                var ids = string.Join(",", tableroIds);
                var query = $"SELECT * FROM Tareas WHERE tableroId IN ({ids})";
                tareas = await _context.Tareas.FromSqlRaw(query).ToListAsync();
            }

            ViewBag.Tableros = tableros;
            ViewBag.Tareas = tareas;

            return View();
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
                var userId = HttpContext.Session.GetInt32("UserId");
                tablero.userId = userId.Value;

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
                    var userId = HttpContext.Session.GetInt32("UserId");
                    tablero.userId = userId.Value;
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
                await _tareasController.EliminarTareasPorTablero(id);
                _context.Tableros.Remove(tablero);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task EliminarTablerosPorUser(int id)
        {
            var tableros = await _context.Tableros.Where(t => t.Id == id).ToListAsync();
            if (tableros.Count > 0)
            {
                _context.Tableros.RemoveRange(tableros);
                await _context.SaveChangesAsync();
            }
        }

        private bool TableroExists(int id)
        {
            return _context.Tableros.Any(e => e.Id == id);
        }
    }
}
