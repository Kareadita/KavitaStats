using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KavitaStats.Data.Migrations.V3
{
    /// <inheritdoc />
    public partial class JsonColumnsAndMetadataDl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LibraryStatFileTypeGroup");

            migrationBuilder.DropTable(
                name: "UserStatDevicePlatform");

            migrationBuilder.DropTable(
                name: "UserStatRole");

            migrationBuilder.AddColumn<string>(
                name: "DevicePlatforms",
                table: "UserStat",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Roles",
                table: "UserStat",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MatchedMetadataEnabled",
                table: "ServerStat",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FileTypes",
                table: "LibraryStat",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DevicePlatforms",
                table: "UserStat");

            migrationBuilder.DropColumn(
                name: "Roles",
                table: "UserStat");

            migrationBuilder.DropColumn(
                name: "MatchedMetadataEnabled",
                table: "ServerStat");

            migrationBuilder.DropColumn(
                name: "FileTypes",
                table: "LibraryStat");

            migrationBuilder.CreateTable(
                name: "LibraryStatFileTypeGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LibraryStatId = table.Column<int>(type: "INTEGER", nullable: false),
                    FileType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryStatFileTypeGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryStatFileTypeGroup_LibraryStat_LibraryStatId",
                        column: x => x.LibraryStatId,
                        principalTable: "LibraryStat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStatDevicePlatform",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserStatId = table.Column<int>(type: "INTEGER", nullable: false),
                    DevicePlatform = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStatDevicePlatform", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStatDevicePlatform_UserStat_UserStatId",
                        column: x => x.UserStatId,
                        principalTable: "UserStat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStatRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserStatId = table.Column<int>(type: "INTEGER", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStatRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStatRole_UserStat_UserStatId",
                        column: x => x.UserStatId,
                        principalTable: "UserStat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LibraryStatFileTypeGroup_LibraryStatId",
                table: "LibraryStatFileTypeGroup",
                column: "LibraryStatId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStatDevicePlatform_UserStatId",
                table: "UserStatDevicePlatform",
                column: "UserStatId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStatRole_UserStatId",
                table: "UserStatRole",
                column: "UserStatId");
        }
    }
}
