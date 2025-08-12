using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuzumesDeepDungeon.Migrations
{
    /// <inheritdoc />
    public partial class AddedColumnRealesdToGameRank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameRanks_Users_UserId",
                table: "GameRanks");

            migrationBuilder.AlterColumn<double>(
                name: "Rate",
                table: "GameRanks",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AddColumn<string>(
                name: "Released",
                table: "GameRanks",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GameRanks_Users_UserId",
                table: "GameRanks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameRanks_Users_UserId",
                table: "GameRanks");

            migrationBuilder.DropColumn(
                name: "Released",
                table: "GameRanks");

            migrationBuilder.AlterColumn<double>(
                name: "Rate",
                table: "GameRanks",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL",
                oldDefaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_GameRanks_Users_UserId",
                table: "GameRanks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
