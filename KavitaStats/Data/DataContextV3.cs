using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using KavitaStats.Entities.Enum;
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
        
        var stringListComparer = new ValueComparer<List<string>>(
            (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList());

        var devicePlatformListComparer = new ValueComparer<List<DevicePlatform>>(
            (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList());
        
        var fileTypeGroupListComparer = new ValueComparer<List<FileTypeGroup>>(
            (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList());

        
        builder.Entity<UserStat>()
            .Property(s => s.Roles)
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                v => JsonSerializer.Deserialize<List<string>>(v, JsonSerializerOptions.Default) ?? new List<string>())
            .Metadata.SetValueComparer(stringListComparer);        
        builder.Entity<UserStat>()
            .Property(s => s.DevicePlatforms)
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                v => JsonSerializer.Deserialize<List<DevicePlatform>>(v, JsonSerializerOptions.Default) ?? new List<DevicePlatform>())
            .Metadata.SetValueComparer(devicePlatformListComparer);        
        builder.Entity<LibraryStat>()
            .Property(s => s.FileTypes)
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                v => JsonSerializer.Deserialize<List<FileTypeGroup>>(v, JsonSerializerOptions.Default) ?? new List<FileTypeGroup>())
            .Metadata.SetValueComparer(fileTypeGroupListComparer);
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