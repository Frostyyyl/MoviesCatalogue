using Microsoft.Extensions.DependencyInjection;

namespace GrobelnyKasprzak.MovieCatalogue.Interfaces;

public interface IDaoModule
{
    void Register(IServiceCollection services);
}
