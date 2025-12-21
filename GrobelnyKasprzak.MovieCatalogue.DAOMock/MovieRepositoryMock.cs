using GrobelnyKasprzak.MovieCatalogue.Core;
using GrobelnyKasprzak.MovieCatalogue.DAOMock.Models;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace GrobelnyKasprzak.MovieCatalogue.DAOMock
{
    public class MovieRepositoryMock : IMovieRepository
    {
        private static readonly List<Movie> _movies =
        [
            new Movie { Id = 1, Title = "Matrix", Year = 1999, Genre = MovieGenre.SciFi, DirectorId = 1 },
            new Movie { Id = 2, Title = "Shrek", Year = 2001, Genre = MovieGenre.Animation, DirectorId = 2 }
        ];

        private static int _nextId = 3;

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
            ArgumentNullException.ThrowIfNull(movie);

            var newMovie = new Movie
            {
                Id = _nextId++,
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
            ArgumentNullException.ThrowIfNull(movie);

            var existing = GetById(movie.Id)
                ?? throw new KeyNotFoundException($"Movie with ID {movie.Id} not found.");

            var movieToUpdate = new Movie
            {
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre,
                DirectorId = movie.DirectorId
            };

            ValidateMovie(movieToUpdate);

            existing.Title = movieToUpdate.Title;
            existing.Year = movieToUpdate.Year;
            existing.Genre = movieToUpdate.Genre;
            existing.DirectorId = movieToUpdate.DirectorId;
        }

        public void Delete(int id)
        {
            var movie = GetById(id)
                ?? throw new KeyNotFoundException($"Movie with ID {id} not found.");

            _movies.Remove((Movie)movie);
        }

        private static void ValidateMovie(Movie movie)
        {
            var context = new ValidationContext(movie);
            Validator.ValidateObject(movie, context, validateAllProperties: true);
        }
    }
}
