using System;
using KavitaStats.Entities.Interfaces;
using KavitaStats.Entities.V3;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace KavitaStats.Data;

/// <summary>
/// V3 is very different, we will use a completely different context to store it
/// </summary>
public sealed class DataContextV3 : DbContext
{
    public DbSet<ServerStat> ServerStat { get; set; }
    public DbSet<RelationshipStat> RelationshipStat { get; set; }
    public DbSet<UserStat> UserStat { get; set; }
    public DbSet<LibraryStat> LibraryStat { get; set; }
    public DbSet<LibraryStatFileTypeGroup> LibraryStatFileTypeGroup { get; set; }
    public DbSet<UserStatDevicePlatform> UserStatDevicePlatform { get; set; }
    public DbSet<UserStatRole> UserStatRole { get; set; }
    public DbSet<UserAgeRestriction> UserAgeRestriction { get; set; }
    

        
    public DataContextV3(DbContextOptions options) : base(options)
    {
        ChangeTracker.Tracked += OnEntityTracked;
        ChangeTracker.StateChanged += OnEntityStateChanged;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // ServerInfo configuration
        builder.Entity<ServerStat>()
            .HasKey(s => s.Id); 

        builder.Entity<ServerStat>()
            .HasIndex(s => s.InstallId)
            .IsUnique();

        // LibraryStat configuration
        builder.Entity<LibraryStat>()
            .HasOne(l => l.ServerStat)
            .WithMany(s => s.Libraries)
            .HasForeignKey(l => l.ServerStatId)
            .OnDelete(DeleteBehavior.Cascade);

        // Repeat for other entities
        builder.Entity<RelationshipStat>()
            .HasOne(r => r.ServerStat)
            .WithMany(s => s.Relationships)
            .HasForeignKey(r => r.ServerStatId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserStat>()
            .HasOne(u => u.ServerStat)
            .WithMany(s => s.Users)
            .HasForeignKey(u => u.ServerStatId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void OnEntityTracked(object sender, EntityTrackedEventArgs e)
    {
        if (!e.FromQuery && e.Entry.State == EntityState.Added && e.Entry.Entity is IHasDate entity)
        {
            entity.Created = DateTime.UtcNow;
            entity.LastModified = DateTime.UtcNow;
        }
        if (!e.FromQuery && e.Entry.State == EntityState.Added && e.Entry.Entity is IHasUpdateCounter entity2)
        {
            entity2.UpdateCount = 1;
        }

    }

    private static void OnEntityStateChanged(object sender, EntityStateChangedEventArgs e)
    {
        if (e.NewState == EntityState.Modified && e.Entry.Entity is IHasDate entity)
            entity.LastModified = DateTime.UtcNow;
            
        if (e.NewState == EntityState.Modified && e.Entry.Entity is IHasUpdateCounter entity2)
            entity2.UpdateCount += 1;
    }
}