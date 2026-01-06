using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using GrobelnyKasprzak.MovieCatalogue.MVC.Services;
using GrobelnyKasprzak.MovieCatalogue.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register data access object
        ReflectionLoader.RegisterDao(builder.Services, builder.Configuration);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddSingleton<ILookupService, LookupService>();
        builder.Services.AddScoped<IMovieService, MovieService>();
        builder.Services.AddScoped<IDirectorService, DirectorService>();

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(MovieService).Assembly);
            cfg.AddMaps(typeof(Program).Assembly);
        });


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}