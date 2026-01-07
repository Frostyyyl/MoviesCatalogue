using GrobelnyKasprzak.MovieCatalogue.Core;
using GrobelnyKasprzak.MovieCatalogue.DAO.Mock.Models;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace GrobelnyKasprzak.MovieCatalogue.DAO.Mock
{
    public class MovieRepositoryMock : IMovieRepository
    {
        private static readonly List<Movie> _movies =
        [
            new Movie { Id = 1, Title = "Matrix", Year = 1999, Genre = MovieGenre.SciFi, DirectorId = 1 },
            new Movie { Id = 2, Title = "Shrek", Year = 2001, Genre = MovieGenre.Animation, DirectorId = 2 }
        ];

        public IEnumerable<IMovie> GetAll()
        {
            return _movies;
        }

        public IEnumerable<IMovie> GetByDirectorId(int directorId)
        {
            return _movies.Where(m => m.DirectorId == directorId).ToList();
        }

        public IMovie? GetById(int id)
        {
            return _movies.FirstOrDefault(m => m.Id == id);
        }

        public IMovie CreateNew()
        {
            return new Movie { Year = DateTime.Now.Year };
        }

        public void Add(IMovie movie)
        {
            var newMovie = new Movie
            {
                Id = _movies.Any() ? _movies.Max(m => m.Id) + 1 : 1,
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre,
                DirectorId = movie.DirectorId
            };

            ValidateMovie(newMovie);

            _movies.Add(newMovie);
        }

        public void Update(IMovie movie)
        {
            var existing = GetById(movie.Id)
                ?? throw new KeyNotFoundException($"Movie with ID {movie.Id} not found.");

            ValidateMovie(movie);

            existing.Title = movie.Title;
            existing.Year = movie.Year;
            existing.Genre = movie.Genre;
            existing.DirectorId = movie.DirectorId;
        }

        public void Delete(int id)
        {
            var movie = GetById(id)
                ?? throw new KeyNotFoundException($"Movie with ID {id} not found.");

            _movies.Remove((Movie)movie);
        }

        public bool Exists(int? excludeId = null, string? title = null, int? year = null, MovieGenre? genre = null, int? directorId = null)
        {
            return _movies.Any(m =>
                (excludeId == null || m.Id != excludeId) &&
                (title == null || m.Title == title) &&
                (year == null || m.Year == year) &&
                (genre == null || m.Genre == genre) &&
                (directorId == null || m.DirectorId == directorId)
            );
        }

        private static void ValidateMovie(IMovie movie)
        {
            var context = new ValidationContext(movie);
            Validator.ValidateObject(movie, context, validateAllProperties: true);
        }
    }
}
