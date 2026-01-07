using System.ComponentModel.DataAnnotations;

namespace GrobelnyKasprzak.MovieCatalogue.Interfaces;

public interface IDirectorService
{
    ValidationResult? ValidateDirector(string? name, int? birthYear, int? id = null);
    ValidationResult? ValidateName(string? name);
    ValidationResult? ValidateBirthYear(int? birthYear);
    ValidationResult? ValidateDuplicate(int? id, string name, int birthYear);
    IEnumerable<IDirector> GetAllDirectors();
    IDirector? GetDirectorById(int id);
    void AddDirector(IDirector director);
    void UpdateDirector(IDirector director);
    void DeleteDirector(int id);
    IDirector CreateNewDirector();
}