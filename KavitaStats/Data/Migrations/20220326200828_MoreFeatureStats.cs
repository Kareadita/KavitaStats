using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KavitaStats.Data.Migrations
{
    public partial class MoreFeatureStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActiveSiteTheme",
                table: "StatRecord",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MangaReaderMode",
                table: "StatRecord",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfCollections",
                table: "StatRecord",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfReadingLists",
                table: "StatRecord",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfUsers",
                table: "StatRecord",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "OPDSEnabled",
                table: "StatRecord",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TotalFiles",
                table: "StatRecord",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "UpdateCount",
                table: "StatRecord",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveSiteTheme",
                table: "StatRecord");

            migrationBuilder.DropColumn(
                name: "MangaReaderMode",
                table: "StatRecord");

            migrationBuilder.DropColumn(
                name: "NumberOfCollections",
                table: "StatRecord");

            migrationBuilder.DropColumn(
                name: "NumberOfReadingLists",
                table: "StatRecord");

            migrationBuilder.DropColumn(
                name: "NumberOfUsers",
                table: "StatRecord");

            migrationBuilder.DropColumn(
                name: "OPDSEnabled",
                table: "StatRecord");

            migrationBuilder.DropColumn(
                name: "TotalFiles",
                table: "StatRecord");

            migrationBuilder.DropColumn(
                name: "UpdateCount",
                table: "StatRecord");
        }
    }
}
