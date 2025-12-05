using GrobelnyKasprzak.MovieCatalogue.DAOSql.Models;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GrobelnyKasprzak.MovieCatalogue.DAOSql
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieCatalogueContext _db;

        public MovieRepository()
        {
            _db = new MovieCatalogueContext();
            _db.Database.EnsureCreated();
        }

        public IEnumerable<IMovie> GetAll()
        {
            return _db.Movies
                .Include(m => m.Director)
                .ToList();
        }

        public IMovie? GetById(int id)
        {
            return _db.Movies
                .Include(m => m.Director)
                .FirstOrDefault(m => m.Id == id);
        }

        public void Add(IMovie movie)
        {
            _db.Movies.Add((Movie)movie);
            _db.SaveChanges();
        }

        public void Update(IMovie movie)
        {
            _db.Movies.Update((Movie)movie);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var movie = GetById(id);

            if (movie is Movie concreteMovie)
            {
                _db.Movies.Remove(concreteMovie);
                _db.SaveChanges();
            }
        }
    }
}
