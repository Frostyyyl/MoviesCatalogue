using GrobelnyKasprzak.MovieCatalogue.Core;
using System.ComponentModel.DataAnnotations;

namespace GrobelnyKasprzak.MovieCatalogue.Interfaces;

public interface IMovieService
{

    ValidationResult? ValidateMovie(string? title, int? year, int? directorId, MovieGenre? genre = null, int? id = null);
    ValidationResult? ValidateTitle(string? title);
    ValidationResult? ValidateYear(int? year);
    ValidationResult? ValidateDirector(int? directorId);
    ValidationResult? ValidateGenre(MovieGenre? genre);
    ValidationResult? ValidateDuplicate(int? id, string title, int year, int directorId);
    IEnumerable<IMovie> GetAllMovies();
    IEnumerable<IMovie> GetMoviesByDirectorId(int directorId);
    IMovie? GetMovieById(int id);
    void AddMovie(IMovie movie);
    void UpdateMovie(IMovie movie);
    void DeleteMovie(int id);
    IMovie CreateNewMovie();
}
