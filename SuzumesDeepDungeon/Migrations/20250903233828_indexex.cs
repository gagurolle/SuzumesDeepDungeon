using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuzumesDeepDungeon.Migrations
{
    /// <inheritdoc />
    public partial class indexex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                schema: "public",
                table: "users",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                schema: "public",
                table: "users",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "IX_gameranks_created",
                schema: "public",
                table: "gameranks",
                column: "created");

            migrationBuilder.CreateIndex(
                name: "IX_gameranks_rate",
                schema: "public",
                table: "gameranks",
                column: "rate");

            migrationBuilder.CreateIndex(
                name: "IX_gameranks_released",
                schema: "public",
                table: "gameranks",
                column: "released");

            migrationBuilder.CreateIndex(
                name: "IX_gameranks_status",
                schema: "public",
                table: "gameranks",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_gameranks_updated",
                schema: "public",
                table: "gameranks",
                column: "updated");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_email",
                schema: "public",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_username",
                schema: "public",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_gameranks_created",
                schema: "public",
                table: "gameranks");

            migrationBuilder.DropIndex(
                name: "IX_gameranks_rate",
                schema: "public",
                table: "gameranks");

            migrationBuilder.DropIndex(
                name: "IX_gameranks_released",
                schema: "public",
                table: "gameranks");

            migrationBuilder.DropIndex(
                name: "IX_gameranks_status",
                schema: "public",
                table: "gameranks");

            migrationBuilder.DropIndex(
                name: "IX_gameranks_updated",
                schema: "public",
                table: "gameranks");
        }
    }
}
