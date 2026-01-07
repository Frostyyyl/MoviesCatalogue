using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace GrobelnyKasprzak.MovieCatalogue.Services
{
    public class MovieService(IMovieRepository repo) : IMovieService
    {
        private readonly IMovieRepository _repo = repo;

        public IEnumerable<IMovie> GetAllMovies() => _repo.GetAll();

        public IEnumerable<IMovie> GetMoviesByDirectorId(int directorId)
            => _repo.GetByDirectorId(directorId);

        public IMovie? GetMovieById(int id) => _repo.GetById(id);

        public ValidationResult? ValidateMovie(string? title, int? year, int? directorId, int? id = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                return new ValidationResult("Title is required.");

            if (year == null)
                return new ValidationResult("Year is required.");
            else if (year < 1888 || year > DateTime.Now.Year)
                return new ValidationResult("Year must be 1888–current.");

            if (directorId == null || directorId <= 0)
                return new ValidationResult("Please select a Director.");

            var duplicate = _repo.Exists(title: title, year: year.Value, directorId: directorId.Value, excludeId: id);
            if (duplicate)
                return new ValidationResult($"This movie already exists for this director in the year {year}.");

            return null;
        }

        public void AddMovie(IMovie movie)
        {
            ValidationResult? result = ValidateMovie(movie.Title, movie.Year, movie.DirectorId, movie.Id);

            if (result != null)
                throw new InvalidOperationException(result.ErrorMessage);

            _repo.Add(movie);
        }

        public void UpdateMovie(IMovie movie)
        {
            ValidationResult? result = ValidateMovie(movie.Title, movie.Year, movie.DirectorId, movie.Id);

            if (result != null)
                throw new InvalidOperationException(result.ErrorMessage);

            _repo.Update(movie);

        }

        public void DeleteMovie(int id) => _repo.Delete(id);

        public IMovie CreateNewMovie() => _repo.CreateNew();
    }
}
