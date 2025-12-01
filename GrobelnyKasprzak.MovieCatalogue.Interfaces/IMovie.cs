using GrobelnyKasprzak.MovieCatalogue.Core;

namespace GrobelnyKasprzak.MovieCatalogue.Interfaces
{
    public interface IMovie
    {
        int Id { get; set; }
        string Title { get; set; }
        int Year { get; set; }
        MovieGenre Genre { get; set; }
        int StudioId { get; set; }
        int DirectorId { get; set; }
    }
}
