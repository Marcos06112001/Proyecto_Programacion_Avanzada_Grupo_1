using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Programacion_Grupo_1.Models;

namespace Proyecto_Programacion_Grupo_1.Controllers
{
    public class CompraController : Controller
    {
        private readonly ProyectoContext _context;

        public CompraController(ProyectoContext context)
        {
            _context = context;
        }

        // GET: Compra
        public async Task<IActionResult> Index()
        {
            var compras = await _context.Compras
                .Include(c => c.Usuario)
                .Include(c => c.Producto)
                .ToListAsync();
            return View(compras);
        }

        // GET: Compra/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .Include(c => c.Usuario)
                .Include(c => c.Producto)
                .FirstOrDefaultAsync(m => m.CompraID == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // GET: Compra/Create
        public IActionResult Create()
        {
            ViewData["UsuarioID"] = new SelectList(_context.Usuarios, "UsuarioID", "Nombre");
            ViewData["ProductoID"] = new SelectList(_context.Productos, "ProductoID", "Nombre");
            ViewData["UsuarioID"] = new SelectList(_context.Usuarios, "UsuarioID", "Nombre");
            return View();
        }

        // POST: Compra/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompraID,UsuarioID,ProductoID,Cantidad,FechaCompra")] Compra compra)
        {
            if (ModelState.IsValid)
            {
                // Verificar stock antes de realizar la compra
                var producto = await _context.Productos.FindAsync(compra.ProductoID);
                if (producto == null || producto.Stock < compra.Cantidad)
                {
                    ModelState.AddModelError("", "No hay suficiente stock disponible");
                    ViewData["UsuarioID"] = new SelectList(_context.Usuarios, "UsuarioID", "Nombre", compra.UsuarioID);
                    ViewData["ProductoID"] = new SelectList(_context.Productos, "ProductoID", "Nombre", compra.ProductoID);
                    return View(compra);
                }

                // Actualizar stock
                producto.Stock -= compra.Cantidad;
                _context.Update(producto);

                // Asignar fecha actual si no viene especificada
                if (compra.FechaCompra == default)
                {
                    compra.FechaCompra = DateTime.Now;
                }

                _context.Add(compra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioID"] = new SelectList(_context.Usuarios, "UsuarioID", "Nombre", compra.UsuarioID);
            ViewData["ProductoID"] = new SelectList(_context.Productos, "ProductoID", "Nombre", compra.ProductoID);
            return View(compra);
        }

        // GET: Compra/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }
            ViewData["UsuarioID"] = new SelectList(_context.Usuarios, "UsuarioID", "Nombre", compra.UsuarioID);
            ViewData["ProductoID"] = new SelectList(_context.Productos, "ProductoID", "Nombre", compra.ProductoID);
            return View(compra);
        }

        // POST: Compra/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompraID,UsuarioID,ProductoID,Cantidad,FechaCompra")] Compra compra)
        {
            if (id != compra.CompraID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraExists(compra.CompraID))
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
            ViewData["UsuarioID"] = new SelectList(_context.Usuarios, "UsuarioID", "Nombre", compra.UsuarioID);
            ViewData["ProductoID"] = new SelectList(_context.Productos, "ProductoID", "Nombre", compra.ProductoID);
            return View(compra);
        }

        // GET: Compra/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .Include(c => c.Usuario)
                .Include(c => c.Producto)
                .FirstOrDefaultAsync(m => m.CompraID == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // POST: Compra/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compra = await _context.Compras.FindAsync(id);
            if (compra != null)
            {
                _context.Compras.Remove(compra);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CompraExists(int id)
        {
            return _context.Compras.Any(e => e.CompraID == id);
        }
        // GET: Compra/Tienda
        public async Task<IActionResult> Tienda()
        {
            var productos = await _context.Productos.Where(p => p.Stock > 0).ToListAsync();
            var membresias = await _context.Membresias.ToListAsync();

            // Solución: convertir a IEnumerable explícitamente
            IEnumerable<Producto> productosEnumerable = productos;
            IEnumerable<Membresia> membresiasEnumerable = membresias;

            return View(Tuple.Create(productosEnumerable, membresiasEnumerable));
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(int productoId, int cantidad, string tipo = "producto")
        {
            var usuarioId = 2; //  Reemplazar por el ID real del usuario logueado

            if (tipo == "producto")
            {
                var producto = await _context.Productos.FindAsync(productoId);
                if (producto == null || producto.Stock < cantidad)
                {
                    TempData["Error"] = "Producto no disponible o stock insuficiente.";
                    return RedirectToAction("Tienda");
                }

                var compra = new Compra
                {
                    UsuarioID = usuarioId,
                    ProductoID = productoId,
                    Cantidad = cantidad,
                    FechaCompra = DateTime.Now
                };

                producto.Stock -= cantidad;
                _context.Update(producto);

                _context.Compras.Add(compra);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "¡Producto comprado con éxito!";
            }
            else if (tipo == "membresia")
            {
                var membresia = await _context.Membresias.FindAsync(productoId);
                if (membresia == null)
                {
                    TempData["Error"] = "Membresía no encontrada.";
                    return RedirectToAction("Tienda");
                }

                var compra = new Compra
                {
                    UsuarioID = usuarioId,
                    ProductoID = productoId, // Usamos el mismo campo para simplificar
                    Cantidad = 1,
                    FechaCompra = DateTime.Now
                };

                _context.Compras.Add(compra);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "¡Membresía comprada con éxito!";
            }

            return RedirectToAction("Index");
        }
    }
}