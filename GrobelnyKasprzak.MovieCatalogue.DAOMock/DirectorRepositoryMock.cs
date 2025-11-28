using GrobelnyKasprzak.MovieCatalogue.Entity;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;

namespace GrobelnyKasprzak.MovieCatalogue.DAOMock
{
    public class DirectorRepositoryMock : IDirectorRepository
    {
        private static List<Director> _directors = new List<Director>()
        {
            new Director { Id = 1, Name = "Lana & Lilly Wachowski" },
            new Director { Id = 2, Name = "Andrew Adamson" },
            new Director { Id = 3, Name = "Quentin Tarantino" },
            new Director { Id = 4, Name = "Christopher Nolan" }
        };

        private static int _nextId = 5;

        public IEnumerable<Director> GetAll()
        {
            return _directors;
        }

        public Director? GetById(int id)
        {
            return _directors.FirstOrDefault(d => d.Id == id);
        }

        public void Add(Director director)
        {
            director.Id = _nextId++;
            if (director.Movies == null) director.Movies = new List<Movie>();
            _directors.Add(director);
        }

        public void Update(Director director)
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
                _directors.Remove(director);
            }
        }
    }
}
