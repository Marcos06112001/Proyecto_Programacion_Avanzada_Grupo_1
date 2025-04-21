using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Programacion_Grupo_1.Models;

public class ReservaClaseController : Controller
{
    private readonly ProyectoContext _context;

    public ReservaClaseController(ProyectoContext context)
    {
        _context = context;
    }

    // GET: ReservaClase
    public async Task<IActionResult> Index()
    {
        var reservas = await _context.ReservasClase
            .Include(r => r.Usuario)
            .Include(r => r.Clase)
            .ToListAsync();
        return View(reservas);
    }

    // GET: ReservaClase/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var reserva = await _context.ReservasClase
            .Include(r => r.Usuario)
            .Include(r => r.Clase)
            .FirstOrDefaultAsync(r => r.ReservaID == id);
        if (reserva == null)
        {
            return NotFound();
        }

        return View(reserva);
    }

    // GET: ReservaClase/Create
    public IActionResult Create()
    {
        ViewData["Usuarios"] = _context.Usuarios.Select(u => new SelectListItem
        {
            Value = u.UsuarioID.ToString(),
            Text = u.Nombre
        }).ToList();

        ViewData["Clases"] = _context.Clases.Select(c => new SelectListItem
        {
            Value = c.ClaseID.ToString(),
            Text = c.Nombre
        }).ToList();

        return View();
    }

    // POST: ReservaClase/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("UsuarioID,ClaseID,FechaReserva")] ReservaClase reservaClase)
    {
        // Verifica si hay errores en el modelo
        if (!ModelState.IsValid)
        {
            foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Error: {modelError.ErrorMessage}");
            }
        }

        if (ModelState.IsValid)
        {
            _context.Add(reservaClase);
            await _context.SaveChangesAsync();
            Console.WriteLine("Reserva guardada exitosamente");
            return RedirectToAction(nameof(Index));
        }

        ViewData["Usuarios"] = _context.Usuarios.Select(u => new SelectListItem
        {
            Value = u.UsuarioID.ToString(),
            Text = u.Nombre
        }).ToList();

        ViewData["Clases"] = _context.Clases.Select(c => new SelectListItem
        {
            Value = c.ClaseID.ToString(),
            Text = c.Nombre
        }).ToList();

        return View(reservaClase);
    }

    // GET: ReservaClase/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var reservaClase = await _context.ReservasClase.FindAsync(id);
        if (reservaClase == null)
        {
            return NotFound();
        }

        ViewData["Usuarios"] = _context.Usuarios.Select(u => new SelectListItem
        {
            Value = u.UsuarioID.ToString(),
            Text = u.Nombre
        }).ToList();

        ViewData["Clases"] = _context.Clases.Select(c => new SelectListItem
        {
            Value = c.ClaseID.ToString(),
            Text = c.Nombre
        }).ToList();

        return View(reservaClase);
    }

    // POST: ReservaClase/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ReservaID,UsuarioID,ClaseID,FechaReserva")] ReservaClase reservaClase)
    {
        if (id != reservaClase.ReservaID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(reservaClase);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ReservasClase.Any(r => r.ReservaID == id))
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

        ViewData["Usuarios"] = _context.Usuarios.Select(u => new SelectListItem
        {
            Value = u.UsuarioID.ToString(),
            Text = u.Nombre
        }).ToList();

        ViewData["Clases"] = _context.Clases.Select(c => new SelectListItem
        {
            Value = c.ClaseID.ToString(),
            Text = c.Nombre
        }).ToList();

        return View(reservaClase);
    }

    // GET: ReservaClase/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var reservaClase = await _context.ReservasClase
            .Include(r => r.Usuario)
            .Include(r => r.Clase)
            .FirstOrDefaultAsync(r => r.ReservaID == id);
        if (reservaClase == null)
        {
            return NotFound();
        }

        return View(reservaClase);
    }

    // POST: ReservaClase/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var reservaClase = await _context.ReservasClase.FindAsync(id);
        if (reservaClase != null)
        {
            _context.ReservasClase.Remove(reservaClase);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Reservar()
    {
        int? usuarioID = HttpContext.Session.GetInt32("UsuarioID");
        if (usuarioID == null)
        {
            return RedirectToAction("Login", "Usuario");
        }

        ViewBag.UsuarioID = usuarioID;

        var clases = await _context.Clases.Include(c => c.Academia).ToListAsync();
        return View(clases);
    }
    public async Task<IActionResult> MisClases()
    {
        int? usuarioID = HttpContext.Session.GetInt32("UsuarioID");
        if (usuarioID == null)
        {
            return RedirectToAction("Login", "Usuario");
        }

        var reservas = await _context.ReservasClase
            .Include(r => r.Clase)
                .ThenInclude(c => c.Academia)
            .Where(r => r.UsuarioID == usuarioID)
            .ToListAsync();

        return View(reservas);
    }

    [HttpPost]
    public IActionResult ReservarClaseAjax(int claseId)
    {
        try
        {
            int? usuarioID = HttpContext.Session.GetInt32("UsuarioID");

            if (usuarioID == null)
            {
                return Json(new { success = false, message = "Usuario no autenticado." });
            }

            // Verificar si la clase existe
            var clase = _context.Clases.FirstOrDefault(c => c.ClaseID == claseId);
            if (clase == null)
            {
                return Json(new { success = false, message = "Clase no encontrada." });
            }

            // Verificar si ya existe una reserva para este usuario y esta clase
            bool yaReservada = _context.ReservasClase
                .Any(r => r.ClaseID == claseId && r.UsuarioID == usuarioID);

            if (yaReservada)
            {
                return Json(new { success = false, message = "Ya reservaste esta clase." });
            }

            // Crear una nueva reserva
            var nuevaReserva = new ReservaClase
            {
                ClaseID = claseId,
                UsuarioID = usuarioID.Value,
                FechaReserva = DateTime.Now
            };

            _context.ReservasClase.Add(nuevaReserva);
            _context.SaveChanges();

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Ocurrió un error: " + ex.Message });
        }
    }
    [HttpPost]
    public async Task<IActionResult> EliminarDelCarrito(int id)
    {
        var reserva = await _context.ReservasClase.FindAsync(id);
        if (reserva != null)
        {
            _context.ReservasClase.Remove(reserva);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("MisClases");
    }
}