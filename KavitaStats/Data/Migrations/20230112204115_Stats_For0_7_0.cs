using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KavitaStats.Data.Migrations
{
    public partial class Stats_For0_7_0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "PercentOfLibrariesIncludedInDashboard",
                table: "StatRecord",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PercentOfLibrariesIncludedInRecommended",
                table: "StatRecord",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PercentOfLibrariesIncludedInSearch",
                table: "StatRecord",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PercentOfLibrariesWithFolderWatchingEnabled",
                table: "StatRecord",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<bool>(
                name: "StoreCoversAsWebP",
                table: "StatRecord",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "TotalReadingHours",
                table: "StatRecord",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "UsersWithEmulateComicBook",
                table: "StatRecord",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentOfLibrariesIncludedInDashboard",
                table: "StatRecord");

            migrationBuilder.DropColumn(
                name: "PercentOfLibrariesIncludedInRecommended",
                table: "StatRecord");

            migrationBuilder.DropColumn(
                name: "PercentOfLibrariesIncludedInSearch",
                table: "StatRecord");

            migrationBuilder.DropColumn(
                name: "PercentOfLibrariesWithFolderWatchingEnabled",
                table: "StatRecord");

            migrationBuilder.DropColumn(
                name: "StoreCoversAsWebP",
                table: "StatRecord");

            migrationBuilder.DropColumn(
                name: "TotalReadingHours",
                table: "StatRecord");

            migrationBuilder.DropColumn(
                name: "UsersWithEmulateComicBook",
                table: "StatRecord");
        }
    }
}
