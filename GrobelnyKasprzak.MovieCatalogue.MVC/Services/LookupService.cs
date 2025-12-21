using GrobelnyKasprzak.MovieCatalogue.Core;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GrobelnyKasprzak.MovieCatalogue.MVC.Services;

public class LookupService : ILookupService
{
    private readonly IEnumerable<SelectListItem> _genres;

    public LookupService()
    {
        _genres = Enum.GetValues<MovieGenre>()
            .Select(genre => new SelectListItem
            {
                Value = genre.ToString(),
                Text = genre.ToString(),
            })
            .ToList()
            .AsReadOnly();
    }

    public IEnumerable<SelectListItem> GetGenreSelectList() => _genres;
}