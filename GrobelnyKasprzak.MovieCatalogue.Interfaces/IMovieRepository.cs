namespace GrobelnyKasprzak.MovieCatalogue.Interfaces
{
    public interface IMovieRepository
    {
        IEnumerable<IMovie> GetAll();
        IEnumerable<IMovie> GetByDirectorId(int directorId);
        IMovie? GetById(int id);
        void Add(IMovie movie);
        void Update(IMovie movie);
        void Delete(int id);
        IMovie CreateNew();
    }
}
