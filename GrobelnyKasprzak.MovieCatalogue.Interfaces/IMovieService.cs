using System.ComponentModel.DataAnnotations;

namespace GrobelnyKasprzak.MovieCatalogue.Interfaces;

public interface IMovieService
{

    ValidationResult? ValidateMovie(string? title, int? year, int? directorId, int? id = null);
    IEnumerable<IMovie> GetAllMovies();
    IEnumerable<IMovie> GetMoviesByDirectorId(int directorId);
    IMovie? GetMovieById(int id);
    void AddMovie(IMovie movie);
    void UpdateMovie(IMovie movie);
    void DeleteMovie(int id);
    IMovie CreateNewMovie();
}
