using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace GrobelnyKasprzak.MovieCatalogue.Services
{
    public class DirectorService(IDirectorRepository directorRepository, IMovieRepository movieRepository) : IDirectorService
    {
        private readonly IDirectorRepository _directorRepository = directorRepository;
        private readonly IMovieRepository _movieRepository = movieRepository;

        public IEnumerable<IDirector> GetAllDirectors() => _directorRepository.GetAll();

        public IDirector? GetDirectorById(int id) => _directorRepository.GetById(id);

        public ValidationResult? ValidateDirector(string? name, int? birthYear, int? id = null)
        {
            ValidationResult? result;

            result = ValidateName(name);
            if (result != null) return result;

            result = ValidateBirthYear(birthYear);
            if (result != null) return result;

            result = ValidateDuplicate(id, name!, birthYear!.Value);
            if (result != null) return result;

            return null;
        }


        public void AddDirector(IDirector director)
        {
            ValidationResult? result = ValidateDirector(director.Name, director.BirthYear, director.Id);

            if (result != null)
            {
                throw new InvalidOperationException(result.ErrorMessage);
            }

            _directorRepository.Add(director);
        }

        public void UpdateDirector(IDirector director)
        {
            ValidationResult? result = ValidateDirector(director.Name, director.BirthYear, director.Id);

            if (result != null)
            {
                throw new InvalidOperationException(result.ErrorMessage);
            }

            _directorRepository.Update(director);
        }

        public void DeleteDirector(int id)
        {
            var hasMovies = _movieRepository.GetByDirectorId(id).Any();

            if (hasMovies)
            {
                throw new InvalidOperationException("Cannot delete a director who has movies assigned.");
            }

            _directorRepository.Delete(id);
        }

        public IDirector CreateNewDirector() => _directorRepository.CreateNew();

        public ValidationResult? ValidateName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new ValidationResult(
                    "Surname is required.",
                    [nameof(IDirector.Name)]);

            if (name.Length < 3)
            {
                return new ValidationResult(
                    "Surname must have at least 3 characters.",
                    [nameof(IDirector.Name)]);
            }

            if (name.Length > 100)
            {
                return new ValidationResult(
                    "Surname must have at most 100 characters.",
                    [nameof(IDirector.Name)]);
            }

            return null;
        }

        public ValidationResult? ValidateBirthYear(int? birthYear)
        {
            if (birthYear == null)
                return new ValidationResult(
                    "Year is required.",
                    [nameof(IDirector.BirthYear)]);

            if (birthYear < 1800 || birthYear > DateTime.Now.Year)
                return new ValidationResult(
                    "Year must be between 1800 and current.",
                    [nameof(IDirector.BirthYear)]);

            return null;
        }

        public ValidationResult? ValidateDuplicate(int? id, string name, int birthYear)
        {
            if (_directorRepository.Exists(id, name, birthYear))
                return new ValidationResult(
                    "This director is already in the system.");

            return null;
        }
    }
}