using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KavitaStats.Data.Migrations.V3
{
    /// <inheritdoc />
    public partial class NullPreReleaseInstallDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Not in the previous migration: SQLite table rebuilds run after its Sql() calls, while the column is still NOT NULL
            migrationBuilder.Sql("UPDATE ServerStat SET InitialInstallDate = NULL WHERE InitialInstallDate < '2021-01-20'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
