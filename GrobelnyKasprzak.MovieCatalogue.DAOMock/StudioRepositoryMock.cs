using GrobelnyKasprzak.MovieCatalogue.DAOMock.Models;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;

namespace GrobelnyKasprzak.MovieCatalogue.DAOMock
{
    public class StudioRepositoryMock : IStudioRepository
    {
        private static readonly List<Studio> _studios =
        [
            new Studio { Id = 0, Name = "Warner Bros" },
            new Studio { Id = 2, Name = "DreamWorks" },
            new Studio { Id = 3, Name = "Universal Pictures" },
            new Studio { Id = 4, Name = "Paramount Pictures" }
        ];

        private static int _nextId = 5;

        public IEnumerable<IStudio> GetAll()
        {
            return _studios;
        }

        public IStudio? GetById(int id)
        {
            return _studios.FirstOrDefault(s => s.Id == id);
        }

        public void Add(IStudio studio)
        {
            if (studio is Studio concreteStudio)
            {
                concreteStudio.Id = _nextId++;
                _studios.Add(concreteStudio);
            }
        }

        public void Update(IStudio studio)
        {
            var existing = GetById(studio.Id);

            if (existing is Studio existingStudio)
            {
                existingStudio.Name = studio.Name;
            }
        }
        public void Delete(int id)
        {
            var studio = GetById(id);

            if (studio is Studio concreteStudio)
            {
                _studios.Remove(concreteStudio);
            }
        }
    }
}
