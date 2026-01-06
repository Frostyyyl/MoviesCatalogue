using GrobelnyKasprzak.MovieCatalogue.DAO.Sql.Models;
using Microsoft.EntityFrameworkCore;

namespace GrobelnyKasprzak.MovieCatalogue.DAO.Sql
{
    public class MovieCatalogueContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Director> Directors { get; set; }

        public MovieCatalogueContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "MovieCatalogueSqlDatabase"
            );
            Directory.CreateDirectory(folder);

            string path = Path.Combine(folder, "movies.db");

            options.UseSqlite($"Data Source={path}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Director)
                .WithMany(d => d.Movies)
                .HasForeignKey(m => m.DirectorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
