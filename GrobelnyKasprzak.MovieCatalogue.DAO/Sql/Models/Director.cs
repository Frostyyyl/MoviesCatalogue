using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace GrobelnyKasprzak.MovieCatalogue.DAO.Sql.Models
{
    public class Director : IDirector
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Director name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Director year of birth is required")]
        [Range(1800, int.MaxValue, ErrorMessage = "Year of birth must be 1800 or later")]
        public int BirthYear { get; set; }
        public ICollection<Movie> Movies { get; set; } = [];
    }
}
