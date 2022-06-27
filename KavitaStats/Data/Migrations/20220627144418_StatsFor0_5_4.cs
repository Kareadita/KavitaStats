using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KavitaStats.Data.Migrations
{
    public partial class StatsFor0_5_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxChaptersInASeries",
                table: "StatRecord",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxSeriesInALibrary",
                table: "StatRecord",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxVolumesInASeries",
                table: "StatRecord",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "StoreBookmarksAsWebP",
                table: "StatRecord",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TotalGenres",
                table: "StatRecord",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalPeople",
                table: "StatRecord",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsersOnCardLayout",
                table: "StatRecord",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsersOnListLayout",
                table: "StatRecord",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "UsingSeriesRelationships",
                table: "StatRecord",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxChaptersInASeries",
                table: "StatRecord");

            migrationBuilder.DropColumn(
                name: "MaxSeriesInALibrary",
                table: "StatRecord");

            migrationBuilder.DropColumn(
                name: "MaxVolumesInASeries",
                table: "StatRecord");

            migrationBuilder.DropColumn(
                name: "StoreBookmarksAsWebP",
                table: "StatRecord");

            migrationBuilder.DropColumn(
                name: "TotalGenres",
                table: "StatRecord");

            migrationBuilder.DropColumn(
                name: "TotalPeople",
                table: "StatRecord");

            migrationBuilder.DropColumn(
                name: "UsersOnCardLayout",
                table: "StatRecord");

            migrationBuilder.DropColumn(
                name: "UsersOnListLayout",
                table: "StatRecord");

            migrationBuilder.DropColumn(
                name: "UsingSeriesRelationships",
                table: "StatRecord");
        }
    }
}
