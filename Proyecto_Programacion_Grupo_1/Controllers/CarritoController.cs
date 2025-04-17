using Microsoft.AspNetCore.Mvc;
using Proyecto_Programacion_Grupo_1.Models;
using System.Text.Json;


namespace Proyecto_Programacion_Grupo_1.Controllers
{
    public class CarritoController : Controller
    {
        private const string CarritoSessionKey = "Carrito";
        private readonly ProyectoContext _context;

        public CarritoController(ProyectoContext context)
        {
            _context = context;
        }

        private CarritoCompra GetCarrito()
        {
            var carritoJson = HttpContext.Session.GetString(CarritoSessionKey);
            return carritoJson == null ? new CarritoCompra() :
                   JsonSerializer.Deserialize<CarritoCompra>(carritoJson);
        }

        private void SaveCarrito(CarritoCompra carrito)
        {
            HttpContext.Session.SetString(CarritoSessionKey, JsonSerializer.Serialize(carrito));
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(int productoId, int cantidad = 1)
        {
            var producto = await _context.Productos.FindAsync(productoId);
            if (producto == null || producto.Stock < cantidad)
            {
                return NotFound();
            }

            var carrito = GetCarrito();
            var item = carrito.Items.FirstOrDefault(i => i.ProductoID == productoId);

            if (item != null)
            {
                item.Cantidad += cantidad;
            }
            else
            {
                carrito.Items.Add(new CarritoItem
                {
                    ProductoID = productoId,
                    Producto = producto,
                    Cantidad = cantidad
                });
            }

            SaveCarrito(carrito);
            return RedirectToAction("Catalogo", "Producto");
        }

        public IActionResult Index()
        {
            var carrito = GetCarrito();
            return View(carrito);
        }

        [HttpPost]
        public IActionResult Eliminar(int productoId)
        {
            var carrito = GetCarrito();
            var item = carrito.Items.FirstOrDefault(i => i.ProductoID == productoId);

            if (item != null)
            {
                carrito.Items.Remove(item);
                SaveCarrito(carrito);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ActualizarCantidad(int productoId, int cantidad)
        {
            if (cantidad <= 0)
                return RedirectToAction(nameof(Index));

            var carrito = GetCarrito();
            var item = carrito.Items.FirstOrDefault(i => i.ProductoID == productoId);

            if (item != null)
            {
                item.Cantidad = cantidad;
                SaveCarrito(carrito);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
