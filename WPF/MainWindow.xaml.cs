using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using GrobelnyKasprzak.MovieCatalogue.Services;
using WPF.ViewModels;
using System.Windows;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var loader = new ReflectionLoader();

            var movieRepo = loader.GetRepository<IMovieRepository>();
            var directorRepo = loader.GetRepository<IDirectorRepository>();

            var movieService = new MovieService(movieRepo);
            var directorService = new DirectorService(directorRepo);

            DataContext = new MainViewModel(movieService, directorService);
        }
    }
}