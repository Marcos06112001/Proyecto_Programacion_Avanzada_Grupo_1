using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Programacion_Grupo_1.Models;

namespace Proyecto_Programacion_Grupo_1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Inicio()
        {
            return View(); // Regresa la vista Inicio.cshtml
        }
        public IActionResult Welcome()
        {
            return View();
        }

        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult Userr()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
