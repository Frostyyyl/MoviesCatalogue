using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using GrobelnyKasprzak.MovieCatalogue.Services;
using System.Collections.ObjectModel;
using WPF.Commands;

namespace WPF.ViewModels
{
    public class MoviesViewModel : ViewModel
    {
        private readonly MovieService _service;
        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }


        public ObservableCollection<IMovie> Movies { get; } = new();

        private IMovie? _selectedMovie;
        public IMovie? SelectedMovie
        {
            get => _selectedMovie;
            set { _selectedMovie = value; OnPropertyChanged(); }
        }

        public MoviesViewModel(MovieService service)
        {
            AddCommand = new RelayCommand(_ => AddMovie());
            EditCommand = new RelayCommand(_ => EditMovie(), _ => SelectedMovie != null);
            DeleteCommand = new RelayCommand(_ => DeleteMovie(), _ => SelectedMovie != null);
            _service = service;
            LoadMovies();
        }

        public void LoadMovies()
        {
            Movies.Clear();
            foreach (var m in _service.GetAllMovies())
                Movies.Add(m);
        }

        private void AddMovie()
        {
            var movie = _service.CreateNewMovie();
            movie.Title = "New Movie";

            _service.AddMovie(movie);
            LoadMovies();
        }

        private void EditMovie()
        {
            if (SelectedMovie == null) return;

            SelectedMovie.Title += " *edited*";
            _service.UpdateMovie(SelectedMovie);

            LoadMovies();
        }

        private void DeleteMovie()
        {
            if (SelectedMovie == null) return;

            _service.DeleteMovie(SelectedMovie.Id);
            LoadMovies();
        }

    }
}
