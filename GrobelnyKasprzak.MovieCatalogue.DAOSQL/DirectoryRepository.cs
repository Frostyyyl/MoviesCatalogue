using GrobelnyKasprzak.MovieCatalogue.DAOSQL.Models;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GrobelnyKasprzak.MovieCatalogue.DAOSQL
{
    public class DirectorRepository : IDirectorRepository
    {
        private readonly MovieCatalogueContext _db;

        public DirectorRepository(MovieCatalogueContext db)
        {
            _db = db;
        }

        public IEnumerable<IDirector> GetAll() =>
            _db.Directors.Include(d => d.Movies).ToList();

        public IDirector? GetById(int id) =>
            _db.Directors.Include(d => d.Movies)
                         .FirstOrDefault(d => d.Id == id);

        public void Add(IDirector director)
        {
            _db.Directors.Add((Director)director);
            _db.SaveChanges();
        }

        public void Update(IDirector director)
        {
            _db.Directors.Update((Director)director);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var director = GetById(id);

            if (director is Director concreteDirector)
            {
                _db.Directors.Remove(concreteDirector);
                _db.SaveChanges();
            }
        }
    }
}
