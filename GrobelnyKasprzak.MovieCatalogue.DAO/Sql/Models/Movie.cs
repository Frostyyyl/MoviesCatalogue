using GrobelnyKasprzak.MovieCatalogue.Core;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrobelnyKasprzak.MovieCatalogue.DAO.Sql.Models
{
    public class Movie : IMovie
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 100 characters")]
        public string Title { get; set; } = string.Empty;
        [Range(1888, int.MaxValue, ErrorMessage = "Year must be 1888 or later")]
        public int Year { get; set; }
        [Required(ErrorMessage = "Genre is required")]
        public MovieGenre Genre { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Director")]
        public int DirectorId { get; set; }
        [ForeignKey("DirectorId")]
        public Director? Director { get; set; }
    }
}
