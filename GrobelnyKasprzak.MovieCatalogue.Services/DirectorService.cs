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
            if (string.IsNullOrWhiteSpace(name))
                return new ValidationResult("Surname is required.", [nameof(IDirector.Name)]);

            if (birthYear == null)
                return new ValidationResult("Year is required.", [nameof(IDirector.BirthYear)]);

            if (birthYear < 1800 || birthYear > DateTime.Now.Year)
                return new ValidationResult("Year must be 1800–current.", [nameof(IDirector.BirthYear)]);

            var duplicate = _directorRepository.Exists(id, name, birthYear);
            if (duplicate)
                return new ValidationResult("This director is already in the system.");

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
    }
}