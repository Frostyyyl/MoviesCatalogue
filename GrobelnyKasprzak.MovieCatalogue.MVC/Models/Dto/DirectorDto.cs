using GrobelnyKasprzak.MovieCatalogue.Interfaces;

namespace GrobelnyKasprzak.MovieCatalogue.MVC.Models.Dto;

public class DirectorDto : IDirector
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
