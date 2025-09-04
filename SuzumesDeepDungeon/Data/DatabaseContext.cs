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
        // Указываем схему по умолчанию (опционально)
        modelBuilder.HasDefaultSchema("public");

        modelBuilder.Entity<GameRank>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Navigation(s => s.User).AutoInclude(false);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Rate).HasDefaultValue(0);

            // Для PostgreSQL используем CURRENT_TIMESTAMP вместо datetime('now')
            entity.Property(e => e.Created).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(s => s.Updated)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP")
                  .ValueGeneratedOnAddOrUpdate()
                  .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Save);

            entity.HasOne(g => g.User)
                .WithMany(u => u.GameRanks)
                .HasForeignKey(g => g.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(g => g.Stores)
                .WithOne(s => s.GameRank)
                .HasForeignKey(s => s.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(g => g.Tags)
                .WithOne(s => s.GameRank)
                .HasForeignKey(s => s.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(g => g.Screenshots)
                .WithOne(s => s.GameRank)
                .HasForeignKey<Screenshot>(s => s.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(g => g.Achievements)
                .WithOne(s => s.GameRank)
                .HasForeignKey(s => s.GameId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
            
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Rate);
            entity.HasIndex(e => e.Created);
            entity.HasIndex(e => e.Updated);
            entity.HasIndex(e => e.Released);
            
            
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Username).IsRequired();
            entity.Property(e => e.Password).IsRequired();
            entity.Property(e => e.Role).IsRequired();
            entity.Property(e => e.IsAdmin).IsRequired();
            entity.Property(e => e.Email);

            // Для PostgreSQL используем CURRENT_TIMESTAMP
            entity.Property(e => e.Created).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(s => s.Updated)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP")
                  .ValueGeneratedOnAddOrUpdate()
                  .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Save);

            entity.HasMany(u => u.GameRanks)
                .WithOne(g => g.User)
                .HasForeignKey(g => g.UserId);

            entity.Navigation(s => s.GameRanks).AutoInclude(false);
            
            entity.HasIndex(e => e.Username);
            entity.HasIndex(e => e.Email);
        });

        modelBuilder.Entity<Screenshot>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(s => s.SteamHeaderUrl).IsRequired();
            entity.Property(s => s.RawgBackgroundUrl).IsRequired();

            // Для PostgreSQL используем CURRENT_TIMESTAMP
            entity.Property(s => s.Created).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(s => s.Updated)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP")
                  .ValueGeneratedOnAddOrUpdate()
                  .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Save);
            
            
        });

        modelBuilder.Entity<Screenshot>()
            .HasIndex(s => s.GameId)
            .IsUnique();

        modelBuilder.Entity<Trailer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Name).IsRequired();

            // Для PostgreSQL используем CURRENT_TIMESTAMP
            entity.Property(e => e.Created).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(s => s.Updated)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP")
                  .ValueGeneratedOnAddOrUpdate()
                  .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Save);

            entity.HasOne(t => t.GameRank)
                .WithMany(g => g.Trailers)
                .HasForeignKey(t => t.GameId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(e => e.GameId);
        });

        modelBuilder.Entity<GameAchievement>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            // Для PostgreSQL используем CURRENT_TIMESTAMP
            entity.Property(e => e.Created).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(s => s.Updated)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP")
                  .ValueGeneratedOnAddOrUpdate()
                  .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Save);

            entity.HasOne(a => a.GameRank)
                .WithMany(g => g.Achievements)
                .HasForeignKey(a => a.GameId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(e => e.GameId);
        });

        modelBuilder.Entity<GameTag>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            // Для PostgreSQL используем CURRENT_TIMESTAMP
            entity.Property(e => e.Created).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(s => s.Updated)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP")
                  .ValueGeneratedOnAddOrUpdate()
                  .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Save);

            entity.HasOne(t => t.GameRank)
                .WithMany(g => g.Tags)
                .HasForeignKey(t => t.GameId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(e => e.GameId);
        });

        modelBuilder.Entity<Stores>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            // Для PostgreSQL используем CURRENT_TIMESTAMP
            entity.Property(e => e.Created).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(s => s.Updated)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP")
                  .ValueGeneratedOnAddOrUpdate()
                  .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Save);

            entity.HasOne(t => t.GameRank)
                .WithMany(g => g.Stores)
                .HasForeignKey(t => t.GameId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(e => e.GameId);
        });

        // Для PostgreSQL рекомендуется явно указать имена таблиц и столбцов в нижнем регистре
        // чтобы избежать проблем с чувствительностью к регистру
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            entityType.SetTableName(entityType.GetTableName().ToLower());

            foreach (var property in entityType.GetProperties())
            {
                property.SetColumnName(property.Name.ToLower());
            }
        }
    }
}