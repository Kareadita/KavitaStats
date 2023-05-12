using KavitaStats.Entities.Enum;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KavitaStats.Data.Migrations
{
    /// <inheritdoc />
    public partial class EncodeMediaAs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EncodeMediaAs",
                table: "StatRecord",
                type: "INTEGER",
                nullable: false,
                defaultValue: EncodeFormat.PNG);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EncodeMediaAs",
                table: "StatRecord");
        }
    }
}
