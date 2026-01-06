using GrobelnyKasprzak.MovieCatalogue.DAO.Sql.Models;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace GrobelnyKasprzak.MovieCatalogue.DAO.Sql
{
    public class DirectorRepository(MovieCatalogueContext db) : IDirectorRepository
    {
        private readonly MovieCatalogueContext _db = db;

        public IEnumerable<IDirector> GetAll() => _db.Directors.ToList();

        public IDirector? GetById(int id) => _db.Directors.Find(id);

        public void Add(IDirector director)
        {
            var newDirector = new Director
            {
                Name = director.Name,
                BirthYear = director.BirthYear
            };

            ValidateDirector(newDirector);

            _db.Directors.Add(newDirector);
            _db.SaveChanges();
        }

        public void Update(IDirector director)
        {
            var existing = GetById(director.Id)
                ?? throw new KeyNotFoundException($"Director with ID {director.Id} not found.");

            ValidateDirector(director);

            existing.Name = director.Name;
            existing.BirthYear = director.BirthYear;

            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var director = GetById(id)
                ?? throw new KeyNotFoundException($"Director with ID {id} not found.");

            _db.Directors.Remove((Director)director);
            _db.SaveChanges();
        }

        public IDirector CreateNew()
        {
            return new Director { BirthYear = DateTime.Now.Year };
        }

        public bool Exists(string? name = null, int? birthYear = null)
        {
            var query = _db.Directors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(d => d.Name == name);

            if (birthYear.HasValue)
                query = query.Where(d => d.BirthYear == birthYear.Value);

            return query.Any();
        }

        private static void ValidateDirector(IDirector director)
        {
            var context = new ValidationContext(director);
            Validator.ValidateObject(director, context, validateAllProperties: true);
        }
    }
}
