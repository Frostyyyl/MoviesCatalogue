using GrobelnyKasprzak.MovieCatalogue.DAOSql.Models;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;

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

        public IEnumerable<IMovie> GetAll() => _db.Movies.ToList();

        public IEnumerable<IMovie> GetByDirectorId(int directorId) =>
            _db.Movies.Where(m => m.DirectorId == directorId).ToList();

        public IMovie? GetById(int id) =>
            _db.Movies.FirstOrDefault(m => m.Id == id);

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

        public IMovie CreateNew()
        {
            throw new NotImplementedException();
        }
    }
}
