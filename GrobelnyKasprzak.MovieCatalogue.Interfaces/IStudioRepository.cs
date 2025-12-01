namespace GrobelnyKasprzak.MovieCatalogue.Interfaces
{
    public interface IStudioRepository
    {
        IEnumerable<IStudio> GetAll();
        IStudio? GetById(int id);
        void Add(IStudio studio);
        void Update(IStudio studio);
        void Delete(int id);
    }
}
