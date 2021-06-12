using System.Diagnostics.CodeAnalysis;
using GameCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace GameCenter.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Person> People { get; set; }
    }
}