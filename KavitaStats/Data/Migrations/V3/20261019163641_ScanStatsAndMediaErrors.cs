using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KavitaStats.Data.Migrations.V3
{
    /// <inheritdoc />
    public partial class ScanStatsAndMediaErrors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalMediaErrors",
                table: "ServerStat",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FolderWatchMode",
                table: "LibraryStat",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastScanDurationMs",
                table: "LibraryStat",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastScanFolderCount",
                table: "LibraryStat",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastScanListMs",
                table: "LibraryStat",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastScanParseMs",
                table: "LibraryStat",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastScanWalkMs",
                table: "LibraryStat",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MultiFileEpubChapterCount",
                table: "LibraryStat",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MediaErrorStat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Reason = table.Column<int>(type: "INTEGER", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    DismissedCount = table.Column<int>(type: "INTEGER", nullable: false),
                    ServerStatId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaErrorStat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaErrorStat_ServerStat_ServerStatId",
                        column: x => x.ServerStatId,
                        principalTable: "ServerStat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaErrorStat_ServerStatId",
                table: "MediaErrorStat",
                column: "ServerStatId");

            migrationBuilder.Sql("""
                UPDATE ServerStat
                SET UsingRestrictedProfiles = EXISTS (
                    SELECT 1
                    FROM UserStat u
                    JOIN UserAgeRestriction r ON r.UserStatId = u.Id
                    WHERE u.ServerStatId = ServerStat.Id AND r.AgeRating <> -1)
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaErrorStat");

            migrationBuilder.DropColumn(
                name: "TotalMediaErrors",
                table: "ServerStat");

            migrationBuilder.DropColumn(
                name: "FolderWatchMode",
                table: "LibraryStat");

            migrationBuilder.DropColumn(
                name: "LastScanDurationMs",
                table: "LibraryStat");

            migrationBuilder.DropColumn(
                name: "LastScanFolderCount",
                table: "LibraryStat");

            migrationBuilder.DropColumn(
                name: "LastScanListMs",
                table: "LibraryStat");

            migrationBuilder.DropColumn(
                name: "LastScanParseMs",
                table: "LibraryStat");

            migrationBuilder.DropColumn(
                name: "LastScanWalkMs",
                table: "LibraryStat");

            migrationBuilder.DropColumn(
                name: "MultiFileEpubChapterCount",
                table: "LibraryStat");
        }
    }
}
