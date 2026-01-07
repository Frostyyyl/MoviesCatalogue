using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GrobelnyKasprzak.MovieCatalogue.Services
{
    public static class ReflectionLoader
    {
        public static void RegisterDao(IServiceCollection services, IConfiguration config)
        {
            string activeDao = config["DaoConfig:ActiveDao"] ?? throw new Exception("Brak ActiveDao");
            string dllName = config[$"DaoConfig:{activeDao}:Dll"] ?? throw new Exception("Brak Dll");
            string className = config[$"DaoConfig:{activeDao}:Class"] ?? throw new Exception("Brak Class");

            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dllName);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Brak pliku: {fullPath}");

            Assembly daoAssembly = Assembly.LoadFrom(fullPath);
            Type? moduleType = daoAssembly.GetType(className);

            if (moduleType == null || !typeof(IDaoModule).IsAssignableFrom(moduleType))
                throw new Exception($"Klasa {className} nie implementuje IDaoModule.");

            var instance = Activator.CreateInstance(moduleType) as IDaoModule;
            instance?.Register(services);
        }
    }
}