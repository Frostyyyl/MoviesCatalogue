using AutoMapper;
using GrobelnyKasprzak.MovieCatalogue.MVC.Mappings;
using GrobelnyKasprzak.MovieCatalogue.MVC.ViewModels;
using GrobelnyKasprzak.MovieCatalogue.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrobelnyKasprzak.MovieCatalogue.MVC.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly IMapper _mapper;
        private readonly MovieService _movieService = new();
        private readonly StudioService _studioService = new();
        private readonly DirectorService _directorService = new();

        public MoviesController(ILogger<MoviesController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        // GET: MoviesController
        public ActionResult Index()
        {
            return View();
        }

        // GET: MoviesController/Details/5
        public ActionResult Details(int id)
        {
            var movie = _movieService.GetMovieById(id);
            if (movie == null) return NotFound();

            var studio = _studioService.GetStudioById(movie.StudioId);
            var director = _directorService.GetDirectorById(movie.DirectorId);

            var viewModel = _mapper.Map<MovieViewModel>(movie, opt =>
            {
                opt.Items[MappingKeys.StudioName] = studio?.Name;
                opt.Items[MappingKeys.DirectorName] = director?.Name;
            });

            return View(viewModel);
        }

        // GET: MoviesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MoviesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MoviesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MoviesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MoviesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MoviesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
