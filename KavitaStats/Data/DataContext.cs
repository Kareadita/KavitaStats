using System;
using KavitaStats.Entities;
using KavitaStats.Entities.Interfaces;
using KavitaStats.Entities.V2;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace KavitaStats.Data;

public sealed class DataContext : IdentityDbContext<AppUser, AppRole, int,
    IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
    IdentityRoleClaim<int>, IdentityUserToken<int>>
{
        
    public DbSet<AppUser> AppUser { get; set; }
    public DbSet<StatRecord> StatRecord { get; set; }
    public DbSet<Color> Color { get; set; }
    public DbSet<MangaReaderLayoutMode> MangaReaderLayoutMode { get; set; }
    public DbSet<PageSplit> PageSplit { get; set; }
    public DbSet<FileFormat> FileFormat { get; set; }
        
    public DataContext(DbContextOptions options) : base(options)
    {
        ChangeTracker.Tracked += OnEntityTracked;
        ChangeTracker.StateChanged += OnEntityStateChanged;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


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
            entity.Created = DateTime.Now;
            entity.LastModified = DateTime.Now;
        }
        if (!e.FromQuery && e.Entry.State == EntityState.Added && e.Entry.Entity is IHasUpdateCounter entity2)
        {
            entity2.UpdateCount = 1;
        }

    }

    void OnEntityStateChanged(object sender, EntityStateChangedEventArgs e)
    {
        if (e.NewState == EntityState.Modified && e.Entry.Entity is IHasDate entity)
            entity.LastModified = DateTime.Now;
            
        if (e.NewState == EntityState.Modified && e.Entry.Entity is IHasUpdateCounter entity2)
            entity2.UpdateCount += 1;
    }
        
}