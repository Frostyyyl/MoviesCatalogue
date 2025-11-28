using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using GrobelnyKasprzak.MovieCatalogue.Entity;

namespace GrobelnyKasprzak.MovieCatalogue.Services
{
    public class MovieService
    {
        private readonly IMovieRepository _repo;

        public MovieService(IMovieRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Movie> GetAllMovies() => _repo.GetAll();

        public Movie? GetMovieById(int id) => _repo.GetById(id);

        public void AddMovie(Movie movie) => _repo.Add(movie);

        public void UpdateMovie(Movie movie) => _repo.Update(movie);

        public void DeleteMovie(int id) => _repo.Delete(id);
    }
}
