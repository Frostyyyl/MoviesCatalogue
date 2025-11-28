using GrobelnyKasprzak.MovieCatalogue.Entity;

namespace GrobelnyKasprzak.MovieCatalogue.Interfaces
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> GetAll();
        Movie? GetById(int id);
        void Add(Movie movie);
        void Update(Movie movie);
        void Delete(int id);
    }
}
