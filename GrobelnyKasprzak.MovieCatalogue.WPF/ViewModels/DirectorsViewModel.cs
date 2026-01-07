using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using GrobelnyKasprzak.MovieCatalogue.WPF.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace GrobelnyKasprzak.MovieCatalogue.WPF.ViewModels
{
    public class DirectorsViewModel : ViewModel, IDataErrorInfo
    {
        private readonly IDirectorService _directorService;
        private readonly ICollectionView _directorsView;
        public ICommand UpdateCommand { get; }
        public ICommand CreateCommand { get; }
        public ICommand DeleteCommand { get; }

        private bool _editNameTouched;
        private bool _editYearTouched;
        private bool _newNameTouched;
        private bool _newYearTouched;

        public static event Action? DirectorsChanged;
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
                _directorsView.Refresh();
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
                SelectedDirector = null;
                ResetForms();
                RefreshValidation();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private DirectorDto? _selectedDirector;
        public DirectorDto? SelectedDirector
        {
            get => _selectedDirector;
            set
            {
                _selectedDirector = value;
                OnPropertyChanged();

                if (_selectedDirector != null)
                {
                    EditName = _selectedDirector.Name;
                    EditBirthYear = _selectedDirector.BirthYear;
                    _editNameTouched = false;
                    _editYearTouched = false;
                }
                else
                {
                    EditName = string.Empty;
                    EditBirthYear = null;
                }

                CommandManager.InvalidateRequerySuggested();
            }
        }

        // Form (Edit)
        private string _editName = string.Empty;
        public string EditName
        {
            get => _editName;
            set
            {
                _editName = value;
                _editNameTouched = true;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private int? _editBirthYear;
        public int? EditBirthYear
        {
            get => _editBirthYear;
            set
            {
                _editBirthYear = value;
                _editYearTouched = true;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        // Form (New)
        private string _newName = string.Empty;
        public string NewName
        {
            get => _newName;
            set
            {
                _newName = value;
                _newNameTouched = true;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private int? _newBirthYear;
        public int? NewBirthYear
        {
            get => _newBirthYear;
            set
            {
                _newBirthYear = value;
                _newYearTouched = true;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        // Constructor
        public DirectorsViewModel(IDirectorService directorService)
        {
            _directorService = directorService;

            Directors = new ObservableCollection<DirectorDto>();

            _directorsView = CollectionViewSource.GetDefaultView(Directors);
            _directorsView.Filter = FilterDirectors;

            LoadDirectors();

            UpdateCommand = new RelayCommand(UpdateDirector, _ => SelectedDirector != null && IsEditValid);
            CreateCommand = new RelayCommand(CreateDirector, _ => IsNewValid);
            DeleteCommand = new RelayCommand(DeleteDirector);
        }

        private void ResetForms()
        {
            _newNameTouched = _newYearTouched = _editNameTouched = _editYearTouched = false;

            NewName = string.Empty;
            NewBirthYear = null;

            EditName = string.Empty;
            EditBirthYear = null;
        }

        private bool FilterDirectors(object obj)
        {
            if (obj is not DirectorDto director)
                return false;

            // name filter
            if (string.IsNullOrWhiteSpace(SearchText))
                return true;

            return director.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        }


        // CRUD
        private void CreateDirector(object? _)
        {
            try
            {
                var director = _directorService.CreateNewDirector();
                director.Name = NewName;
                director.BirthYear = NewBirthYear ?? 0;

                _directorService.AddDirector(director);
                LoadDirectors();
                ResetForms();

                DirectorsChanged?.Invoke();
                MessageBox.Show("Director added successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateDirector(object? _)
        {
            if (SelectedDirector == null) return;

            try
            {
                var director = _directorService.GetDirectorById(SelectedDirector.Id);
                if (director != null)
                {
                    director.Name = EditName;
                    director.BirthYear = EditBirthYear ?? 0;
                    _directorService.UpdateDirector(director);

                    int index = Directors.IndexOf(SelectedDirector);
                    Directors[index] = new DirectorDto { Id = SelectedDirector.Id, Name = EditName, BirthYear = EditBirthYear!.Value };
                    SelectedDirector = Directors[index];
                    _editNameTouched = _editYearTouched = false;

                    DirectorsChanged?.Invoke();
                    MessageBox.Show("Updated successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteDirector(object? _)
        {
            if (SelectedDirector == null) return;
            try
            {
                _directorService.DeleteDirector(SelectedDirector.Id);
                Directors.Remove(SelectedDirector);
                SelectedDirector = null;
                ResetForms();
                DirectorsChanged?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadDirectors()
        {
            Directors.Clear();

            var directors = _directorService.GetAllDirectors();

            foreach (var director in directors)
            {
                Directors.Add(new DirectorDto
                {
                    Id = director.Id,
                    Name = director.Name,
                    BirthYear = director.BirthYear
                });
            }

            _directorsView.Refresh();
        }


        // Validation
        public bool IsEditValid =>
            SelectedDirector != null &&
            _directorService.ValidateName(EditName) == null &&
            _directorService.ValidateBirthYear(EditBirthYear) == null;

        public bool IsNewValid =>
            _directorService.ValidateName(NewName) == null &&
            _directorService.ValidateBirthYear(NewBirthYear) == null;

        public string Error => string.Empty;
        public string this[string columnName]
        {
            get
            {
                ValidationResult? result = null;

                // NEW
                if (columnName == nameof(NewName))
                {
                    if (!_newNameTouched) return string.Empty;
                    result = _directorService.ValidateName(NewName);
                }
                else if (columnName == nameof(NewBirthYear))
                {
                    if (!_newYearTouched) return string.Empty;
                    result = _directorService.ValidateBirthYear(NewBirthYear);
                }

                // EDIT
                else if (columnName == nameof(EditName))
                {
                    if (!_editNameTouched) return string.Empty;
                    result = _directorService.ValidateName(EditName);
                }
                else if (columnName == nameof(EditBirthYear))
                {
                    if (!_editYearTouched) return string.Empty;
                    result = _directorService.ValidateBirthYear(EditBirthYear);
                }

                return result?.ErrorMessage ?? string.Empty;
            }
        }
    }
}