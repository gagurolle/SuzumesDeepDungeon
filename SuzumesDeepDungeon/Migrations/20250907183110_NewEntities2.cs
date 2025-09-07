using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuzumesDeepDungeon.Migrations
{
    /// <inheritdoc />
    public partial class NewEntities2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_twitchactions_twitchactionbase_commandtriggeredid",
                schema: "public",
                table: "twitchactions");

            migrationBuilder.DropForeignKey(
                name: "FK_twitchactions_twitchactionbase_rewardredemptionid",
                schema: "public",
                table: "twitchactions");

            migrationBuilder.AddForeignKey(
                name: "FK_twitchactions_twitchactionbase_commandtriggeredid",
                schema: "public",
                table: "twitchactions",
                column: "commandtriggeredid",
                principalSchema: "public",
                principalTable: "twitchactionbase",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_twitchactions_twitchactionbase_rewardredemptionid",
                schema: "public",
                table: "twitchactions",
                column: "rewardredemptionid",
                principalSchema: "public",
                principalTable: "twitchactionbase",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_twitchactions_twitchactionbase_commandtriggeredid",
                schema: "public",
                table: "twitchactions");

            migrationBuilder.DropForeignKey(
                name: "FK_twitchactions_twitchactionbase_rewardredemptionid",
                schema: "public",
                table: "twitchactions");

            migrationBuilder.AddForeignKey(
                name: "FK_twitchactions_twitchactionbase_commandtriggeredid",
                schema: "public",
                table: "twitchactions",
                column: "commandtriggeredid",
                principalSchema: "public",
                principalTable: "twitchactionbase",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_twitchactions_twitchactionbase_rewardredemptionid",
                schema: "public",
                table: "twitchactions",
                column: "rewardredemptionid",
                principalSchema: "public",
                principalTable: "twitchactionbase",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
