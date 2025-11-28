using GrobelnyKasprzak.MovieCatalogue.Data;
using GrobelnyKasprzak.MovieCatalogue.Entity;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GrobelnyKasprzak.MovieCatalogue.DAL
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieCatalogueContext _db;

        public MovieRepository()
        {
            _db = new MovieCatalogueContext();
            _db.Database.EnsureCreated();
        }

        public IEnumerable<Movie> GetAll()
        {
            return _db.Movies
                .Include(m => m.Studio)
                .Include(m => m.Director)
                .ToList();
        }

        public Movie? GetById(int id)
        {
            return _db.Movies
                .Include(m => m.Studio)
                .Include(m => m.Director)
                .FirstOrDefault(m => m.Id == id);
        }

        public void Add(Movie movie)
        {
            _db.Movies.Add(movie);
            _db.SaveChanges();
        }

        public void Update(Movie movie)
        {
            _db.Movies.Update(movie);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var m = GetById(id);
            if (m != null)
            {
                _db.Movies.Remove(m);
                _db.SaveChanges();
            }
        }
    }
}
