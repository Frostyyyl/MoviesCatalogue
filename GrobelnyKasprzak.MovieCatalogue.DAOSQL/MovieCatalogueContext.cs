using GrobelnyKasprzak.MovieCatalogue.DAOSQL.Models;
using Microsoft.EntityFrameworkCore;

namespace GrobelnyKasprzak.MovieCatalogue.DAOSQL
{
    public class MovieCatalogueContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Studio> Studios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=movies.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Director)
                .WithMany(d => d.Movies);

            modelBuilder.Entity<Studio>()
                .HasMany(s => s.Movies)
                .WithOne(m => m.Studio)
                .HasForeignKey(m => m.StudioId);
        }
    }

}
