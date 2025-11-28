using GrobelnyKasprzak.MovieCatalogue.Data;
using GrobelnyKasprzak.MovieCatalogue.Entity;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GrobelnyKasprzak.MovieCatalogue.Repositories
{
    public class DirectorRepository : IDirectorRepository
    {
        private readonly MovieCatalogueContext _db;

        public DirectorRepository(MovieCatalogueContext db)
        {
            _db = db;
        }

        public IEnumerable<Director> GetAll() =>
            _db.Directors.Include(d => d.Movies).ToList();

        public Director? GetById(int id) =>
            _db.Directors.Include(d => d.Movies)
                         .FirstOrDefault(d => d.Id == id);

        public void Add(Director director)
        {
            _db.Directors.Add(director);
            _db.SaveChanges();
        }

        public void Update(Director director)
        {
            _db.Directors.Update(director);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var d = GetById(id);
            if (d != null)
            {
                _db.Directors.Remove(d);
                _db.SaveChanges();
            }
        }
    }
}
