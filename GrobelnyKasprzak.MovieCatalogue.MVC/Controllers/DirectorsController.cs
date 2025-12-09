using AutoMapper;
using GrobelnyKasprzak.MovieCatalogue.MVC.Mappings;
using GrobelnyKasprzak.MovieCatalogue.MVC.Models.Dto;
using GrobelnyKasprzak.MovieCatalogue.MVC.ViewModels;
using GrobelnyKasprzak.MovieCatalogue.Services;
using Microsoft.AspNetCore.Mvc;

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

            var viewModel = new List<DirectorViewModel>();

            foreach (var director in directors)
            {
                var movies = _movieService.GetMoviesByDirectorId(director.Id);

                viewModel.Add(_mapper.Map<DirectorViewModel>(director, opt =>
                {
                    opt.Items[MappingKeys.Movies] = movies;
                }));
            }

            @ViewData["Search"] = search;

            return View(viewModel);
        }

        // GET: DirectorController/Details/5
        public ActionResult Details(int id)
        {
            var director = _directorService.GetDirectorById(id);
            if (director == null) return NotFound();

            var movies = _movieService.GetMoviesByDirectorId(director.Id);

            var viewModel = _mapper.Map<DirectorViewModel>(director, opt =>
            {
                opt.Items[MappingKeys.Movies] = movies;
            });

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

            var director = _mapper.Map<DirectorDto>(model);
            _directorService.AddDirector(director);

            return RedirectToAction(nameof(Index));
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
        public ActionResult Edit(int id, DirectorViewModel viewModel)
        {
            if (id != viewModel.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var directorToUpdate = _mapper.Map<DirectorDto>(viewModel);
            _directorService.UpdateDirector(directorToUpdate);

            return RedirectToAction(nameof(Details), new { id });
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
    }
}
