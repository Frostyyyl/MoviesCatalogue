using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using GrobelnyKasprzak.MovieCatalogue.WPF.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace GrobelnyKasprzak.MovieCatalogue.WPF.ViewModels
{
    public class DirectorsViewModel : ViewModel, IDataErrorInfo
    {
        private readonly IDirectorService _directorService;
        private readonly IMovieService _movieService;
        public ICommand UpdateCommand { get; }
        public ICommand CreateCommand { get; }
        public ICommand DeleteCommand { get; }
        private bool _editNameTouched;
        private bool _editYearTouched;
        private bool _newNameTouched;
        private bool _newYearTouched;

        public static event Action? DirectorsChanged;
        public ObservableCollection<DirectorDto> Directors { get; }

        // State
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
            set { _editName = value; _editNameTouched = true; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        private int? _editBirthYear;
        public int? EditBirthYear
        {
            get => _editBirthYear;
            set { _editBirthYear = value; _editYearTouched = true; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        // Form (New)
        private string _newName = string.Empty;
        public string NewName
        {
            get => _newName;
            set { _newName = value; _newNameTouched = true; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        private int? _newBirthYear;
        public int? NewBirthYear
        {
            get => _newBirthYear;
            set { _newBirthYear = value; _newYearTouched = true; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        // Constructor
        public DirectorsViewModel(IDirectorService directorService, IMovieService movieService)
        {
            _directorService = directorService;
            _movieService = movieService;
            Directors = new ObservableCollection<DirectorDto>(_directorService.GetAllDirectors().Select(d => new DirectorDto { Id = d.Id, Name = d.Name, BirthYear = d.BirthYear }));

            UpdateCommand = new RelayCommand(UpdateDirector, _ => SelectedDirector != null && IsEditValid);
            CreateCommand = new RelayCommand(CreateDirector, _ => IsNewValid);
            DeleteCommand = new RelayCommand(DeleteDirector, _ => CanDelete);
        }

        private void ResetForms()
        {
            _newNameTouched = _newYearTouched = _editNameTouched = _editYearTouched = false;
            NewName = string.Empty;
            NewBirthYear = null;
            EditName = string.Empty;
            EditBirthYear = null;
            OnPropertyChanged(string.Empty);
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

                int nextId = (Directors.Count > 0) ? Directors.Max(d => d.Id) + 1 : 1;
                Directors.Add(new DirectorDto { Id = director.Id > 0 ? director.Id : nextId, Name = NewName, BirthYear = NewBirthYear!.Value });

                ResetForms();
                DirectorsChanged?.Invoke();
                MessageBox.Show("Director added successfully!");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
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
            catch (Exception ex) { MessageBox.Show(ex.Message); }
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
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // Validation
        public bool IsEditValid => !string.IsNullOrWhiteSpace(EditName) && EditBirthYear >= 1850 && EditBirthYear <= 2026;
        public bool IsNewValid => !string.IsNullOrWhiteSpace(NewName) && NewBirthYear >= 1850 && NewBirthYear <= 2026;
        public bool CanDelete => SelectedDirector != null && !_movieService.GetAllMovies().Any(m => m.DirectorId == SelectedDirector.Id);

        public string Error => string.Empty;
        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(NewName))
                {
                    if (!_newNameTouched) return string.Empty;
                    if (string.IsNullOrWhiteSpace(NewName)) return "Surname is required.";
                }
                if (columnName == nameof(NewBirthYear))
                {
                    if (!_newYearTouched) return string.Empty;
                    if (NewBirthYear == null) return "Year is required.";
                    if (NewBirthYear < 1850 || NewBirthYear > 2026) return "Year must be 1850-2026.";
                }

                if (columnName == nameof(EditName))
                {
                    if (!_editNameTouched) return string.Empty;
                    if (string.IsNullOrWhiteSpace(EditName)) return "Surname is required.";
                }
                if (columnName == nameof(EditBirthYear))
                {
                    if (!_editYearTouched) return string.Empty;
                    if (EditBirthYear == null) return "Year is required.";
                    if (EditBirthYear < 1850 || EditBirthYear > 2026) return "Year must be 1850-2026.";
                }

                return string.Empty;
            }
        }
    }
}