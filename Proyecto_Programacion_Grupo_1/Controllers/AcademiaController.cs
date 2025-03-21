using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Programacion_Grupo_1.Models;

namespace Proyecto_Programacion_Grupo_1.Controllers
{
    public class AcademiaController : Controller
    {
        private readonly ProyectoContext _context;

        public AcademiaController(ProyectoContext context)
        {
            _context = context;
        }
        // GET: Academia
        public async Task<IActionResult> Index()
        {
            var academias = await _context.Academias.ToListAsync();
            return View(academias);
        }

        // GET: Academia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academia = await _context.Academias
                .Include(a => a.Clases)
                .FirstOrDefaultAsync(a => a.AcademiaID == id);

            if (academia == null)
            {
                return NotFound();
            }

            return View(academia);
        }

        // GET: Academia/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Academia/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AcademiaID,Nombre,Ubicacion,Horario")] Academia academia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(academia);
                await _context.SaveChangesAsync(); 
                return RedirectToAction(nameof(Index)); 
            }
            return View(academia); 
        }

        // GET: Academia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academia = await _context.Academias.FindAsync(id);
            if (academia == null)
            {
                return NotFound();
            }
            return View(academia);
        }

        // POST: Academia/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AcademiaID,Nombre,Ubicacion,Horario")] Academia academia)
        {
            if (id != academia.AcademiaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(academia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Academias.Any(a => a.AcademiaID == id))
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
            return View(academia);
        }

        // GET: Academia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academia = await _context.Academias
                .FirstOrDefaultAsync(a => a.AcademiaID == id);
            if (academia == null)
            {
                return NotFound();
            }

            return View(academia);
        }

        // POST: Academia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var academia = await _context.Academias.FindAsync(id);
            if (academia != null)
            {
                _context.Academias.Remove(academia);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
