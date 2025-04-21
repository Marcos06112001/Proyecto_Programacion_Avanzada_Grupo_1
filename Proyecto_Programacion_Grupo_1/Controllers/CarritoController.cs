using Microsoft.AspNetCore.Mvc;
using Proyecto_Programacion_Grupo_1.Models;
using System.Text.Json;
using Proyecto_Programacion_Grupo_1.Helpers;


namespace Proyecto_Programacion_Grupo_1.Controllers
{
    public class CarritoController : Controller
    {
        private readonly ProyectoContext _context;

        public CarritoController(ProyectoContext context)
        {
            _context = context;
        }

        public IActionResult VerCarrito()
        {
            var carrito = HttpContext.Session.GetObject<List<CarritoItem>>("Carrito") ?? new List<CarritoItem>();
            return View(carrito);
        }

        [HttpPost]
        public IActionResult AgregarProducto(int productoId, int cantidad)
        {
            var producto = _context.Productos.Find(productoId);
            if (producto == null)
            {
                return NotFound();
            }

            var carrito = HttpContext.Session.GetObject<List<CarritoItem>>("Carrito") ?? new List<CarritoItem>();

            var itemExistente = carrito.FirstOrDefault(ci => ci.ProductoID == productoId && ci.MembresiaID == null);
            if (itemExistente != null)
            {
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                carrito.Add(new CarritoItem
                {
                    ProductoID = productoId,
                    Producto = producto,
                    Cantidad = cantidad
                });
            }

            HttpContext.Session.SetObject("Carrito", carrito);
            return RedirectToAction("VerCarrito");
        }

        [HttpPost]
        public IActionResult AgregarMembresia(int membresiaId, int cantidad)
        {
            var membresia = _context.Membresias.Find(membresiaId);
            if (membresia == null)
            {
                return NotFound();
            }

            var carrito = HttpContext.Session.GetObject<List<CarritoItem>>("Carrito") ?? new List<CarritoItem>();

            var itemExistente = carrito.FirstOrDefault(ci => ci.MembresiaID == membresiaId && ci.ProductoID == null);
            if (itemExistente != null)
            {
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                carrito.Add(new CarritoItem
                {
                    MembresiaID = membresiaId,
                    Membresia = membresia,
                    Cantidad = cantidad
                });
            }

            HttpContext.Session.SetObject("Carrito", carrito);
            return RedirectToAction("VerCarrito");
        }

        [HttpPost]
        public IActionResult EliminarDelCarrito(int itemIndex)
        {
            var carrito = HttpContext.Session.GetObject<List<CarritoItem>>("Carrito") ?? new List<CarritoItem>();

            if (itemIndex >= 0 && itemIndex < carrito.Count)
            {
                carrito.RemoveAt(itemIndex);
                HttpContext.Session.SetObject("Carrito", carrito);
            }

            return RedirectToAction("VerCarrito");
        }
    }
}
