using GrobelnyKasprzak.MovieCatalogue.Core;
using GrobelnyKasprzak.MovieCatalogue.DAO.Sql.Models;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace GrobelnyKasprzak.MovieCatalogue.DAO.Sql
{
    public class MovieRepository(MovieCatalogueContext db) : IMovieRepository
    {
        private readonly MovieCatalogueContext _db = db;

        public IEnumerable<IMovie> GetAll() => _db.Movies.ToList();

        public IEnumerable<IMovie> GetByDirectorId(int directorId) =>
            _db.Movies.Where(m => m.DirectorId == directorId);

        public IMovie? GetById(int id) =>
            _db.Movies.FirstOrDefault(m => m.Id == id);

        public void Add(IMovie movie)
        {
            var newMovie = new Movie
            {
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre,
                DirectorId = movie.DirectorId
            };

            ValidateMovie(newMovie);

            _db.Movies.Add(newMovie);
            _db.SaveChanges();
        }

        public void Update(IMovie movie)
        {
            var existing = GetById(movie.Id)
                ?? throw new KeyNotFoundException($"Movie with ID {movie.Id} not found.");

            ValidateMovie(movie);

            existing.Title = movie.Title;
            existing.Year = movie.Year;
            existing.Genre = movie.Genre;
            existing.DirectorId = movie.DirectorId;

            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var movie = GetById(id)
                ?? throw new KeyNotFoundException($"Movie with ID {id} not found.");

            _db.Movies.Remove((Movie)movie);
            _db.SaveChanges();
        }

        public IMovie CreateNew()
        {
            return new Movie { Year = DateTime.Now.Year };
        }

        private static void ValidateMovie(IMovie movie)
        {
            var context = new ValidationContext(movie);
            Validator.ValidateObject(movie, context, validateAllProperties: true);
        }
        public bool Exists(string? title = null, int? year = null, MovieGenre? genre = null, int? directorId = null, int? excludeId = null)
        {
            var query = _db.Movies.AsQueryable();

            if (excludeId.HasValue)
                query = query.Where(m => m.Id != excludeId.Value);

            if (!string.IsNullOrEmpty(title))
                query = query.Where(m => m.Title == title);

            if (year.HasValue)
                query = query.Where(m => m.Year == year.Value);

            if (genre.HasValue)
                query = query.Where(m => m.Genre == genre.Value);

            if (directorId.HasValue)
                query = query.Where(m => m.DirectorId == directorId.Value);

            return query.Any();
        }
    }
}
