using GrobelnyKasprzak.MovieCatalogue.Core;
using GrobelnyKasprzak.MovieCatalogue.DAOMock.Models;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;

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
            var movie = _movies.FirstOrDefault(m => m.Id == id);

            if (movie != null)
            {
                return movie;
            }

            return null;
        }

        public IMovie CreateNew()
        {
            return new Movie { Title = "", Year = 2025 };
        }

        public void Add(IMovie movie)
        {
            var newMovie = new Movie
            {
                Id = _nextId++,
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre,
                DirectorId = movie.DirectorId
            };

            _movies.Add(newMovie);
        }

        public void Update(IMovie movie)
        {
            var existing = GetById(movie.Id);

            if (existing != null)
            {
                existing.Title = movie.Title;
                existing.Year = movie.Year;
                existing.Genre = movie.Genre;
                existing.DirectorId = movie.DirectorId;
            }
        }

        public void Delete(int id)
        {
            var movie = GetById(id);

            if (movie != null)
            {
                _movies.Remove((Movie)movie);
            }
        }
    }
}
