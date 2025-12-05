using GrobelnyKasprzak.MovieCatalogue.Core;

namespace GrobelnyKasprzak.MovieCatalogue.MVC.ViewModels
{
    public class MovieViewModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int Year { get; set; }
        public MovieGenre Genre { get; set; }
        public required string Director { get; set; }
    }
}
