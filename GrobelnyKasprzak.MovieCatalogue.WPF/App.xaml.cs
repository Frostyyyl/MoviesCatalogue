using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using GrobelnyKasprzak.MovieCatalogue.Services;
using GrobelnyKasprzak.MovieCatalogue.WPF.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace GrobelnyKasprzak.MovieCatalogue.WPF
{
    public partial class App : Application
    {
        public static ServiceProvider? Services { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                var services = new ServiceCollection();

                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

                ReflectionLoader.RegisterDao(services, config);

                services.AddSingleton<IMovieService, MovieService>();
                services.AddSingleton<IDirectorService, DirectorService>();
                services.AddSingleton<MainViewModel>();

                Services = services.BuildServiceProvider();

                var mainWindow = new MainWindow
                {
                    DataContext = Services.GetRequiredService<MainViewModel>()
                };

                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd startu aplikacji: {ex.Message}\n\n{ex.StackTrace}",
                    "Błąd krytyczny", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }
    }
}
