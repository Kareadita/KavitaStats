using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KavitaStats.Data.Migrations
{
    /// <inheritdoc />
    public partial class LastReadTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastReadTime",
                table: "StatRecord",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastReadTime",
                table: "StatRecord");
        }
    }
}
