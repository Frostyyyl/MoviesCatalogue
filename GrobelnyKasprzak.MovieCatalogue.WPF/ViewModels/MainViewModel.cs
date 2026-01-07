using GrobelnyKasprzak.MovieCatalogue.Interfaces;

namespace GrobelnyKasprzak.MovieCatalogue.WPF.ViewModels
{
    public class MainViewModel(IMovieService movieService, IDirectorService directorService) : ViewModel
    {
        public MoviesViewModel MoviesVM { get; } = new MoviesViewModel(movieService, directorService);
        public DirectorsViewModel DirectorsVM { get; } = new DirectorsViewModel(directorService);
    }
}
