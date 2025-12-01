namespace GrobelnyKasprzak.MovieCatalogue.Interfaces
{
    public interface IMovieRepository
    {
        IEnumerable<IMovie> GetAll();
        IMovie? GetById(int id);
        void Add(IMovie movie);
        void Update(IMovie movie);
        void Delete(int id);
    }
}
