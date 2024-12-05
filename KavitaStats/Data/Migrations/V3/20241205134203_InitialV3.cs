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
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastActive = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServerInfoV3",
                columns: table => new
                {
                    InstallId = table.Column<string>(type: "TEXT", nullable: false),
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
                    UsingRestrictedProfiles = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerInfoV3", x => x.InstallId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    InstallId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryStat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryStat_ServerInfoV3_InstallId",
                        column: x => x.InstallId,
                        principalTable: "ServerInfoV3",
                        principalColumn: "InstallId");
                });

            migrationBuilder.CreateTable(
                name: "RelationshipStat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    Relationship = table.Column<int>(type: "INTEGER", nullable: false),
                    InstallId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelationshipStat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelationshipStat_ServerInfoV3_InstallId",
                        column: x => x.InstallId,
                        principalTable: "ServerInfoV3",
                        principalColumn: "InstallId");
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
                    DevicePlatforms = table.Column<string>(type: "TEXT", nullable: true),
                    Roles = table.Column<string>(type: "TEXT", nullable: true),
                    InstallId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStat_ServerInfoV3_InstallId",
                        column: x => x.InstallId,
                        principalTable: "ServerInfoV3",
                        principalColumn: "InstallId");
                });

            migrationBuilder.CreateTable(
                name: "FileFormat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Extension = table.Column<string>(type: "TEXT", nullable: true),
                    Format = table.Column<int>(type: "INTEGER", nullable: false),
                    LibraryStatId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileFormat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileFormat_LibraryStat_LibraryStatId",
                        column: x => x.LibraryStatId,
                        principalTable: "LibraryStat",
                        principalColumn: "Id");
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

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileFormat_LibraryStatId",
                table: "FileFormat",
                column: "LibraryStatId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryStat_InstallId",
                table: "LibraryStat",
                column: "InstallId");

            migrationBuilder.CreateIndex(
                name: "IX_RelationshipStat_InstallId",
                table: "RelationshipStat",
                column: "InstallId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAgeRestriction_UserStatId",
                table: "UserAgeRestriction",
                column: "UserStatId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserStat_InstallId",
                table: "UserStat",
                column: "InstallId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "FileFormat");

            migrationBuilder.DropTable(
                name: "RelationshipStat");

            migrationBuilder.DropTable(
                name: "UserAgeRestriction");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "LibraryStat");

            migrationBuilder.DropTable(
                name: "UserStat");

            migrationBuilder.DropTable(
                name: "ServerInfoV3");
        }
    }
}
