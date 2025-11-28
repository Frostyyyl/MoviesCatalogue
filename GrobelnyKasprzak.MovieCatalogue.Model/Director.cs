using System.ComponentModel.DataAnnotations;

namespace GrobelnyKasprzak.MovieCatalogue.Entity
{
    public class Director
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Director name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")] 
        public string Name { get; set; }

        public List<Movie> Movies { get; set; } = new();
    }
}
