using Domain.DatabaseModel;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DatabaseContext : DbContext
    {

        public DbSet<Game> Game { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<Developer> Developer { get; set; }
        public DbSet<DealDate> DealDate { get; set; }
        public DbSet<DLC> DLC { get; set; }
        public DbSet<GameCategory> GameCategory { get; set; }
        public DbSet<GameDeal> GameDeal { get; set; }
        public DbSet<GameDeveloper> GameDeveloper { get; set; }
        public DbSet<GameDLC> GameDLC { get; set; }
        public DbSet<GameGenre> GameGenre { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<GamePublisher> GamePublisher { get; set; }
        public DbSet<Platform> Platform { get; set; }
        public DbSet<PriceOverview> PriceOverview { get; set; }
        public DbSet<Publisher> Publisher { get; set; }
        public DbSet<ReleaseDate> ReleaseDate { get; set; }
        public DbSet<Screenshot> Screenshot { get; set; }
        public DbSet<SteamApp> SteamApp { get; set; }
        public DbSet<Store> Store { get; set; }
        public DbSet<SystemRequirement> SystemRequirement { get; set; }
        public DbSet<VideoContent> VideoContent { get; set; }
        public DbSet<Video> Video { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasIndex(c => c.Description).IsUnique(true);
            modelBuilder.Entity<Currency>().HasIndex(c => c.Code).IsUnique(true);
            modelBuilder.Entity<Developer>().HasIndex(d => d.Name).IsUnique(true);
            modelBuilder.Entity<Game>().HasIndex(g => g.SteamAppId).IsUnique(true);
            modelBuilder.Entity<Publisher>().HasIndex(p => p.Name).IsUnique(true);
            modelBuilder.Entity<Platform>().HasIndex(p => p.Name).IsUnique(true);
            modelBuilder.Entity<Genre>().HasIndex(p => p.Description).IsUnique(true);
            modelBuilder.Entity<SteamApp>().HasIndex(s=> s.SteamId).IsUnique(true);
         
            

            //or: modelBuilder.Entity<User>().HasAlternateKey(u => new { u.Passport, u.Name})
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=ABDUL;Initial Catalog=GameCity;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

    }
}