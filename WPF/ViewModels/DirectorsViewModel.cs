using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using GrobelnyKasprzak.MovieCatalogue.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Commands;

namespace WPF.ViewModels
{
    public class DirectorsViewModel : ViewModel
    {
        private readonly DirectorService _service;
        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }


        public ObservableCollection<IDirector> Directors { get; } = new();

        private IDirector? _selectedDirector;
        public IDirector? SelectedDirector
        {
            get => _selectedDirector;
            set { _selectedDirector = value; OnPropertyChanged(); }
        }

        public DirectorsViewModel(DirectorService service)
        {
            AddCommand = new RelayCommand(_ => AddDirector());
            EditCommand = new RelayCommand(_ => EditDirector(), _ => SelectedDirector != null);
            DeleteCommand = new RelayCommand(_ => DeleteDirector(), _ => SelectedDirector != null);
            _service = service;
            LoadDirectors();
        }

        private void AddDirector()
        {
            var d = _service.CreateNewDirector();
            d.Name = "New Director";

            _service.AddDirector(d);
            LoadDirectors();
        }

        private void EditDirector()
        {
            if (SelectedDirector == null) return;

            SelectedDirector.Name += " *edited*";
            _service.UpdateDirector(SelectedDirector);

            LoadDirectors();
        }

        private void DeleteDirector()
        {
            if (SelectedDirector == null) return;

            _service.DeleteDirector(SelectedDirector.Id);
            LoadDirectors();
        }


        public void LoadDirectors()
        {
            Directors.Clear();
            foreach (var d in _service.GetAllDirectors())
                Directors.Add(d);
        }
    }
}
