using AutoMapper;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using GrobelnyKasprzak.MovieCatalogue.MVC.ViewModels;

namespace GrobelnyKasprzak.MovieCatalogue.MVC.Mappings;

public class DirectorProfile : Profile
{
    public DirectorProfile()
    {
        CreateMap<IDirector, DirectorViewModel>()
            .ForMember(dest => dest.Movies, opt => opt.MapFrom((src, dest, _, ctx) =>
            {
                if (ctx.TryGetItems(out var items) &&
                    items.TryGetValue(MappingKeys.Movies, out var moviesObj) &&
                    moviesObj is IEnumerable<IMovie> movies)
                {
                    return ctx.Mapper.Map<IEnumerable<MovieListItemViewModel>>(movies);
                }

                return [];
            }));
    }
}
