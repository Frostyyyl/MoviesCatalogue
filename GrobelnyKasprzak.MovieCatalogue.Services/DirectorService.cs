using GrobelnyKasprzak.MovieCatalogue.Entity;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;

namespace GrobelnyKasprzak.MovieCatalogue.Services
{
    public class DirectorService
    {
        private readonly IDirectorRepository _repo;

        public DirectorService(IDirectorRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Director> GetAllDirectors() => _repo.GetAll();

        public Director? GetDirectorById(int id) => _repo.GetById(id);

        public void AddDirector(Director director) => _repo.Add(director);

        public void UpdateDirector(Director director) => _repo.Update(director);

        public void DeleteDirector(int id) => _repo.Delete(id);
    }
}
