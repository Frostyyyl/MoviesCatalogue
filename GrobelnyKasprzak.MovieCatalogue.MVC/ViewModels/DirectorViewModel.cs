namespace GrobelnyKasprzak.MovieCatalogue.MVC.ViewModels;

public class DirectorViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IEnumerable<MovieListItemViewModel> Movies { get; set; } = [];
}

