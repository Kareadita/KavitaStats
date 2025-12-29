using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KavitaStats.Data.Migrations.V3
{
    /// <inheritdoc />
    public partial class SocialProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TotalPagesRead",
                table: "UserStat",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TotalSecondsRead",
                table: "UserStat",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TotalWordsRead",
                table: "UserStat",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPagesRead",
                table: "UserStat");

            migrationBuilder.DropColumn(
                name: "TotalSecondsRead",
                table: "UserStat");

            migrationBuilder.DropColumn(
                name: "TotalWordsRead",
                table: "UserStat");
        }
    }
}
