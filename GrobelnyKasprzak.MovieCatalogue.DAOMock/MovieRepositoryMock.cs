using GrobelnyKasprzak.MovieCatalogue.Entity;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using System.Xml.Linq;

namespace GrobelnyKasprzak.MovieCatalogue.DAOMock
{
    public class MovieRepositoryMock : IMovieRepository
    {
        private static List<Movie> _movies = new List<Movie>()
        {
            new Movie { Id = 1, Title = "Matrix", Year = 1999, StudioId = 1, Genre = MovieGenre.SciFi, DirectorId = 1 },
            new Movie { Id = 2, Title = "Shrek", Year = 2001, StudioId = 2, Genre = MovieGenre.Animation, DirectorId = 2 }
        };

        private static int _nextId = 3;

        public IEnumerable<Movie> GetAll()
        {
            var studioRepo = new StudioRepositoryMock();
            var directorRepo = new DirectorRepositoryMock();

            foreach (var movie in _movies)
            {
                if (movie.StudioId > 0)
                    movie.Studio = studioRepo.GetById(movie.StudioId);

                if (movie.DirectorId > 0)
                    movie.Director = directorRepo.GetById(movie.DirectorId);
            }

            return _movies;
        }

        public Movie? GetById(int id)
        {
            var movie = _movies.FirstOrDefault(m => m.Id == id);

            if (movie != null)
            {
                var studioRepo = new StudioRepositoryMock();
                var directorRepo = new DirectorRepositoryMock();

                movie.Studio = studioRepo.GetById(movie.StudioId);
                movie.Director = directorRepo.GetById(movie.DirectorId);
            }

            return movie;
        }

        public void Add(Movie movie)
        {
            movie.Id = _nextId++;
            _movies.Add(movie);
        }

        public void Update(Movie movie)
        {
            var existing = _movies.FirstOrDefault(m => m.Id == movie.Id);
            if (existing != null)
            {
                existing.Title = movie.Title;
                existing.Year = movie.Year;
                existing.Genre = movie.Genre;
                existing.StudioId = movie.StudioId;
                existing.DirectorId = movie.DirectorId;

                var studioRepo = new StudioRepositoryMock();
                existing.Studio = studioRepo.GetById(movie.StudioId);

                var directorRepo = new DirectorRepositoryMock();
                existing.Director = directorRepo.GetById(movie.DirectorId);
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
