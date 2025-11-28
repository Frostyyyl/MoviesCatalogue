using GrobelnyKasprzak.MovieCatalogue.Entity;

namespace GrobelnyKasprzak.MovieCatalogue.Interfaces
{
    public interface IStudioRepository
    {
        IEnumerable<Studio> GetAll();
        Studio? GetById(int id);
        void Add(Studio studio);
        void Update(Studio studio);
        void Delete(int id);
    }
}
