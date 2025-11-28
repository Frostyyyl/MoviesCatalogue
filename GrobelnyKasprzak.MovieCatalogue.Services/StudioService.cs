using GrobelnyKasprzak.MovieCatalogue.Entity;
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

        public IEnumerable<Studio> GetAllStudios() => _repo.GetAll();

        public Studio? GetStudioById(int id) => _repo.GetById(id);

        public void AddStudio(Studio studio) => _repo.Add(studio);

        public void UpdateStudio(Studio studio) => _repo.Update(studio);

        public void DeleteStudio(int id) => _repo.Delete(id);
    }
}
