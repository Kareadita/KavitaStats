using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KavitaStats.Data.Migrations.V3
{
    /// <inheritdoc />
    public partial class SeedInstallLifecycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                INSERT INTO InstallLifecycle (InstallId, FirstSeen, LastSeen, InitialVersion, LastVersion, InitialInstallDate, EverHadSeries, Seeded)
                SELECT InstallId, Created, LastModified, InitialKavitaVersion, KavitaVersion, InitialInstallDate, TotalSeries > 0, 1
                FROM ServerStat
                """);

            migrationBuilder.Sql("""
                INSERT INTO KavitaPlusPeriod (InstallLifecycleId, StartedOn, Seeded)
                SELECT l.Id, s.Created, 1
                FROM ServerStat s
                JOIN InstallLifecycle l ON l.InstallId = s.InstallId
                WHERE s.ActiveKavitaPlusSubscription = 1
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM KavitaPlusPeriod WHERE Seeded = 1");
            migrationBuilder.Sql("DELETE FROM InstallLifecycle WHERE Seeded = 1");
        }
    }
}
