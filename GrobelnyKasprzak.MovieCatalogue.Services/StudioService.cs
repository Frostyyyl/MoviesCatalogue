using GrobelnyKasprzak.MovieCatalogue.Interfaces;

namespace GrobelnyKasprzak.MovieCatalogue.Services
{
    public class StudioService
    {
        private readonly IStudioRepository _repo;

        public StudioService(IStudioRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<IStudio> GetAllStudios() => _repo.GetAll();

        public IStudio? GetStudioById(int id) => _repo.GetById(id);

        public void AddStudio(IStudio studio) => _repo.Add(studio);

        public void UpdateStudio(IStudio studio) => _repo.Update(studio);

        public void DeleteStudio(int id) => _repo.Delete(id);
    }
}
