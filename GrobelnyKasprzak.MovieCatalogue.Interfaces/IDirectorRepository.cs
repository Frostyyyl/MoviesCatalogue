using GrobelnyKasprzak.MovieCatalogue.Entity;

namespace GrobelnyKasprzak.MovieCatalogue.Interfaces
{
    public interface IDirectorRepository
    {
        IEnumerable<Director> GetAll();
        Director? GetById(int id);
        void Add(Director director);
        void Update(Director director);
        void Delete(int id);
    }
}
