using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace GrobelnyKasprzak.MovieCatalogue.WPF.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected void RefreshValidation()
        {
            OnPropertyChanged(string.Empty);
            CommandManager.InvalidateRequerySuggested();
        }

    }
}
