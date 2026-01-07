using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GrobelnyKasprzak.MovieCatalogue.DAO.Mock;

public class MockModule : IDaoModule
{
    public void Register(IServiceCollection services)
    {
        services.AddScoped<IMovieRepository, MovieRepositoryMock>();
        services.AddScoped<IDirectorRepository, DirectorRepositoryMock>();
    }
}
