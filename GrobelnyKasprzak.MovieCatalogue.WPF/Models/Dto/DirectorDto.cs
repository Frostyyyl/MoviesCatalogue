using GrobelnyKasprzak.MovieCatalogue.WPF.ViewModels;

public class DirectorDto : ViewModel
{
    private string _name = "";
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    private int _birthYear;
    public int BirthYear
    {
        get => _birthYear;
        set
        {
            _birthYear = value;
            OnPropertyChanged();
        }
    }

    public int Id { get; set; }
}
