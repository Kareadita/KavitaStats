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
            migrationBuilder.AddColumn<bool>(
                name: "IsSharingAnnotations",
                table: "UserStat",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSharingProfile",
                table: "UserStat",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserStat",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSharingAnnotations",
                table: "UserStat");

            migrationBuilder.DropColumn(
                name: "IsSharingProfile",
                table: "UserStat");

            migrationBuilder.DropColumn(
                name: "TotalPagesRead",
                table: "UserStat");

            migrationBuilder.DropColumn(
                name: "TotalSecondsRead",
                table: "UserStat");

            migrationBuilder.DropColumn(
                name: "TotalWordsRead",
                table: "UserStat");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserStat");
        }
    }
}
