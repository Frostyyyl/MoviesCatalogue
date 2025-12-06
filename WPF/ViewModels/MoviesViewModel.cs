using GrobelnyKasprzak.MovieCatalogue.Core;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using GrobelnyKasprzak.MovieCatalogue.Services;
using System.Collections.ObjectModel;
using System.Windows;
using WPF.Commands;

namespace WPF.ViewModels
{
    public class MoviesViewModel : ViewModel
    {
        private readonly MovieService _movieService;
        private readonly DirectorService _directorService;

        private ObservableCollection<IMovie> _movies;
        private ObservableCollection<IMovie> _allMovies;
        private ObservableCollection<IDirector> _directors;

        private IMovie? _selectedMovie;
        private string _searchText = string.Empty;

        private string _title = string.Empty;
        private int _year = DateTime.Now.Year;
        private MovieGenre _genre = MovieGenre.Action;
        private IDirector? _selectedDirector;

        public MoviesViewModel(MovieService movieService, DirectorService directorService)
        {
            _movieService = movieService;
            _directorService = directorService;

            _movies = new ObservableCollection<IMovie>();
            _allMovies = new ObservableCollection<IMovie>();
            _directors = new ObservableCollection<IDirector>();

            AddCommand = new RelayCommand(_ => AddMovie(), _ => CanAddMovie());
            UpdateCommand = new RelayCommand(_ => UpdateMovie(), _ => CanUpdateMovie());
            DeleteCommand = new RelayCommand(_ => DeleteMovie(), _ => CanDeleteMovie());
            ClearCommand = new RelayCommand(_ => ClearForm());
            SearchCommand = new RelayCommand(_ => SearchMovies());

            LoadData();
        }

        public ObservableCollection<IMovie> Movies
        {
            get => _movies;
            set { _movies = value; OnPropertyChanged(); }
        }

        public ObservableCollection<IDirector> Directors
        {
            get => _directors;
            set { _directors = value; OnPropertyChanged(); }
        }

        public IMovie? SelectedMovie
        {
            get => _selectedMovie;
            set
            {
                _selectedMovie = value;
                OnPropertyChanged();
                if (value != null)
                {
                    Title = value.Title;
                    Year = value.Year;
                    Genre = value.Genre;
                    SelectedDirector = Directors.FirstOrDefault(d => d.Id == value.DirectorId);
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                SearchMovies();
            }
        }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public int Year
        {
            get => _year;
            set { _year = value; OnPropertyChanged(); }
        }

        public MovieGenre Genre
        {
            get => _genre;
            set { _genre = value; OnPropertyChanged(); }
        }

        public IDirector? SelectedDirector
        {
            get => _selectedDirector;
            set { _selectedDirector = value; OnPropertyChanged(); }
        }

        public Array Genres => Enum.GetValues(typeof(MovieGenre));

        public RelayCommand AddCommand { get; }
        public RelayCommand UpdateCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand ClearCommand { get; }
        public RelayCommand SearchCommand { get; }

        private void LoadData()
        {
            LoadDirectors();
            LoadMovies();
        }

        private void LoadMovies()
        {
            _allMovies.Clear();
            var movies = _movieService.GetAllMovies();
            foreach (var movie in movies)
            {
                _allMovies.Add(movie);
            }
            SearchMovies();
        }

        private void LoadDirectors()
        {
            Directors.Clear();
            var directors = _directorService.GetAllDirectors();
            foreach (var director in directors)
            {
                Directors.Add(director);
            }
        }

        private void SearchMovies()
        {
            Movies.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allMovies
                : _allMovies.Where(m => m.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            foreach (var movie in filtered)
            {
                Movies.Add(movie);
            }
        }

        private bool CanAddMovie() => !string.IsNullOrWhiteSpace(Title) && SelectedDirector != null;

        private void AddMovie()
        {
            try
            {
                var movie = _movieService.CreateNewMovie();
                movie.Title = Title;
                movie.Year = Year;
                movie.Genre = Genre;
                movie.DirectorId = SelectedDirector!.Id;

                _movieService.AddMovie(movie);
                LoadMovies();
                ClearForm();
                MessageBox.Show("Film dodany pomyślnie", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanUpdateMovie() => SelectedMovie != null && !string.IsNullOrWhiteSpace(Title) && SelectedDirector != null;

        private void UpdateMovie()
        {
            try
            {
                if (SelectedMovie == null) return;

                SelectedMovie.Title = Title;
                SelectedMovie.Year = Year;
                SelectedMovie.Genre = Genre;
                SelectedMovie.DirectorId = SelectedDirector!.Id;

                _movieService.UpdateMovie(SelectedMovie);
                LoadMovies();
                ClearForm();
                MessageBox.Show("Film zaktualizowany pomyślnie", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanDeleteMovie() => SelectedMovie != null;

        private void DeleteMovie()
        {
            try
            {
                if (SelectedMovie == null) return;

                var result = MessageBox.Show(
                    $"Czy na pewno usunąć film '{SelectedMovie.Title}'?",
                    "Potwierdzenie",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _movieService.DeleteMovie(SelectedMovie.Id);
                    LoadMovies();
                    ClearForm();
                    MessageBox.Show("Film usunięty pomyślnie", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            Title = string.Empty;
            Year = DateTime.Now.Year;
            Genre = MovieGenre.Action;
            SelectedDirector = null;
            SelectedMovie = null;
        }
    }
}
