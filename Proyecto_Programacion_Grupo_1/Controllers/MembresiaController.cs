using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Programacion_Grupo_1.Models;
using System.Threading.Tasks;

namespace Proyecto_Programacion_Grupo_1.Controllers
{
    public class MembresiaController : Controller
    {
        private readonly ProyectoContext _context;

        public MembresiaController(ProyectoContext context)
        {
            _context = context;
        }

        // GET: Membresia
        public async Task<IActionResult> Index()
        {
            var membresias = _context.Membresias.Include(m => m.Usuario);
            return View(await membresias.ToListAsync());
        }

        // GET: Membresia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var membresia = await _context.Membresias
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.MembresiaID == id);

            if (membresia == null) return NotFound();

            return View(membresia);
        }

        // GET: Membresia/Create
        public IActionResult Create()
        {
            ViewBag.Usuarios = new SelectList(_context.Usuarios, "UsuarioID", "Nombre");
            return View();
        }

        // POST: Membresia/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MembresiaID,Tipo,Precio,UsuarioID")] Membresia membresia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(membresia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Usuarios = new SelectList(_context.Usuarios, "UsuarioID", "Nombre", membresia.UsuarioID);
            return View(membresia);
        }

        // GET: Membresia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var membresia = await _context.Membresias.FindAsync(id);
            if (membresia == null) return NotFound();

            ViewBag.Usuarios = new SelectList(_context.Usuarios, "UsuarioID", "Nombre", membresia.UsuarioID);
            return View(membresia);
        }

        // POST: Membresia/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MembresiaID,Tipo,Precio,UsuarioID")] Membresia membresia)
        {
            if (id != membresia.MembresiaID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(membresia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Membresias.Any(e => e.MembresiaID == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Usuarios = new SelectList(_context.Usuarios, "UsuarioID", "Nombre", membresia.UsuarioID);
            return View(membresia);
        }

        // GET: Membresia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var membresia = await _context.Membresias
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.MembresiaID == id);

            if (membresia == null) return NotFound();

            return View(membresia);
        }

        // POST: Membresia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var membresia = await _context.Membresias.FindAsync(id);
            if (membresia != null)
            {
                _context.Membresias.Remove(membresia);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Membresia/VerMembresias
        public async Task<IActionResult> VerMembresias()
        {
            var membresiasDisponibles = await _context.Membresias
                .Include(m => m.Usuario)
                .ToListAsync();

            return View(membresiasDisponibles);
        }
        [HttpPost]
        public IActionResult Adquirir(int membresiaId)
        {
            int usuarioId = ObtenerUsuarioActual();

            var membresia = _context.Membresias.FirstOrDefault(m => m.MembresiaID == membresiaId);
            if (membresia == null)
                return NotFound();

            var compra = new Compra
            {
                UsuarioID = usuarioId,
                MembresiaID = membresia.MembresiaID,
                Cantidad = 1,
                FechaCompra = DateTime.Now
            };

            _context.Compras.Add(compra);
            _context.SaveChanges();

            TempData["Mensaje"] = "¡Membresía adquirida correctamente!";
            return RedirectToAction("VerMembresias");
        }
        private int ObtenerUsuarioActual()
        {
            int? id = HttpContext.Session.GetInt32("UsuarioID");
            if (id == null || !_context.Usuarios.Any(u => u.UsuarioID == id))
            {
                throw new Exception("El usuario actual no es válido o no está autenticado.");
            }

            return id.Value;
        }
    }
}