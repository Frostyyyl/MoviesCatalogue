using AutoMapper;
using GrobelnyKasprzak.MovieCatalogue.MVC.Models.Dto;
using GrobelnyKasprzak.MovieCatalogue.MVC.Services;
using GrobelnyKasprzak.MovieCatalogue.MVC.ViewModels;
using GrobelnyKasprzak.MovieCatalogue.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GrobelnyKasprzak.MovieCatalogue.MVC.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly IMapper _mapper;
        private readonly ILookupService _lookupService;
        private readonly MovieService _movieService = new();
        private readonly DirectorService _directorService = new();

        public MoviesController(ILogger<MoviesController> logger, IMapper mapper, ILookupService lookupService)
        {
            _logger = logger;
            _mapper = mapper;
            _lookupService = lookupService;
        }

        // GET: MoviesController
        public ActionResult Index(string? search)
        {
            var movies = _movieService.GetAllMovies();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();

                movies = [.. movies.Where(m =>
                    m.Title.Contains(search, StringComparison.CurrentCultureIgnoreCase))];
            }

            var viewModel = _mapper.Map<List<MovieViewModel>>(movies);
            SetDirectorNames(viewModel);

            @ViewData["Search"] = search;

            return View(viewModel);
        }

        // GET: MoviesController/Details/5
        public ActionResult Details(int id)
        {
            var movie = _movieService.GetMovieById(id);
            if (movie == null) return NotFound();

            var viewModel = _mapper.Map<MovieViewModel>(movie);
            SetDirectorName(viewModel);

            return View(viewModel);
        }

        // GET: MoviesController/Create
        public ActionResult Create()
        {
            var newMovie = _movieService.CreateNewMovie();

            var viewModel = _mapper.Map<MovieViewModel>(newMovie);
            PopulateViewModel(viewModel);

            return View(viewModel);
        }

        // POST: MoviesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MovieViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns(model);
                return View(model);
            }

            try
            {
                var movie = _mapper.Map<MovieDto>(model);
                _movieService.AddMovie(movie);

                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);

                PopulateDropdowns(model);
                return View(model);
            }
        }

        // GET: MoviesController/Edit/5
        public ActionResult Edit(int id)
        {
            var movie = _movieService.GetMovieById(id);
            if (movie == null) return NotFound();

            var viewModel = _mapper.Map<MovieViewModel>(movie);
            PopulateViewModel(viewModel);

            return View(viewModel);
        }

        // POST: MoviesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, MovieViewModel model)
        {
            if (id != model.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                PopulateDropdowns(model);
                return View(model);
            }

            try
            {
                var movieToUpdate = _mapper.Map<MovieDto>(model);
                _movieService.UpdateMovie(movieToUpdate);

                return RedirectToAction(nameof(Details), new { id });
            }
            catch (ValidationException exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);

                PopulateDropdowns(model);
                return View(model);
            }
        }


        //GET: MoviesController/Delete/5
        public ActionResult Delete(int id)
        {
            var movie = _movieService.GetMovieById(id);
            if (movie == null) return NotFound();

            return View(movie);
        }

        // POST: MoviesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, MovieViewModel model)
        {
            _movieService.DeleteMovie(id);

            return RedirectToAction(nameof(Index));
        }

        private void SetDirectorName(MovieViewModel model)
        {
            var director = _directorService.GetDirectorById(model.DirectorId);
            model.Director = director?.Name ?? "Unknown Director";
        }

        private void SetDirectorNames(IEnumerable<MovieViewModel> models)
        {
            foreach (var model in models)
            {
                SetDirectorName(model);
            }
        }

        private void PopulateDropdowns(MovieViewModel model)
        {
            model.AvailableDirectors = _mapper.Map<IEnumerable<SelectListItem>>
                (_directorService.GetAllDirectors());
            model.AvailableGenres = _lookupService.GetGenreSelectList();
        }

        private void PopulateViewModel(MovieViewModel model)
        {
            SetDirectorName(model);
            PopulateDropdowns(model);
        }
    }
}
