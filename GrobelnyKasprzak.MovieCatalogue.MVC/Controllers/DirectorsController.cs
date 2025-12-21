using AutoMapper;
using GrobelnyKasprzak.MovieCatalogue.MVC.Models.Dto;
using GrobelnyKasprzak.MovieCatalogue.MVC.ViewModels;
using GrobelnyKasprzak.MovieCatalogue.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GrobelnyKasprzak.MovieCatalogue.MVC.Controllers
{
    public class DirectorsController : Controller
    {
        private readonly ILogger<DirectorsController> _logger;
        private readonly IMapper _mapper;
        private readonly DirectorService _directorService = new();
        private readonly MovieService _movieService = new();

        public DirectorsController(ILogger<DirectorsController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        // GET: DirectorController
        public ActionResult Index(string? search)
        {
            var directors = _directorService.GetAllDirectors();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();

                directors = [.. directors.Where(m =>
                    m.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase))];
            }

            var viewModel = _mapper.Map<List<DirectorViewModel>>(directors);
            SetMovies(viewModel);

            @ViewData["Search"] = search;

            return View(viewModel);
        }

        // GET: DirectorController/Details/5
        public ActionResult Details(int id)
        {
            var director = _directorService.GetDirectorById(id);
            if (director == null) return NotFound();

            var viewModel = _mapper.Map<DirectorViewModel>(director);
            SetMovies(viewModel);

            return View(viewModel);
        }

        // GET: DirectorController/Create
        public ActionResult Create()
        {
            var newDirector = _directorService.CreateNewDirector();

            var viewModel = _mapper.Map<DirectorViewModel>(newDirector);

            return View(viewModel);
        }

        // POST: DirectorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DirectorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var director = _mapper.Map<DirectorDto>(model);
                _directorService.AddDirector(director);

                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);

                return View(model);
            }
        }

        // GET: DirectorController/Edit/5
        public ActionResult Edit(int id)
        {
            var newDirector = _directorService.GetDirectorById(id);

            var viewModel = _mapper.Map<DirectorViewModel>(newDirector);

            return View(viewModel);
        }

        // POST: DirectorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, DirectorViewModel model)
        {
            if (id != model.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var directorToUpdate = _mapper.Map<DirectorDto>(model);
                _directorService.UpdateDirector(directorToUpdate);

                return RedirectToAction(nameof(Details), new { id });
            }
            catch (ValidationException exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);

                return View(model);
            }
        }

        // GET: DirectorController/Delete/5
        public ActionResult Delete(int id)
        {
            var director = _directorService.GetDirectorById(id);
            if (director == null) return NotFound();

            return View(director);
        }

        // POST: DirectorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            _directorService.DeleteDirector(id);

            return RedirectToAction(nameof(Index));
        }

        private void SetMovies(DirectorViewModel model)
        {
            model.Movies = _mapper.Map<IEnumerable<MovieListItemViewModel>>
                (_movieService.GetMoviesByDirectorId(model.Id));
        }

        private void SetMovies(IEnumerable<DirectorViewModel> models)
        {
            foreach (var model in models)
            {
                SetMovies(model);
            }
        }
    }
}
