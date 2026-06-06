namespace FamilyGames.Infrastructure.Data;

using FamilyGames.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    //  Skapar tabellerna Players och Matches i databasen
    public DbSet<Player> Players => Set<Player>();
    public DbSet<Match> Matches => Set<Match>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Konfigurera Player-tabellen
        modelBuilder.Entity<Player>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Name).IsRequired().HasMaxLength(100);
            e.Property(p => p.AvatarEmoji).HasMaxLength(10);
        });

        // Konfigurera Match-tabellen + 1-många relation
        modelBuilder.Entity<Match>(e =>
        {
            e.HasKey(m => m.Id);
            e.Property(m => m.GameType).IsRequired().HasMaxLength(100);

            e.HasOne(m => m.Player)         // Match har en Player
             .WithMany(p => p.Matches)       // Player har många Matches
             .HasForeignKey(m => m.PlayerId)
             .OnDelete(DeleteBehavior.Cascade); // Radera player → raderar dess matcher
        });

        // läggs in vid migration
        modelBuilder.Entity<Player>().HasData(
            new Player { Id = 1, Name = "Alice", Age = 12, AvatarEmoji = "🧒" },
            new Player { Id = 2, Name = "Bob",   Age = 45, AvatarEmoji = "👨" }
        );
    }
}