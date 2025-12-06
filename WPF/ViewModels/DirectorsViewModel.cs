using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using GrobelnyKasprzak.MovieCatalogue.Services;
using System.Collections.ObjectModel;
using System.Windows;
using WPF.Commands;

namespace WPF.ViewModels
{
    public class DirectorsViewModel : ViewModel
    {
        private readonly DirectorService _directorService;
        private readonly MovieService _movieService;

        private ObservableCollection<IDirector> _directors;
        private ObservableCollection<IDirector> _allDirectors;
        private IDirector? _selectedDirector;
        private string _searchText = string.Empty;
        private string _name = string.Empty;

        public DirectorsViewModel(DirectorService directorService, MovieService movieService)
        {
            _directorService = directorService;
            _movieService = movieService;
            _directors = new ObservableCollection<IDirector>();
            _allDirectors = new ObservableCollection<IDirector>();

            AddCommand = new RelayCommand(_ => AddDirector(), _ => !string.IsNullOrWhiteSpace(Name));
            UpdateCommand = new RelayCommand(_ => UpdateDirector(), _ => SelectedDirector != null && !string.IsNullOrWhiteSpace(Name));
            DeleteCommand = new RelayCommand(_ => DeleteDirector(), _ => SelectedDirector != null);
            ClearCommand = new RelayCommand(_ => ClearForm());

            LoadDirectors();
        }

        public ObservableCollection<IDirector> Directors
        {
            get => _directors;
            set { _directors = value; OnPropertyChanged(); }
        }

        public IDirector? SelectedDirector
        {
            get => _selectedDirector;
            set
            {
                _selectedDirector = value;
                OnPropertyChanged();
                if (value != null) Name = value.Name;
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                SearchDirectors();
            }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public RelayCommand AddCommand { get; }
        public RelayCommand UpdateCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand ClearCommand { get; }

        public void LoadDirectors()
        {
            _allDirectors.Clear();
            foreach (var d in _directorService.GetAllDirectors())
                _allDirectors.Add(d);
            SearchDirectors();
        }

        private void SearchDirectors()
        {
            Directors.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allDirectors
                : _allDirectors.Where(d => d.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            foreach (var d in filtered)
                Directors.Add(d);
        }

        private void AddDirector()
        {
            try
            {
                var d = _directorService.CreateNewDirector();
                d.Name = Name;
                _directorService.AddDirector(d);
                LoadDirectors();
                ClearForm();
                MessageBox.Show("Reżyser dodany", "Sukces");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message}", "Błąd");
            }
        }

        private void UpdateDirector()
        {
            try
            {
                if (SelectedDirector == null) return;
                SelectedDirector.Name = Name;
                _directorService.UpdateDirector(SelectedDirector);
                LoadDirectors();
                ClearForm();
                MessageBox.Show("Reżyser zaktualizowany", "Sukces");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message}", "Błąd");
            }
        }

        private void DeleteDirector()
        {
            try
            {
                if (SelectedDirector == null) return;
                var hasMovies = _movieService.GetAllMovies().Any(m => m.DirectorId == SelectedDirector.Id);
                if (hasMovies)
                {
                    MessageBox.Show("Nie można usunąć reżysera z filmami.", "Błąd");
                    return;
                }
                if (MessageBox.Show($"Usunąć '{SelectedDirector.Name}'?", "Potwierdzenie", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _directorService.DeleteDirector(SelectedDirector.Id);
                    LoadDirectors();
                    ClearForm();
                    MessageBox.Show("Reżyser usunięty", "Sukces");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message}", "Błąd");
            }
        }

        private void ClearForm()
        {
            Name = string.Empty;
            SelectedDirector = null;
        }
    }
}
