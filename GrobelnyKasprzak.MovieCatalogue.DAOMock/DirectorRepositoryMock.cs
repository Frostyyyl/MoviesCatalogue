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
            var newDirector = new Director
            {
                Id = _nextId++,
                Name = director.Name
            };

            _directors.Add(newDirector);
        }

        public void Update(IDirector director)
        {
            var existing = GetById(director.Id);

            if (existing != null)
            {
                existing.Name = director.Name;
            }
        }

        public void Delete(int id)
        {
            var director = GetById(id);

            if (director != null)
            {
                _directors.Remove((Director)director);
            }
        }
    }
}
