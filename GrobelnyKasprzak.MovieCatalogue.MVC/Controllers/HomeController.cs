using GrobelnyKasprzak.MovieCatalogue.Core;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using GrobelnyKasprzak.MovieCatalogue.MVC.Models;
using GrobelnyKasprzak.MovieCatalogue.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GrobelnyKasprzak.MovieCatalogue.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DirectorService _service;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            ReflectionLoader loader = new();
            IDirectorRepository repository = loader.GetRepository<IDirectorRepository>();
            _service = new(repository);

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
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
