using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace GrobelnyKasprzak.MovieCatalogue.DAOMock.Models
{
    public class Studio : IStudio
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Studio name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public required string Name { get; set; }
    }
}
