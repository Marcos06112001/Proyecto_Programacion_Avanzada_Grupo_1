using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Programacion_Grupo_1.Models;

namespace Proyecto_Programacion_Grupo_1.Controllers
{
    public class PagoController : Controller
    {
        private readonly ProyectoContext _context;

        public PagoController(ProyectoContext context)
        {
            _context = context;
        }

        // GET: Pago
        public async Task<IActionResult> Index()
        {
            var pagos = await _context.Pagos
                .Include(p => p.Usuario)
                .ToListAsync();
            return View(pagos);
        }

        // GET: Pago/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.PagoID == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // GET: Pago/Create
        public IActionResult Create()
        {
            ViewData["UsuarioID"] = new SelectList(_context.Usuarios, "UsuarioID", "Nombre");
            ViewBag.MetodosPago = new List<SelectListItem>
            {
                new SelectListItem { Value = "Tarjeta", Text = "Tarjeta de Crédito/Débito" },
                new SelectListItem { Value = "PayPal", Text = "PayPal" },
                new SelectListItem { Value = "Transferencia", Text = "Transferencia Bancaria" }
            };
            return View();
        }

        // POST: Pago/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PagoID,UsuarioID,Monto,MetodoPago,FechaPago")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                // Asignar fecha actual si no viene especificada
                if (pago.FechaPago == default)
                {
                    pago.FechaPago = DateTime.Now;
                }

                _context.Add(pago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioID"] = new SelectList(_context.Usuarios, "UsuarioID", "Nombre", pago.UsuarioID);
            ViewBag.MetodosPago = new List<SelectListItem>
            {
                new SelectListItem { Value = "Tarjeta", Text = "Tarjeta de Crédito/Débito" },
                new SelectListItem { Value = "PayPal", Text = "PayPal" },
                new SelectListItem { Value = "Transferencia", Text = "Transferencia Bancaria" }
            };
            return View(pago);
        }

        // GET: Pago/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }
            ViewData["UsuarioID"] = new SelectList(_context.Usuarios, "UsuarioID", "Nombre", pago.UsuarioID);
            ViewBag.MetodosPago = new List<SelectListItem>
            {
                new SelectListItem { Value = "Tarjeta", Text = "Tarjeta de Crédito/Débito" },
                new SelectListItem { Value = "PayPal", Text = "PayPal" },
                new SelectListItem { Value = "Transferencia", Text = "Transferencia Bancaria" }
            };
            return View(pago);
        }

        // POST: Pago/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PagoID,UsuarioID,Monto,MetodoPago,FechaPago")] Pago pago)
        {
            if (id != pago.PagoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagoExists(pago.PagoID))
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
            ViewData["UsuarioID"] = new SelectList(_context.Usuarios, "UsuarioID", "Nombre", pago.UsuarioID);
            ViewBag.MetodosPago = new List<SelectListItem>
            {
                new SelectListItem { Value = "Tarjeta", Text = "Tarjeta de Crédito/Débito" },
                new SelectListItem { Value = "PayPal", Text = "PayPal" },
                new SelectListItem { Value = "Transferencia", Text = "Transferencia Bancaria" }
            };
            return View(pago);
        }

        // GET: Pago/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.PagoID == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // POST: Pago/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago != null)
            {
                _context.Pagos.Remove(pago);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PagoExists(int id)
        {
            return _context.Pagos.Any(e => e.PagoID == id);
        }
    }
}