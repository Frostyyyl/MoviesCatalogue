using GrobelnyKasprzak.MovieCatalogue.Services;

namespace WPF.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public MoviesViewModel MoviesVM { get; }
        public DirectorsViewModel DirectorsVM { get; }

        public MainViewModel()
        {
            var movieService = new MovieService();
            var directorService = new DirectorService();

            MoviesVM = new MoviesViewModel(movieService, directorService);
            DirectorsVM = new DirectorsViewModel(directorService, movieService);
        }
    }
}
