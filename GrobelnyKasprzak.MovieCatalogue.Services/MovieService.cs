using GrobelnyKasprzak.MovieCatalogue.Core;
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

        public ValidationResult? ValidateMovie(string? title, int? year, int? directorId, MovieGenre? genre = null, int? id = null)
        {
            ValidationResult? result;

            result = ValidateTitle(title);
            if (result != null) return result;

            result = ValidateYear(year);
            if (result != null) return result;

            result = ValidateDirector(directorId);
            if (result != null) return result;

            result = ValidateGenre(genre);
            if (result != null) return result;

            result = ValidateDuplicate(id, title!, year!.Value, directorId!.Value);
            if (result != null) return result;

            return null;
        }

        public void AddMovie(IMovie movie)
        {
            ValidationResult? result = ValidateMovie(movie.Title, movie.Year, movie.DirectorId, movie.Genre, movie.Id);

            if (result != null)
                throw new InvalidOperationException(result.ErrorMessage);

            _repo.Add(movie);
        }

        public void UpdateMovie(IMovie movie)
        {
            ValidationResult? result = ValidateMovie(movie.Title, movie.Year, movie.DirectorId, movie.Genre, movie.Id);

            if (result != null)
                throw new InvalidOperationException(result.ErrorMessage);

            _repo.Update(movie);

        }

        public void DeleteMovie(int id) => _repo.Delete(id);

        public IMovie CreateNewMovie() => _repo.CreateNew();

        public ValidationResult? ValidateTitle(string? title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return new ValidationResult(
                    "Title is required.",
                    [nameof(IMovie.Title)]);

            if (title.Length < 2 || title.Length > 100)
                return new ValidationResult(
                    "Title must be between 2 and 100 characters.",
                    [nameof(IMovie.Title)]);

            return null;
        }

        public ValidationResult? ValidateYear(int? year)
        {
            if (year == null)
                return new ValidationResult(
                    "Year is required.",
                    [nameof(IMovie.Year)]);

            if (year < 1888 || year > DateTime.Now.Year)
                return new ValidationResult(
                    "Year must be between 1888 and current.",
                    [nameof(IMovie.Year)]);

            return null;
        }

        public ValidationResult? ValidateDirector(int? directorId)
        {
            if (directorId == null || directorId <= 0)
                return new ValidationResult(
                    "Please select a Director.",
                    [nameof(IMovie.DirectorId)]);

            return null;
        }

        public ValidationResult? ValidateGenre(MovieGenre? genre)
        {
            if (genre == null)
                return new ValidationResult(
                    "Genre is required.",
                    [nameof(IMovie.Genre)]);

            return null;
        }

        public ValidationResult? ValidateDuplicate(int? id, string title, int year, int directorId)
        {
            if (_repo.Exists(id, title, year, directorId: directorId))
                return new ValidationResult(
                    $"This movie already exists for this director in the year {year}.");

            return null;
        }
    }
}
