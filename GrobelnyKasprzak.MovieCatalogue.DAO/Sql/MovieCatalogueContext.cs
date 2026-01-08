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
            string baseDir = AppContext.BaseDirectory;
            DirectoryInfo? dir = new(baseDir);

            while (dir != null && dir.GetFiles("*.sln").Length == 0)
            {
                dir = dir.Parent;
            }

            if (dir == null)
                throw new Exception("Solution directory not found.");

            // Create a solution-level folder for the database
            string dbFolder = Path.Combine(dir.FullName, "Database");
            Directory.CreateDirectory(dbFolder);
            string dbPath = Path.Combine(dbFolder, "movies.db");

            options.UseSqlite($"Data Source={dbPath}");
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
