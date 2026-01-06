using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using GrobelnyKasprzak.MovieCatalogue.WPF.Commands;
using GrobelnyKasprzak.MovieCatalogue.WPF.Models.Dto;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace GrobelnyKasprzak.MovieCatalogue.WPF.ViewModels
{
    public class MoviesViewModel : ViewModel, IDataErrorInfo
    {
        private readonly IMovieService _movieService;
        private readonly IDirectorService _directorService;
        private readonly ICollectionView _moviesView;
        private bool _editTitleTouched;
        private bool _editYearTouched;
        private bool _newTitleTouched;
        private bool _newYearTouched;
        public ICommand UpdateCommand { get; }
        public ICommand CreateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ObservableCollection<MovieDto> Movies { get; }
        public ObservableCollection<DirectorDto> Directors { get; }

        // State and search
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                _moviesView.Refresh();
            }
        }

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged();
                SelectedMovie = null;
                ResetForms();
            }
        }

        private MovieDto? _selectedMovie;
        public MovieDto? SelectedMovie
        {
            get => _selectedMovie;
            set
            {
                _selectedMovie = value;
                OnPropertyChanged();
                if (_selectedMovie != null)
                {
                    EditTitle = _selectedMovie.Title;
                    EditYear = _selectedMovie.Year;
                    EditDirectorId = _selectedMovie.DirectorId;
                    _editTitleTouched = false;
                    _editYearTouched = false;
                }
                CommandManager.InvalidateRequerySuggested();
            }
        }

        // Form (Edit)
        private string _editTitle = string.Empty;
        public string EditTitle { get => _editTitle; set { _editTitle = value; _editTitleTouched = true; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); } }

        private int? _editYear;
        public int? EditYear { get => _editYear; set { _editYear = value; _editYearTouched = true; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); } }

        private int? _editDirectorId;
        public int? EditDirectorId { get => _editDirectorId; set { _editDirectorId = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); } }

        // Form (New)
        private string _newTitle = string.Empty;
        public string NewTitle { get => _newTitle; set { _newTitle = value; _newTitleTouched = true; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); } }

        private int? _newYear;
        public int? NewYear { get => _newYear; set { _newYear = value; _newYearTouched = true; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); } }

        private int? _newDirectorId;
        public int? NewDirectorId { get => _newDirectorId; set { _newDirectorId = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); } }

        // Constructor
        public MoviesViewModel(IMovieService movieService, IDirectorService directorService)
        {
            _movieService = movieService;
            _directorService = directorService;

            Directors = new ObservableCollection<DirectorDto>();
            Movies = new ObservableCollection<MovieDto>();

            _moviesView = CollectionViewSource.GetDefaultView(Movies);
            _moviesView.Filter = FilterMovies;

            RefreshDirectorsData();
            RefreshMoviesData();

            DirectorsViewModel.DirectorsChanged += () => Application.Current.Dispatcher.Invoke(RefreshDirectorsData);

            UpdateCommand = new RelayCommand(UpdateMovie, _ => SelectedMovie != null && IsEditValid);
            CreateCommand = new RelayCommand(CreateMovie, _ => IsNewValid);
            DeleteCommand = new RelayCommand(DeleteMovie, _ => SelectedMovie != null);
        }

        private bool FilterMovies(object obj)
        {
            if (obj is not MovieDto movie) return false;
            if (string.IsNullOrWhiteSpace(SearchText)) return true;

            return movie.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                   movie.DirectorName.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        }

        private void ResetForms()
        {
            _newTitleTouched = _newYearTouched = _editTitleTouched = _editYearTouched = false;
            NewTitle = string.Empty;
            NewYear = null;
            NewDirectorId = null;
            EditTitle = string.Empty;
            EditYear = null;
            EditDirectorId = null;
            OnPropertyChanged(string.Empty);
        }

        private void RefreshDirectorsData()
        {
            var data = _directorService.GetAllDirectors().Select(d => new DirectorDto { Id = d.Id, Name = d.Name }).ToList();
            Directors.Clear();
            foreach (var d in data) Directors.Add(d);
        }

        private void RefreshMoviesData()
        {
            var dict = _directorService.GetAllDirectors().ToDictionary(d => d.Id, d => d.Name);
            var data = _movieService.GetAllMovies().Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Year = m.Year,
                DirectorId = m.DirectorId,
                DirectorName = dict.TryGetValue(m.DirectorId, out var n) ? n : "Unknown"
            }).ToList();
            Movies.Clear();
            foreach (var m in data) Movies.Add(m);
        }

        // CRUD
        private void CreateMovie(object? _)
        {
            try
            {
                var movie = _movieService.CreateNewMovie();
                movie.Title = NewTitle;
                movie.Year = NewYear ?? 0;
                movie.DirectorId = NewDirectorId ?? 0;
                _movieService.AddMovie(movie);

                RefreshMoviesData();
                ResetForms();
                MessageBox.Show("Movie added successfully!");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void UpdateMovie(object? _)
        {
            if (SelectedMovie == null) return;
            try
            {
                var movie = _movieService.GetMovieById(SelectedMovie.Id);
                if (movie != null)
                {
                    movie.Title = EditTitle;
                    movie.Year = EditYear ?? 0;
                    movie.DirectorId = EditDirectorId ?? 0;
                    _movieService.UpdateMovie(movie);
                    RefreshMoviesData();
                    MessageBox.Show("Updated successfully!");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void DeleteMovie(object? _)
        {
            if (SelectedMovie == null) return;
            try
            {
                _movieService.DeleteMovie(SelectedMovie.Id);
                Movies.Remove(SelectedMovie);
                SelectedMovie = null;
                ResetForms();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // Validation
        public bool IsEditValid => !string.IsNullOrWhiteSpace(EditTitle) && EditYear >= 1850 && EditYear <= 2026 && EditDirectorId > 0;
        public bool IsNewValid => !string.IsNullOrWhiteSpace(NewTitle) && NewYear >= 1850 && NewYear <= 2026 && NewDirectorId > 0;

        public string Error => string.Empty;
        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(NewTitle))
                {
                    if (!_newTitleTouched) return string.Empty;
                    if (string.IsNullOrWhiteSpace(NewTitle)) return "Title is required.";
                }
                if (columnName == nameof(NewYear))
                {
                    if (!_newYearTouched) return string.Empty;
                    if (NewYear == null) return "Year is required.";
                    if (NewYear < 1850 || NewYear > 2026) return "Year must be 1850-2026.";
                }

                if (columnName == nameof(EditTitle))
                {
                    if (!_editTitleTouched) return string.Empty;
                    if (string.IsNullOrWhiteSpace(EditTitle)) return "Title is required.";
                }
                if (columnName == nameof(EditYear))
                {
                    if (!_editYearTouched) return string.Empty;
                    if (EditYear == null) return "Year is required.";
                    if (EditYear < 1850 || EditYear > 2026) return "Year must be 1850-2026.";
                }
                return string.Empty;
            }
        }
    }
}