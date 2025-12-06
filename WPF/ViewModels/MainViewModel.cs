using GrobelnyKasprzak.MovieCatalogue.Services;

namespace WPF.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public MoviesViewModel MoviesVM { get; }
        public DirectorsViewModel DirectorsVM { get; }

        public MainViewModel(MovieService movieService, DirectorService directorService)
        {
            MoviesVM = new MoviesViewModel(movieService);
            DirectorsVM = new DirectorsViewModel(directorService);
        }
    }
}
