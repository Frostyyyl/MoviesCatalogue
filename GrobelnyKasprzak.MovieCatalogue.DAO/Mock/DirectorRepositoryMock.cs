using GrobelnyKasprzak.MovieCatalogue.DAO.Mock.Models;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace GrobelnyKasprzak.MovieCatalogue.DAO.Mock
{
    public class DirectorRepositoryMock : IDirectorRepository
    {
        private static readonly List<Director> _directors =
        [
            new Director { Id = 1, Name = "Lana & Lilly Wachowski", BirthYear = 1968 },
            new Director { Id = 2, Name = "Andrew Adamson", BirthYear = 1966 },
            new Director { Id = 3, Name = "Quentin Tarantino", BirthYear = 1963 },
            new Director { Id = 4, Name = "Christopher Nolan", BirthYear = 1970 },
            new Director { Id = 5, Name = "Sergio Leone", BirthYear = 1929 },
            new Director { Id = 6, Name = "Stanley Kubrick", BirthYear = 1928 },
            new Director { Id = 7, Name = "David Lynch", BirthYear = 1946 },
        ];

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
            return new Director { BirthYear = DateTime.Now.Year };
        }

        public void Add(IDirector director)
        {
            int nextId = _directors.Count > 0 ? _directors.Max(d => d.Id) + 1 : 1;

            var newDirector = new Director
            {
                Id = nextId,
                Name = director.Name,
                BirthYear = director.BirthYear
            };

            ValidateDirector(newDirector);

            _directors.Add(newDirector);
            director.Id = nextId;
        }


        public void Update(IDirector director)
        {
            var existing = GetById(director.Id)
                ?? throw new KeyNotFoundException($"Director with ID {director.Id} not found.");

            ValidateDirector(director);

            existing.Name = director.Name;
            existing.BirthYear = director.BirthYear;
        }

        public void Delete(int id)
        {
            var director = GetById(id)
                ?? throw new KeyNotFoundException($"Director with ID {id} not found.");

            _directors.Remove((Director)director);
        }

        public bool Exists(int? excludeId = null, string? name = null, int? birthYear = null)
        {
            return _directors.Any(m =>
                (name == null || m.Name == name) &&
                (birthYear == null || m.BirthYear == birthYear) &&
                (excludeId == null || m.Id != excludeId)
            );
        }

        private static void ValidateDirector(IDirector director)
        {
            var context = new ValidationContext(director);
            Validator.ValidateObject(director, context, validateAllProperties: true);
        }
    }
}
