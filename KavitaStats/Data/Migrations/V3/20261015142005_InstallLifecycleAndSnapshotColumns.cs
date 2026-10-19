using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KavitaStats.Data.Migrations.V3
{
    /// <inheritdoc />
    public partial class InstallLifecycleAndSnapshotColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActiveInstalls",
                table: "HistoricalSnapshot",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveUsers",
                table: "HistoricalSnapshot",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChurnedInstalls",
                table: "HistoricalSnapshot",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InstallsWithContent",
                table: "HistoricalSnapshot",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewInstalls",
                table: "HistoricalSnapshot",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OptedOutInstalls",
                table: "HistoricalSnapshot",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InstallLifecycle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InstallId = table.Column<string>(type: "TEXT", nullable: true),
                    FirstSeen = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastSeen = table.Column<DateTime>(type: "TEXT", nullable: false),
                    InitialVersion = table.Column<string>(type: "TEXT", nullable: true),
                    LastVersion = table.Column<string>(type: "TEXT", nullable: true),
                    InitialInstallDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EverHadSeries = table.Column<bool>(type: "INTEGER", nullable: false),
                    Seeded = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstallLifecycle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SnapshotVersionCount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HistoricalSnapshotId = table.Column<int>(type: "INTEGER", nullable: false),
                    KavitaVersion = table.Column<string>(type: "TEXT", nullable: true),
                    Count = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotVersionCount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SnapshotVersionCount_HistoricalSnapshot_HistoricalSnapshotId",
                        column: x => x.HistoricalSnapshotId,
                        principalTable: "HistoricalSnapshot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KavitaPlusPeriod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CancellingOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EndedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EndReason = table.Column<int>(type: "INTEGER", nullable: true),
                    IsFree = table.Column<bool>(type: "INTEGER", nullable: true),
                    Seeded = table.Column<bool>(type: "INTEGER", nullable: false),
                    InstallLifecycleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KavitaPlusPeriod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KavitaPlusPeriod_InstallLifecycle_InstallLifecycleId",
                        column: x => x.InstallLifecycleId,
                        principalTable: "InstallLifecycle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstallLifecycle_InstallId",
                table: "InstallLifecycle",
                column: "InstallId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KavitaPlusPeriod_InstallLifecycleId",
                table: "KavitaPlusPeriod",
                column: "InstallLifecycleId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotVersionCount_HistoricalSnapshotId",
                table: "SnapshotVersionCount",
                column: "HistoricalSnapshotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KavitaPlusPeriod");

            migrationBuilder.DropTable(
                name: "SnapshotVersionCount");

            migrationBuilder.DropTable(
                name: "InstallLifecycle");

            migrationBuilder.DropColumn(
                name: "ActiveInstalls",
                table: "HistoricalSnapshot");

            migrationBuilder.DropColumn(
                name: "ActiveUsers",
                table: "HistoricalSnapshot");

            migrationBuilder.DropColumn(
                name: "ChurnedInstalls",
                table: "HistoricalSnapshot");

            migrationBuilder.DropColumn(
                name: "InstallsWithContent",
                table: "HistoricalSnapshot");

            migrationBuilder.DropColumn(
                name: "NewInstalls",
                table: "HistoricalSnapshot");

            migrationBuilder.DropColumn(
                name: "OptedOutInstalls",
                table: "HistoricalSnapshot");
        }
    }
}
