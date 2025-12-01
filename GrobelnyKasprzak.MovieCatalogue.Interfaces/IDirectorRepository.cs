namespace GrobelnyKasprzak.MovieCatalogue.Interfaces
{
    public interface IDirectorRepository
    {
        IEnumerable<IDirector> GetAll();
        IDirector? GetById(int id);
        void Add(IDirector director);
        void Update(IDirector director);
        void Delete(int id);
    }
}
