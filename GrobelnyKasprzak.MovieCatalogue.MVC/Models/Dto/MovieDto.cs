using GrobelnyKasprzak.MovieCatalogue.Core;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;

namespace GrobelnyKasprzak.MovieCatalogue.MVC.Models.Dto;

internal class MovieDto : IMovie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Year { get; set; }
    public MovieGenre Genre { get; set; }
    public int DirectorId { get; set; }
}

