using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrobelnyKasprzak.MovieCatalogue.Entity
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 100 characters")]
        public string Title { get; set; }
        [Range(1888, 2100, ErrorMessage = "Year must be between 1888 and 2100")]
        public int Year { get; set; }
        [Required(ErrorMessage = "Genre is required")]
        public MovieGenre Genre { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Studio")]
        public int StudioId { get; set; }
        [ForeignKey("StudioId")]
        public Studio Studio { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Director")]
        public int DirectorId { get; set; }
        [ForeignKey("DirectorId")]
        public Director Director { get; set; }
    }
}
