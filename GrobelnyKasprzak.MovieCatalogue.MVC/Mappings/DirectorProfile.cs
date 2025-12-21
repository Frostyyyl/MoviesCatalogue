using AutoMapper;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using GrobelnyKasprzak.MovieCatalogue.MVC.Models.Dto;
using GrobelnyKasprzak.MovieCatalogue.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GrobelnyKasprzak.MovieCatalogue.MVC.Mappings;

public class DirectorProfile : Profile
{
    public DirectorProfile()
    {
        CreateMap<DirectorViewModel, DirectorDto>();

        CreateMap<IDirector, DirectorViewModel>();

        CreateMap<IDirector, SelectListItem>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));
    }
}
