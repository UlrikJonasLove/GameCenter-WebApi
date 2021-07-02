using System.Diagnostics.CodeAnalysis;
using GameCenter.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameCenter.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GamesGenres>().HasKey(x => new {x.GenreId, x.GameId});
            modelBuilder.Entity<GamesActors>().HasKey(x => new {x.PersonId, x.GameId});
            
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Game> Game { get; set; }
        public DbSet<GamesGenres> GamesGenres { get; set; }
        public DbSet<GamesActors> GamesActors { get; set; }
        public DbSet<GameCenters> GameCenters { get; set; }
    }
}