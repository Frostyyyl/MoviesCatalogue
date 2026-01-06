using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrobelnyKasprzak.MovieCatalogue.WPF.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public MoviesViewModel MoviesVM { get; }
        public DirectorsViewModel DirectorsVM { get; }

        public MainViewModel(IMovieService movieService, IDirectorService directorService)
        {
            MoviesVM = new MoviesViewModel(movieService, directorService);
            DirectorsVM = new DirectorsViewModel(directorService, movieService);
        }
    }
}
