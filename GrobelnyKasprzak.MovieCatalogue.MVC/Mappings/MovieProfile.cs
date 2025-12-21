using AutoMapper;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using GrobelnyKasprzak.MovieCatalogue.MVC.Models.Dto;
using GrobelnyKasprzak.MovieCatalogue.MVC.ViewModels;

namespace GrobelnyKasprzak.MovieCatalogue.MVC.Mappings;

public class MovieProfile : Profile
{
    public MovieProfile()
    {
        CreateMap<MovieViewModel, MovieDto>();

        CreateMap<IMovie, MovieListItemViewModel>();

        CreateMap<IMovie, MovieViewModel>()
            .ForMember(dest => dest.Director, opt => opt.Ignore())
            .ForMember(dest => dest.AvailableDirectors, opt => opt.Ignore())
            .ForMember(dest => dest.AvailableGenres, opt => opt.Ignore());
    }
}
