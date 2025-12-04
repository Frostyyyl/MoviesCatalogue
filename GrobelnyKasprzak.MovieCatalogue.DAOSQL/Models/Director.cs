using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace GrobelnyKasprzak.MovieCatalogue.DAOSql.Models
{
    public class Director : IDirector
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Director name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public required string Name { get; set; }
        public ICollection<Movie> Movies { get; set; } = [];
    }
}
