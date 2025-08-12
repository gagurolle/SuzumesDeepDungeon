using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuzumesDeepDungeon.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "user",
                table: "GameRanks",
                newName: "User");

            migrationBuilder.RenameColumn(
                name: "updated",
                table: "GameRanks",
                newName: "Updated");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "GameRanks",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "review",
                table: "GameRanks",
                newName: "Review");

            migrationBuilder.RenameColumn(
                name: "rate",
                table: "GameRanks",
                newName: "Rate");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "GameRanks",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "GameRanks",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "gameTime",
                table: "GameRanks",
                newName: "GameTime");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "GameRanks",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "GameRanks",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "User",
                table: "GameRanks",
                newName: "user");

            migrationBuilder.RenameColumn(
                name: "Updated",
                table: "GameRanks",
                newName: "updated");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "GameRanks",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Review",
                table: "GameRanks",
                newName: "review");

            migrationBuilder.RenameColumn(
                name: "Rate",
                table: "GameRanks",
                newName: "rate");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "GameRanks",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "GameRanks",
                newName: "image");

            migrationBuilder.RenameColumn(
                name: "GameTime",
                table: "GameRanks",
                newName: "gameTime");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "GameRanks",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "GameRanks",
                newName: "id");
        }
    }
}
