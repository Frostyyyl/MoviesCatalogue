using GrobelnyKasprzak.MovieCatalogue.Core;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GrobelnyKasprzak.MovieCatalogue.MVC.ViewModels;

public class MovieViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Year { get; set; }
    public MovieGenre Genre { get; set; }
    public int DirectorId { get; set; }
    public string Director { get; set; } = string.Empty;
    public IEnumerable<SelectListItem> AvailableDirectors { get; set; } = [];
    public IEnumerable<SelectListItem> AvailableGenres { get; set; } = [];
}
