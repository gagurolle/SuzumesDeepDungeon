using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuzumesDeepDungeon.Migrations
{
    /// <inheritdoc />
    public partial class AllEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User",
                table: "GameRanks");

            migrationBuilder.AlterColumn<double>(
                name: "Rate",
                table: "GameRanks",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<double>(
                name: "MetacriticRate",
                table: "GameRanks",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "GameRanks",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YoutubeLink",
                table: "GameRanks",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Achievements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    CompletionPercent = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Achievements_GameRanks_GameId",
                        column: x => x.GameId,
                        principalTable: "GameRanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Screenshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    SteamHeaderUrl = table.Column<string>(type: "TEXT", nullable: false),
                    SteamCapsuleUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Steam600x900Url = table.Column<string>(type: "TEXT", nullable: false),
                    RawgBackgroundUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screenshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Screenshots_GameRanks_GameId",
                        column: x => x.GameId,
                        principalTable: "GameRanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    RawgId = table.Column<string>(type: "TEXT", nullable: false),
                    StoreId = table.Column<int>(type: "INTEGER", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stores_GameRanks_GameId",
                        column: x => x.GameId,
                        principalTable: "GameRanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Slug = table.Column<string>(type: "TEXT", nullable: false),
                    Language = table.Column<string>(type: "TEXT", nullable: false),
                    GamesCount = table.Column<int>(type: "INTEGER", nullable: false),
                    ImageBackground = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tag_GameRanks_GameId",
                        column: x => x.GameId,
                        principalTable: "GameRanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trailers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PreviewImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Video480p = table.Column<string>(type: "TEXT", nullable: false),
                    VideoMaxQuality = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trailers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trailers_GameRanks_GameId",
                        column: x => x.GameId,
                        principalTable: "GameRanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameRanks_UserId",
                table: "GameRanks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_GameId",
                table: "Achievements",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Screenshots_GameId",
                table: "Screenshots",
                column: "GameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stores_GameId",
                table: "Stores",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_GameId",
                table: "Tag",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Trailers_GameId",
                table: "Trailers",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameRanks_Users_UserId",
                table: "GameRanks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameRanks_Users_UserId",
                table: "GameRanks");

            migrationBuilder.DropTable(
                name: "Achievements");

            migrationBuilder.DropTable(
                name: "Screenshots");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Trailers");

            migrationBuilder.DropIndex(
                name: "IX_GameRanks_UserId",
                table: "GameRanks");

            migrationBuilder.DropColumn(
                name: "MetacriticRate",
                table: "GameRanks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GameRanks");

            migrationBuilder.DropColumn(
                name: "YoutubeLink",
                table: "GameRanks");

            migrationBuilder.AlterColumn<int>(
                name: "Rate",
                table: "GameRanks",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "GameRanks",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
