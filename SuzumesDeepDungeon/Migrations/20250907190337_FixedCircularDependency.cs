using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuzumesDeepDungeon.Migrations
{
    /// <inheritdoc />
    public partial class FixedCircularDependency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_twitchactionbase_twitchactions_twitchactionid",
                schema: "public",
                table: "twitchactionbase");

            migrationBuilder.DropIndex(
                name: "IX_twitchactionbase_twitchactionid",
                schema: "public",
                table: "twitchactionbase");

            migrationBuilder.DropColumn(
                name: "twitchactionid",
                schema: "public",
                table: "twitchactionbase");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "twitchactionid",
                schema: "public",
                table: "twitchactionbase",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_twitchactionbase_twitchactionid",
                schema: "public",
                table: "twitchactionbase",
                column: "twitchactionid");

            migrationBuilder.AddForeignKey(
                name: "FK_twitchactionbase_twitchactions_twitchactionid",
                schema: "public",
                table: "twitchactionbase",
                column: "twitchactionid",
                principalSchema: "public",
                principalTable: "twitchactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
