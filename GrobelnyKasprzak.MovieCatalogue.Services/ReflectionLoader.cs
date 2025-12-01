using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace GrobelnyKasprzak.MovieCatalogue.Services
{
    public class ReflectionLoader
    {
        private readonly Assembly _daoAssembly;
        public ReflectionLoader()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();

            string? dllName = config["AppSettings:DaoLibrary"];

            if (string.IsNullOrEmpty(dllName))
            {
                throw new Exception("Nie skonfigurowano nazwy biblioteki DAO w appsettings.json");
            }

            string path = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.Combine(path, dllName);

            if (File.Exists(fullPath))
            {
                _daoAssembly = Assembly.LoadFrom(fullPath);
            }
            else
            {
                throw new FileNotFoundException($"Nie znaleziono pliku biblioteki: {fullPath}");
            }
        }

        public T GetRepository<T>() where T : class
        {
            Type? typeToCreate = null;

            foreach (var t in _daoAssembly.GetTypes())
            {
                if (typeof(T).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                {
                    typeToCreate = t;
                    break;
                }
            }

            if (typeToCreate == null)
            {
                throw new Exception($"Nie znaleziono implementacji {typeof(T).Name} w bibliotece.");
            }

            var instance = Activator.CreateInstance(typeToCreate);

            return instance == null ? throw new Exception($"Nie znaleziono implementacji {typeof(T).Name} w bibliotece.") : (T)instance;
        }
    }
}
