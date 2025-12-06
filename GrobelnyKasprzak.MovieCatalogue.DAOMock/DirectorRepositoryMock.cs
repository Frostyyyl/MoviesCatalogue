using GrobelnyKasprzak.MovieCatalogue.DAOMock.Models;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;

namespace GrobelnyKasprzak.MovieCatalogue.DAOMock
{
    public class DirectorRepositoryMock : IDirectorRepository
    {
        private static readonly List<Director> _directors =
        [
            new Director { Id = 1, Name = "Lana & Lilly Wachowski" },
            new Director { Id = 2, Name = "Andrew Adamson" },
            new Director { Id = 3, Name = "Quentin Tarantino" },
            new Director { Id = 4, Name = "Christopher Nolan" }
        ];

        private static int _nextId = 5;

        public IEnumerable<IDirector> GetAll()
        {
            return _directors;
        }

        public IDirector? GetById(int id)
        {
            return _directors.FirstOrDefault(d => d.Id == id);
        }

        public IDirector CreateNew()
        {
            return new Director { Name = "" };
        }

        public void Add(IDirector director)
        {
            if (director is Director concreteDirector)
            {
                concreteDirector.Id = _nextId++;
                _directors.Add(concreteDirector);
            }
        }

        public void Update(IDirector director)
        {
            var existing = GetById(director.Id);

            if (existing is Director existingDirector)
            {
                existingDirector.Name = director.Name;
            }
        }

        public void Delete(int id)
        {
            var director = GetById(id);

            if (director is Director concreteDirector)
            {
                _directors.Remove(concreteDirector);
            }
        }
    }
}
