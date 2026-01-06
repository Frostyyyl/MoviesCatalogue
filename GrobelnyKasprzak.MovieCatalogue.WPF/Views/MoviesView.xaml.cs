using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GrobelnyKasprzak.MovieCatalogue.WPF.Views
{
    /// <summary>
    /// Logika interakcji dla klasy MoviesView.xaml
    /// </summary>
    public partial class MoviesView : UserControl
    {
        private static readonly Regex _regex = new Regex("[^0-9]+");
        public MoviesView()
        {
            InitializeComponent();
        }

        private void Year_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = _regex.IsMatch(e.Text);
        }
    }
}
