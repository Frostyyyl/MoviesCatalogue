using GrobelnyKasprzak.MovieCatalogue.Data;
using GrobelnyKasprzak.MovieCatalogue.Entity;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GrobelnyKasprzak.MovieCatalogue.Repositories
{
    public class StudioRepository : IStudioRepository
    {
        private readonly MovieCatalogueContext _db;

        public StudioRepository(MovieCatalogueContext db)
        {
            _db = db;
        }

        public IEnumerable<Studio> GetAll() =>
            _db.Studios.Include(s => s.Movies).ToList();

        public Studio? GetById(int id) =>
            _db.Studios.Include(s => s.Movies)
                       .FirstOrDefault(s => s.Id == id);

        public void Add(Studio studio)
        {
            _db.Studios.Add(studio);
            _db.SaveChanges();
        }

        public void Update(Studio studio)
        {
            _db.Studios.Update(studio);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var s = GetById(id);
            if (s != null)
            {
                _db.Studios.Remove(s);
                _db.SaveChanges();
            }
        }
    }
}
