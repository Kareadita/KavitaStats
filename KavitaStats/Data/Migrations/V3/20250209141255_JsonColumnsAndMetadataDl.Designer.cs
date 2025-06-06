﻿// <auto-generated />
using System;
using KavitaStats.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KavitaStats.Data.Migrations.V3
{
    [DbContext(typeof(DataContextV3))]
    [Migration("20250209141255_JsonColumnsAndMetadataDl")]
    partial class JsonColumnsAndMetadataDl
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("KavitaStats.Entities.V3.LibraryStat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CreateCollectionsFromMetadata")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CreateReadingListsFromMetadata")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FileTypes")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IncludeInDashboard")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IncludeInSearch")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastScanned")
                        .HasColumnType("TEXT");

                    b.Property<int>("LibraryType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberOfFolders")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ServerStatId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("UsingExcludePatterns")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("UsingFolderWatching")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ServerStatId");

                    b.ToTable("LibraryStat");
                });

            modelBuilder.Entity("KavitaStats.Entities.V3.RelationshipStat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Count")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Relationship")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ServerStatId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ServerStatId");

                    b.ToTable("RelationshipStat");
                });

            modelBuilder.Entity("KavitaStats.Entities.V3.ServerStat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ActiveKavitaPlusSubscription")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("DotnetVersion")
                        .HasColumnType("TEXT");

                    b.Property<int>("EncodeMediaAs")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("InitialInstallDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("InitialKavitaVersion")
                        .HasColumnType("TEXT");

                    b.Property<string>("InstallId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDocker")
                        .HasColumnType("INTEGER");

                    b.Property<string>("KavitaVersion")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastReadTime")
                        .HasColumnType("TEXT");

                    b.Property<bool>("MatchedMetadataEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxChaptersInASeries")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxSeriesInALibrary")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxVolumesInASeries")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumOfCores")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberOfCollections")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberOfReadingLists")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("OpdsEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("OptedOut")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Os")
                        .HasColumnType("TEXT");

                    b.Property<string>("OsLocale")
                        .HasColumnType("TEXT");

                    b.Property<long>("TimeToOpeCbzMs")
                        .HasColumnType("INTEGER");

                    b.Property<long>("TimeToOpenCbzPages")
                        .HasColumnType("INTEGER");

                    b.Property<long>("TimeToPingKavitaStatsApi")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TotalFiles")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TotalGenres")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TotalLibraries")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TotalPeople")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TotalSeries")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("UsingRestrictedProfiles")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("InstallId")
                        .IsUnique();

                    b.ToTable("ServerStat");
                });

            modelBuilder.Entity("KavitaStats.Entities.V3.UserAgeRestriction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AgeRating")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IncludeUnknowns")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserStatId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserStatId")
                        .IsUnique();

                    b.ToTable("UserAgeRestriction");
                });

            modelBuilder.Entity("KavitaStats.Entities.V3.UserStat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ActiveTheme")
                        .HasColumnType("TEXT");

                    b.Property<int>("CollectionsCreatedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DevicePlatforms")
                        .HasColumnType("TEXT");

                    b.Property<bool>("HasAniListToken")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasMALToken")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasValidEmail")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsEmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsSharingReviews")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastReadTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Locale")
                        .HasColumnType("TEXT");

                    b.Property<float>("PercentageOfLibrariesHasAccess")
                        .HasColumnType("REAL");

                    b.Property<int>("ReadingListsCreatedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Roles")
                        .HasColumnType("TEXT");

                    b.Property<int>("SeriesBookmarksCreatedCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ServerStatId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SmartFilterCreatedCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WantToReadSeriesCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ServerStatId");

                    b.ToTable("UserStat");
                });

            modelBuilder.Entity("KavitaStats.Entities.V3.LibraryStat", b =>
                {
                    b.HasOne("KavitaStats.Entities.V3.ServerStat", "ServerStat")
                        .WithMany("Libraries")
                        .HasForeignKey("ServerStatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServerStat");
                });

            modelBuilder.Entity("KavitaStats.Entities.V3.RelationshipStat", b =>
                {
                    b.HasOne("KavitaStats.Entities.V3.ServerStat", "ServerStat")
                        .WithMany("Relationships")
                        .HasForeignKey("ServerStatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServerStat");
                });

            modelBuilder.Entity("KavitaStats.Entities.V3.UserAgeRestriction", b =>
                {
                    b.HasOne("KavitaStats.Entities.V3.UserStat", "UserStat")
                        .WithOne("AgeRestriction")
                        .HasForeignKey("KavitaStats.Entities.V3.UserAgeRestriction", "UserStatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserStat");
                });

            modelBuilder.Entity("KavitaStats.Entities.V3.UserStat", b =>
                {
                    b.HasOne("KavitaStats.Entities.V3.ServerStat", "ServerStat")
                        .WithMany("Users")
                        .HasForeignKey("ServerStatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServerStat");
                });

            modelBuilder.Entity("KavitaStats.Entities.V3.ServerStat", b =>
                {
                    b.Navigation("Libraries");

                    b.Navigation("Relationships");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("KavitaStats.Entities.V3.UserStat", b =>
                {
                    b.Navigation("AgeRestriction");
                });
#pragma warning restore 612, 618
        }
    }
}
