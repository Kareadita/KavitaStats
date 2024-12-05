using System;
using KavitaStats.Entities;
using KavitaStats.Entities.Interfaces;
using KavitaStats.Entities.V3;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace KavitaStats.Data;

/// <summary>
/// V3 is very different, we will use a completely different context to store it
/// </summary>
public sealed class DataContextV3 : IdentityDbContext<AppUser, AppRole, int,
    IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
    IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public DbSet<AppUser> AppUser { get; set; }

        
    public DataContextV3(DbContextOptions options) : base(options)
    {
        ChangeTracker.Tracked += OnEntityTracked;
        ChangeTracker.StateChanged += OnEntityStateChanged;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<ServerInfoV3>()
            .HasKey(s => s.InstallId);

        builder.Entity<RelationshipStat>()
            .HasOne(r => r.Server)
            .WithMany(s => s.Relationships)
            .HasForeignKey(r => r.InstallId);

        builder.Entity<LibraryStat>()
            .HasOne(l => l.Server)
            .WithMany(s => s.Libraries)
            .HasForeignKey(l => l.InstallId);

        builder.Entity<UserStat>()
            .HasOne(u => u.Server)
            .WithMany(s => s.Users)
            .HasForeignKey(u => u.InstallId);
        


        builder.Entity<AppUser>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

        builder.Entity<AppRole>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();
    }
        
    void OnEntityTracked(object sender, EntityTrackedEventArgs e)
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

    void OnEntityStateChanged(object sender, EntityStateChangedEventArgs e)
    {
        if (e.NewState == EntityState.Modified && e.Entry.Entity is IHasDate entity)
            entity.LastModified = DateTime.UtcNow;
            
        if (e.NewState == EntityState.Modified && e.Entry.Entity is IHasUpdateCounter entity2)
            entity2.UpdateCount += 1;
    }
}