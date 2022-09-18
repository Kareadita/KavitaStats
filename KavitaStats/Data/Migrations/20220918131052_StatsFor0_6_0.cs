using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KavitaStats.Data.Migrations
{
    public partial class StatsFor0_6_0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OptedOut",
                table: "StatRecord",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Color",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<string>(type: "TEXT", nullable: true),
                    StatRecordId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Color_StatRecord_StatRecordId",
                        column: x => x.StatRecordId,
                        principalTable: "StatRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileFormat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Extension = table.Column<string>(type: "TEXT", nullable: true),
                    Format = table.Column<int>(type: "INTEGER", nullable: false),
                    StatRecordId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileFormat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileFormat_StatRecord_StatRecordId",
                        column: x => x.StatRecordId,
                        principalTable: "StatRecord",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MangaReaderLayoutMode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReaderMode = table.Column<int>(type: "INTEGER", nullable: false),
                    StatRecordId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaReaderLayoutMode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MangaReaderLayoutMode_StatRecord_StatRecordId",
                        column: x => x.StatRecordId,
                        principalTable: "StatRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PageSplit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PageSplitOption = table.Column<int>(type: "INTEGER", nullable: false),
                    StatRecordId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageSplit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageSplit_StatRecord_StatRecordId",
                        column: x => x.StatRecordId,
                        principalTable: "StatRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Color_StatRecordId",
                table: "Color",
                column: "StatRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_FileFormat_StatRecordId",
                table: "FileFormat",
                column: "StatRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaReaderLayoutMode_StatRecordId",
                table: "MangaReaderLayoutMode",
                column: "StatRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_PageSplit_StatRecordId",
                table: "PageSplit",
                column: "StatRecordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Color");

            migrationBuilder.DropTable(
                name: "FileFormat");

            migrationBuilder.DropTable(
                name: "MangaReaderLayoutMode");

            migrationBuilder.DropTable(
                name: "PageSplit");

            migrationBuilder.DropColumn(
                name: "OptedOut",
                table: "StatRecord");
        }
    }
}
