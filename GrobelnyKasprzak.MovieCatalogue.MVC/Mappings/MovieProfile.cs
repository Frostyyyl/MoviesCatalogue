using AutoMapper;
using GrobelnyKasprzak.MovieCatalogue.Core;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using GrobelnyKasprzak.MovieCatalogue.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GrobelnyKasprzak.MovieCatalogue.MVC.Mappings;

public class MovieProfile : Profile
{
    public MovieProfile()
    {
        CreateMap<IMovie, MovieViewModel>()
            .ForMember(dest => dest.Director, opt => opt.MapFrom((src, dest, _, ctx)
                => ctx.Items.TryGetValue(MappingKeys.DirectorName, out var name)
                    ? name : "Unknown"))
            .ForMember(dest => dest.AvailableDirectors, opt => opt.MapFrom((src, dest, _, ctx) =>
            {
                if (ctx.TryGetItems(out var items) &&
                    items.TryGetValue(MappingKeys.AvailableDirectors, out var directorsObj) &&
                    directorsObj is IEnumerable<IDirector> directors)
                {
                    return ctx.Mapper.Map<IEnumerable<SelectListItem>>(directors);
                }
                return [];
            }))
            .ForMember(dest => dest.AvailableGenres, opt => opt.MapFrom((src, dest, _, ctx) =>
            {
                if (ctx.TryGetItems(out var items) &&
                    items.TryGetValue(MappingKeys.AvailableGenres, out var genresObj) &&
                    genresObj is IEnumerable<MovieGenre> genres)
                {
                    return ctx.Mapper.Map<IEnumerable<SelectListItem>>(genres);
                }
                return [];
            }));
    }
}
