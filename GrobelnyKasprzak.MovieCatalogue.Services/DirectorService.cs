using GrobelnyKasprzak.MovieCatalogue.Interfaces;

namespace GrobelnyKasprzak.MovieCatalogue.Services
{
    public class DirectorService
    {
        private readonly IDirectorRepository _repo;

        public DirectorService()
        {
            ReflectionLoader loader = new();
            _repo = loader.GetRepository<IDirectorRepository>();
        }

        public DirectorService(IDirectorRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<IDirector> GetAllDirectors() => _repo.GetAll();

        public IDirector? GetDirectorById(int id) => _repo.GetById(id);

        public void AddDirector(IDirector director) => _repo.Add(director);

        public void UpdateDirector(IDirector director) => _repo.Update(director);

        public void DeleteDirector(int id) => _repo.Delete(id);

        public IDirector CreateNewDirector() => _repo.CreateNew();
    }
}
