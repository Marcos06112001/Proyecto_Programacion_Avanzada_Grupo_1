using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Programacion_Grupo_1.Models;
using Proyecto_Programacion_Grupo_1.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        // GET: Pago/RealizarPago
        public IActionResult RealizarPago()
        {
            // Obtener carrito de la sesión usando tu helper
            var carrito = HttpContext.Session.GetObject<List<CarritoItem>>("Carrito") ?? new List<CarritoItem>();

            if (!carrito.Any())
            {
                TempData["ErrorMessage"] = "No hay items en el carrito para procesar el pago";
                return RedirectToAction("VerCarrito", "Carrito");
            }

            // Calcular total
            decimal total = carrito.Sum(item => item.Subtotal);

            // Crear modelo de pago
            var pago = new Pago
            {
                Monto = total,
                FechaPago = DateTime.Now
            };

            // Configurar métodos de pago
            ViewBag.MetodosPago = new List<SelectListItem>
            {
                new SelectListItem { Value = "Tarjeta", Text = "Tarjeta de Crédito/Débito" },
                new SelectListItem { Value = "PayPal", Text = "PayPal" },
                new SelectListItem { Value = "Transferencia", Text = "Transferencia Bancaria" }
            };

            // Pasar total a la vista
            ViewBag.TotalCarrito = total;

            return View("RealizarPago", pago);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RealizarPago([Bind("MetodoPago")] Pago pago)
        {
            // Obtener carrito de la sesión
            var carrito = HttpContext.Session.GetObject<List<CarritoItem>>("Carrito");

            if (carrito == null || !carrito.Any())
            {
                TempData["ErrorMessage"] = "No hay items en el carrito para procesar el pago";
                return RedirectToAction("VerCarrito", "Carrito");
            }

            // Calcular total
            decimal total = carrito.Sum(item => item.Subtotal);
            var usuarioId = ObtenerUsuarioId();

            if (ModelState.IsValid)
            {
                try
                {
                    // Crear registro de pago
                    var nuevoPago = new Pago
                    {
                        UsuarioID = usuarioId,
                        Monto = total,
                        MetodoPago = pago.MetodoPago,
                        FechaPago = DateTime.Now
                    };

                    _context.Pagos.Add(nuevoPago);
                    await _context.SaveChangesAsync();

                    // Limpiar carrito después del pago exitoso
                    HttpContext.Session.Remove("Carrito");

                    TempData["SuccessMessage"] = $"¡Pago de ₡{total:N2} procesado exitosamente!";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error al procesar el pago: " + ex.Message);
                }
            }

            // Si hay errores, recargar vista con datos
            ViewBag.MetodosPago = new List<SelectListItem>
    {
        new SelectListItem { Value = "Sinpe", Text = "Sinpe" },
        new SelectListItem { Value = "PayPal", Text = "PayPal" },
        new SelectListItem { Value = "Transferencia", Text = "Transferencia Bancaria" }
    };

            ViewBag.TotalCarrito = total;

            return View("RealizarPago", pago);
        }



        private int ObtenerUsuarioId()
        {
            if (HttpContext.Session.GetInt32("UsuarioID") is int usuarioId)
            {
                return usuarioId;
            }
            throw new UnauthorizedAccessException("Usuario no autenticado");
        }

    }
}