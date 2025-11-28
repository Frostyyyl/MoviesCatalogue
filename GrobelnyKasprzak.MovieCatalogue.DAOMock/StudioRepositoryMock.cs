using GrobelnyKasprzak.MovieCatalogue.Entity;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
namespace GrobelnyKasprzak.MovieCatalogue.DAOMock
{
    public class StudioRepositoryMock : IStudioRepository
    {
        private static List<Studio> _studios = new List<Studio>()
        {
            new Studio { Id = 1, Name = "Warner Bros" },
            new Studio { Id = 2, Name = "DreamWorks" },
            new Studio { Id = 3, Name = "Universal Pictures" },
            new Studio { Id = 4, Name = "Paramount Pictures" }
        };

        private static int _nextId = 5;

        public IEnumerable<Studio> GetAll()
        {
            return _studios;
        }

        public Studio? GetById(int id)
        {
            return _studios.FirstOrDefault(s => s.Id == id);
        }

        public void Add(Studio studio)
        {
            studio.Id = _nextId++;
            if (studio.Movies == null) studio.Movies = new List<Movie>();
            _studios.Add(studio);
        }

        public void Update(Studio studio)
        {
            var existing = GetById(studio.Id);
            if (existing != null)
            {
                existing.Name = studio.Name;
            }
        }
        public void Delete(int id)
        {
            var studio = GetById(id);
            if (studio != null)
            {
                _studios.Remove(studio);
            }
        }
    }
}
