using Microsoft.EntityFrameworkCore;
using SuzumesDeepDungeon.Models;

namespace SuzumesDeepDungeon.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public DbSet<GameRank> GameRanks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Screenshot> Screenshots { get; set; }
    public DbSet<Trailer> Trailers { get; set; }
    public DbSet<GameTag> Tag { get; set; }
    public DbSet<GameAchievement> Achievements { get; set; }
    public DbSet<Stores> Stores { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Конфигурация модели
        modelBuilder.Entity<GameRank>(entity =>
        {

            entity.HasKey(e => e.Id);
            entity
        .Navigation(s => s.User)
        .AutoInclude(false);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Rate).HasDefaultValue(0);
            entity.Property(e => e.Created).HasDefaultValueSql("datetime('now')");
            entity.Property(s => s.Updated).HasDefaultValueSql("datetime('now')")
                  .ValueGeneratedOnAddOrUpdate();


            entity.HasOne(g => g.User)
        .WithMany(u => u.GameRanks)
        .HasForeignKey(g => g.UserId)
        .IsRequired(false)  // Разрешить NULL
        .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(g => g.Stores).WithOne(s => s.GameRank)
        .HasForeignKey(s => s.GameId)
        .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(g => g.Tags).WithOne(s => s.GameRank)
       .HasForeignKey(s => s.GameId)
       .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(g => g.Screenshots).WithOne(s => s.GameRank)
       .HasForeignKey<Screenshot>(s => s.GameId)
       .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(g => g.Achievements).WithOne(s => s.GameRank)
       .HasForeignKey(s => s.GameId)
       .OnDelete(DeleteBehavior.Cascade).IsRequired(false);

        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired();
            entity.Property(e => e.Password).IsRequired();
            entity.Property(e => e.Role).IsRequired();
            entity.Property(e => e.IsAdmin).IsRequired();
            entity.Property(e => e.Email);
            entity.Property(e => e.Created).HasDefaultValueSql("datetime('now')");
            entity.Property(s => s.Updated).HasDefaultValueSql("datetime('now')")
                  .ValueGeneratedOnAddOrUpdate();
            entity.HasMany(u => u.GameRanks)
        .WithOne(g => g.User)
        .HasForeignKey(g => g.UserId);
            entity
        .Navigation(s => s.GameRanks)
        .AutoInclude(false);
        });


        modelBuilder.Entity<Screenshot>(entity =>
        {

            entity.Property(s => s.SteamHeaderUrl).IsRequired();
            entity.Property(s => s.RawgBackgroundUrl).IsRequired();

            entity.Property(s => s.Created)
                  .HasDefaultValueSql("datetime('now')");

            entity.Property(s => s.Updated).HasDefaultValueSql("datetime('now')")
                  .ValueGeneratedOnAddOrUpdate();
        });

        modelBuilder.Entity<Screenshot>()
        .HasIndex(s => s.GameId)
        .IsUnique();


        modelBuilder.Entity<Trailer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Created).HasDefaultValueSql("datetime('now')");
            entity.Property(s => s.Updated).HasDefaultValueSql("datetime('now')")
                  .ValueGeneratedOnAddOrUpdate();
            entity.HasOne(t => t.GameRank)
                .WithMany(g => g.Trailers)
                .HasForeignKey(t => t.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<GameAchievement>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Created).HasDefaultValueSql("datetime('now')");
            entity.Property(s => s.Updated).HasDefaultValueSql("datetime('now')")
                  .ValueGeneratedOnAddOrUpdate();
            entity.HasOne(a => a.GameRank)
                .WithMany(g => g.Achievements)
                .HasForeignKey(a => a.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<GameTag>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Created).HasDefaultValueSql("datetime('now')");
            entity.Property(s => s.Updated).HasDefaultValueSql("datetime('now')")
                  .ValueGeneratedOnAddOrUpdate();
            entity.HasOne(t => t.GameRank)
                .WithMany(g => g.Tags)
                .HasForeignKey(t => t.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Stores>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Created).HasDefaultValueSql("datetime('now')");
            entity.Property(s => s.Updated).HasDefaultValueSql("datetime('now')")
                  .ValueGeneratedOnAddOrUpdate();
            entity.HasOne(t => t.GameRank)
                .WithMany(g => g.Stores)
                .HasForeignKey(t => t.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}