using Microsoft.AspNetCore.Mvc.Rendering;

namespace GrobelnyKasprzak.MovieCatalogue.MVC.Services;

public interface ILookupService
{
    IEnumerable<SelectListItem> GetGenreSelectList();
}
