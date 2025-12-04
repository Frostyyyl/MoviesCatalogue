using GrobelnyKasprzak.MovieCatalogue.Core;
using GrobelnyKasprzak.MovieCatalogue.DAOMock.Models;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;

namespace GrobelnyKasprzak.MovieCatalogue.DAOMock
{
    public class MovieRepositoryMock : IMovieRepository
    {
        private static readonly List<Movie> _movies =
        [
            new Movie { Id = 1, Title = "Matrix", Year = 1999, StudioId = 1, Genre = MovieGenre.SciFi, DirectorId = 1 },
            new Movie { Id = 2, Title = "Shrek", Year = 2001, StudioId = 2, Genre = MovieGenre.Animation, DirectorId = 2 }
        ];

        private static int _nextId = 3;

        public IEnumerable<IMovie> GetAll()
        {
            return _movies;
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

        public void Add(IMovie movie)
        {
            if (movie is Movie concreteMovie)
            {
                concreteMovie.Id = _nextId++;
                _movies.Add(concreteMovie);
            }
        }

        public void Update(IMovie movie)
        {
            var existing = _movies.FirstOrDefault(m => m.Id == movie.Id);

            if (existing != null)
            {
                existing.Title = movie.Title;
                existing.Year = movie.Year;
                existing.Genre = movie.Genre;
                existing.StudioId = movie.StudioId;
                existing.DirectorId = movie.DirectorId;
            }
        }

        public void Delete(int id)
        {
            var movie = _movies.FirstOrDefault(m => m.Id == id);

            if (movie != null)
            {
                _movies.Remove(movie);
            }
        }
    }
}
