using GrobelnyKasprzak.MovieCatalogue.DAOMock.Models;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using System.ComponentModel.DataAnnotations;

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
            return new Director();
        }

        public void Add(IDirector director)
        {
            ArgumentNullException.ThrowIfNull(director);

            var newDirector = new Director
            {
                Id = _nextId++,
                Name = director.Name
            };

            ValidateDirector(newDirector);

            _directors.Add(newDirector);
        }


        public void Update(IDirector director)
        {
            ArgumentNullException.ThrowIfNull(director);

            var existing = GetById(director.Id)
                ?? throw new KeyNotFoundException($"Director with ID {director.Id} not found.");

            var directorToUpdate = new Director
            {
                Name = director.Name
            };

            ValidateDirector(director);

            existing.Name = directorToUpdate.Name;
        }

        public void Delete(int id)
        {
            var director = GetById(id)
                ?? throw new KeyNotFoundException($"Director with ID {id} not found.");

            _directors.Remove((Director)director);
        }

        private static void ValidateDirector(IDirector director)
        {
            var context = new ValidationContext(director);
            Validator.ValidateObject(director, context, validateAllProperties: true);
        }
    }
}
