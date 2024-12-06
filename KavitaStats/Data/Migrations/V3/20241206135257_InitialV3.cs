using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KavitaStats.Data.Migrations.V3
{
    /// <inheritdoc />
    public partial class InitialV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServerStat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InstallId = table.Column<string>(type: "TEXT", nullable: true),
                    OptedOut = table.Column<bool>(type: "INTEGER", nullable: false),
                    Os = table.Column<string>(type: "TEXT", nullable: true),
                    IsDocker = table.Column<bool>(type: "INTEGER", nullable: false),
                    DotnetVersion = table.Column<string>(type: "TEXT", nullable: true),
                    KavitaVersion = table.Column<string>(type: "TEXT", nullable: true),
                    InitialKavitaVersion = table.Column<string>(type: "TEXT", nullable: true),
                    InitialInstallDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NumOfCores = table.Column<int>(type: "INTEGER", nullable: false),
                    OsLocale = table.Column<string>(type: "TEXT", nullable: true),
                    TimeToOpeCbzMs = table.Column<long>(type: "INTEGER", nullable: false),
                    TimeToOpenCbzPages = table.Column<long>(type: "INTEGER", nullable: false),
                    TimeToPingKavitaStatsApi = table.Column<long>(type: "INTEGER", nullable: false),
                    NumberOfCollections = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfReadingLists = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalFiles = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalGenres = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalSeries = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalLibraries = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalPeople = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxSeriesInALibrary = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxVolumesInASeries = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxChaptersInASeries = table.Column<int>(type: "INTEGER", nullable: false),
                    OpdsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    EncodeMediaAs = table.Column<int>(type: "INTEGER", nullable: false),
                    LastReadTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ActiveKavitaPlusSubscription = table.Column<bool>(type: "INTEGER", nullable: false),
                    UsingRestrictedProfiles = table.Column<bool>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerStat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LibraryStat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IncludeInDashboard = table.Column<bool>(type: "INTEGER", nullable: false),
                    IncludeInSearch = table.Column<bool>(type: "INTEGER", nullable: false),
                    UsingFolderWatching = table.Column<bool>(type: "INTEGER", nullable: false),
                    UsingExcludePatterns = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateCollectionsFromMetadata = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateReadingListsFromMetadata = table.Column<bool>(type: "INTEGER", nullable: false),
                    LibraryType = table.Column<int>(type: "INTEGER", nullable: false),
                    LastScanned = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NumberOfFolders = table.Column<int>(type: "INTEGER", nullable: false),
                    ServerStatId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryStat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryStat_ServerStat_ServerStatId",
                        column: x => x.ServerStatId,
                        principalTable: "ServerStat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RelationshipStat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    Relationship = table.Column<int>(type: "INTEGER", nullable: false),
                    ServerStatId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelationshipStat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelationshipStat_ServerStat_ServerStatId",
                        column: x => x.ServerStatId,
                        principalTable: "ServerStat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LastReadTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsEmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasValidEmail = table.Column<bool>(type: "INTEGER", nullable: false),
                    PercentageOfLibrariesHasAccess = table.Column<float>(type: "REAL", nullable: false),
                    ReadingListsCreatedCount = table.Column<int>(type: "INTEGER", nullable: false),
                    CollectionsCreatedCount = table.Column<int>(type: "INTEGER", nullable: false),
                    WantToReadSeriesCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Locale = table.Column<string>(type: "TEXT", nullable: true),
                    ActiveTheme = table.Column<string>(type: "TEXT", nullable: true),
                    SeriesBookmarksCreatedCount = table.Column<int>(type: "INTEGER", nullable: false),
                    HasAniListToken = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasMALToken = table.Column<bool>(type: "INTEGER", nullable: false),
                    SmartFilterCreatedCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsSharingReviews = table.Column<bool>(type: "INTEGER", nullable: false),
                    ServerStatId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStat_ServerStat_ServerStatId",
                        column: x => x.ServerStatId,
                        principalTable: "ServerStat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LibraryStatFileTypeGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LibraryStatId = table.Column<int>(type: "INTEGER", nullable: false),
                    FileType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryStatFileTypeGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryStatFileTypeGroup_LibraryStat_LibraryStatId",
                        column: x => x.LibraryStatId,
                        principalTable: "LibraryStat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAgeRestriction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AgeRating = table.Column<int>(type: "INTEGER", nullable: false),
                    IncludeUnknowns = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserStatId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAgeRestriction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAgeRestriction_UserStat_UserStatId",
                        column: x => x.UserStatId,
                        principalTable: "UserStat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStatDevicePlatform",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DevicePlatform = table.Column<int>(type: "INTEGER", nullable: false),
                    UserStatId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStatDevicePlatform", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStatDevicePlatform_UserStat_UserStatId",
                        column: x => x.UserStatId,
                        principalTable: "UserStat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStatRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Role = table.Column<string>(type: "TEXT", nullable: true),
                    UserStatId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStatRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStatRole_UserStat_UserStatId",
                        column: x => x.UserStatId,
                        principalTable: "UserStat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LibraryStat_ServerStatId",
                table: "LibraryStat",
                column: "ServerStatId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryStatFileTypeGroup_LibraryStatId",
                table: "LibraryStatFileTypeGroup",
                column: "LibraryStatId");

            migrationBuilder.CreateIndex(
                name: "IX_RelationshipStat_ServerStatId",
                table: "RelationshipStat",
                column: "ServerStatId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerStat_InstallId",
                table: "ServerStat",
                column: "InstallId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAgeRestriction_UserStatId",
                table: "UserAgeRestriction",
                column: "UserStatId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserStat_ServerStatId",
                table: "UserStat",
                column: "ServerStatId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStatDevicePlatform_UserStatId",
                table: "UserStatDevicePlatform",
                column: "UserStatId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStatRole_UserStatId",
                table: "UserStatRole",
                column: "UserStatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LibraryStatFileTypeGroup");

            migrationBuilder.DropTable(
                name: "RelationshipStat");

            migrationBuilder.DropTable(
                name: "UserAgeRestriction");

            migrationBuilder.DropTable(
                name: "UserStatDevicePlatform");

            migrationBuilder.DropTable(
                name: "UserStatRole");

            migrationBuilder.DropTable(
                name: "LibraryStat");

            migrationBuilder.DropTable(
                name: "UserStat");

            migrationBuilder.DropTable(
                name: "ServerStat");
        }
    }
}
