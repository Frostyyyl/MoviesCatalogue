using GrobelnyKasprzak.MovieCatalogue.DAOSql.Models;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GrobelnyKasprzak.MovieCatalogue.DAOSql
{
    public class StudioRepository : IStudioRepository
    {
        private readonly MovieCatalogueContext _db;

        public StudioRepository(MovieCatalogueContext db)
        {
            _db = db;
        }

        public IEnumerable<IStudio> GetAll() =>
            _db.Studios.Include(s => s.Movies).ToList();

        public IStudio? GetById(int id) =>
            _db.Studios.Include(s => s.Movies)
                       .FirstOrDefault(s => s.Id == id);

        public void Add(IStudio studio)
        {
            _db.Studios.Add((Studio)studio);
            _db.SaveChanges();
        }

        public void Update(IStudio studio)
        {
            _db.Studios.Update((Studio)studio);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var studio = GetById(id);

            if (studio is Studio concreteStudio)
            {
                _db.Studios.Remove(concreteStudio);
                _db.SaveChanges();
            }
        }
    }
}
