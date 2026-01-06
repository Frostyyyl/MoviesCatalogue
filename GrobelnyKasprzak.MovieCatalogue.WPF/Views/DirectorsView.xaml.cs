using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace GrobelnyKasprzak.MovieCatalogue.WPF.Views
{
    public partial class DirectorsView : UserControl
    {
        private static readonly Regex _regex = new Regex("[^0-9]+");

        public DirectorsView()
        {
            InitializeComponent();
        }

        private void BirthYear_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = _regex.IsMatch(e.Text);
        }
    }
}