using AutoMapper;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using GrobelnyKasprzak.MovieCatalogue.MVC.ViewModels;
namespace GrobelnyKasprzak.MovieCatalogue.MVC.Mappings;

public class MovieProfile : Profile
{
    public MovieProfile()
    {
        CreateMap<IMovie, MovieViewModel>()
            .ForMember(dest => dest.Studio,
                opt => opt.MapFrom((src, dest, member, ctx) =>
                    ctx.Items[MappingKeys.StudioName] ?? "Unknown"))
            .ForMember(dest => dest.Director,
                opt => opt.MapFrom((src, dest, member, ctx) =>
                    ctx.Items[MappingKeys.DirectorName] ?? "Unknown"));
    }
}
