using GrobelnyKasprzak.MovieCatalogue.Interfaces;

namespace GrobelnyKasprzak.MovieCatalogue.Services
{
    public class MovieService(IMovieRepository repo) : IMovieService
    {
        private readonly IMovieRepository _repo = repo;

        public IEnumerable<IMovie> GetAllMovies() => _repo.GetAll();

        public IEnumerable<IMovie> GetMoviesByDirectorId(int directorId)
            => _repo.GetByDirectorId(directorId);

        public IMovie? GetMovieById(int id) => _repo.GetById(id);

        public void AddMovie(IMovie movie)
        {
            if (_repo.Exists(title: movie.Title, year: movie.Year, directorId: movie.DirectorId))
            {
                throw new InvalidOperationException($"This movie already exists for this director in the year {movie.Year}.");
            }

            _repo.Add(movie);
        }

        public void UpdateMovie(IMovie movie)
        {
            if (_repo.Exists(title: movie.Title, year: movie.Year, directorId: movie.DirectorId, excludeId: movie.Id))
            {
                throw new InvalidOperationException($"This movie already exists for this director in the year {movie.Year}.");
            }

            _repo.Update(movie);

        }

        public void DeleteMovie(int id) => _repo.Delete(id);

        public IMovie CreateNewMovie() => _repo.CreateNew();
    }
}
