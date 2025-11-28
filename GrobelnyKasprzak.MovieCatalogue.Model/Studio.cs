using System.ComponentModel.DataAnnotations;

namespace GrobelnyKasprzak.MovieCatalogue.Entity
{
    public class Studio
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Studio name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")] 
        public string Name { get; set; }
        public List<Movie> Movies { get; set; } = new();
    }
}
