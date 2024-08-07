﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Context;
using MVC.Models;

namespace MVC.Controllers
{
    [Authorize]
    public class TareasController : Controller
    {
        private readonly WebDatabaseContext _context;

        public TareasController(WebDatabaseContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CambiarEstado(int id)
        {
            var tarea = _context.Tareas.FirstOrDefault(t => t.Id == id);
            if(tarea != null)
            {
                tarea.Completa = !tarea.Completa;
                _context.SaveChanges();
                return Ok(new { message = "Tarea actualizada" });
            }
            return NotFound();
        }

        // GET: Tareas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tareas.ToListAsync());
        }

        // GET: Tareas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarea = await _context.Tareas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tarea == null)
            {
                return NotFound();
            }

            return View(tarea);
        }

        // GET: Tareas/Create
        public IActionResult Create(int tableroId)
        {
            ViewBag.TableroId = tableroId;
            return View();
        }

        // POST: Tareas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,FechaLimite,Completa")] Tarea tarea, int tableroId)
        {
            if (ModelState.IsValid)
            {
                tarea.tableroId = tableroId;

                _context.Add(tarea);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Tableros");
            }
            return View(tarea);
        }

        // GET: Tareas/Edit/5
        public async Task<IActionResult> Edit(int? id, int? tableroId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null)
            {
                return NotFound();
            }

            ViewBag.TableroId = tableroId;
            return View(tarea);
        }

        // POST: Tareas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,FechaLimite,Completa")] Tarea tarea, int tableroId)
        {
            if (id != tarea.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tarea.tableroId = tableroId;
                    _context.Update(tarea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TareaExists(tarea.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Tableros");
            }
            return View(tarea);
        }

        // GET: Tareas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarea = await _context.Tareas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tarea == null)
            {
                return NotFound();
            }

            return View(tarea);
        }

        // POST: Tareas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea != null)
            {
                _context.Tareas.Remove(tarea);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Tableros");
        }

        public async Task EliminarTareasPorTablero(int tableroId)
        {
            var tareas = await _context.Tareas.Where(t => t.tableroId == tableroId).ToListAsync();
            if (tareas.Count > 0)
            {
                _context.Tareas.RemoveRange(tareas);
                await _context.SaveChangesAsync();
            }
        }

        private bool TareaExists(int id)
        {
            return _context.Tareas.Any(e => e.Id == id);
        }
    }
}
