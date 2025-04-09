using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Programacion_Grupo_1.Models;
using System.Linq;

namespace Proyecto_Programacion_Grupo_1.Controllers
{
    public class RolController : Controller
    {
        private readonly ProyectoContext _context;

        public RolController(ProyectoContext context)
        {
            _context = context;
        }

        // GET: Rol/Seleccionar
        public IActionResult Seleccionar()
        {
            return View();
        }

        // POST: Rol/Seleccionar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Seleccionar(string correo, string contraseña, string rol)
        {
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contraseña))
            {
                ModelState.AddModelError(string.Empty, "Por favor ingrese todos los campos.");
                return View(); // Si no se ingresa correo o contraseña
            }

            // Buscar el usuario en la base de datos
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == correo && u.Contraseña == contraseña);

            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "Correo o Contraseña incorrectos.");
                return View(); // Si las credenciales no son correctas
            }

            // Verificar si el rol seleccionado es Admin (Administrador) o Usuario
            if (usuario.Rol == "Admin" && rol == "Administrador")
            {
                // Si es administrador, redirigir al administrador
                return RedirectToAction("Index", "Home"); // Cambiar a la vista del administrador
            }
            // Verificar si el rol seleccionado es Usuario
            else if (usuario.Rol == "Usuario" && rol == "Usuario")
            {
                // Si es usuario, redirigir a la página de usuario
                return RedirectToAction("UsuariosPagina", "Rol");
            }

            ModelState.AddModelError(string.Empty, "Rol no válido para este usuario.");
            return View(); // Si el rol no corresponde
        }

        // GET: Rol/UsuariosPagina
        public IActionResult UsuariosPagina()
        {
            return View();
        }
    }
}
