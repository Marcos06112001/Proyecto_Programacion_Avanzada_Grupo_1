using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Programacion_Grupo_1.Models;

namespace Proyecto_Programacion_Grupo_1.Controllers
{
    public class ClaseController : Controller
    {
        private readonly ProyectoContext _context;

        public ClaseController(ProyectoContext context)
        {
            _context = context;
        }
        // GET: Clase
        public async Task<IActionResult> Index()
        {
            var clases = await _context.Clases.Include(c => c.Academia).ToListAsync();
            return View(clases);
        }

        // GET: Clase/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clase = await _context.Clases.Include(c => c.Academia)
                .FirstOrDefaultAsync(c => c.ClaseID == id);
            if (clase == null)
            {
                return NotFound();
            }

            return View(clase);
        }

        // GET: Clase/Create
        public IActionResult Create()
        {
            ViewData["Academias"] = _context.Academias
                .Select(a => new SelectListItem
                {
                    Value = a.AcademiaID.ToString(),
                    Text = a.Nombre
                })
                .ToList();
            return View();
        }

        // POST: Clase/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Descripcion,FechaHora,AcademiaID")] Clase clase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Academias"] = _context.Academias
                .Select(a => new SelectListItem
                {
                    Value = a.AcademiaID.ToString(),
                    Text = a.Nombre
                })
                .ToList();
            return View(clase);
        }

        // GET: Clase/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clase = await _context.Clases.FindAsync(id);
            if (clase == null)
            {
                return NotFound();
            }

            ViewData["Academias"] = _context.Academias
                .Select(a => new SelectListItem
                {
                    Value = a.AcademiaID.ToString(),
                    Text = a.Nombre
                })
                .ToList();
            return View(clase);
        }

        // POST: Clase/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClaseID,Nombre,Descripcion,FechaHora,AcademiaID")] Clase clase)
        {
            if (id != clase.ClaseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Clases.Any(c => c.ClaseID == id))
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

            ViewData["Academias"] = _context.Academias
                .Select(a => new SelectListItem
                {
                    Value = a.AcademiaID.ToString(),
                    Text = a.Nombre
                })
                .ToList();
            return View(clase);
        }

        // GET: Clase/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clase = await _context.Clases.Include(c => c.Academia)
                .FirstOrDefaultAsync(c => c.ClaseID == id);
            if (clase == null)
            {
                return NotFound();
            }

            return View(clase);
        }

        // POST: Clase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clase = await _context.Clases.FindAsync(id);
            if (clase != null)
            {
                _context.Clases.Remove(clase);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
