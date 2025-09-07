using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SuzumesDeepDungeon.Migrations
{
    /// <inheritdoc />
    public partial class NewEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "twitchactionbase",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<int>(type: "integer", nullable: true),
                    twitchactionid = table.Column<int>(type: "integer", nullable: false),
                    systemactionid = table.Column<int>(type: "integer", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    actiontype = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    command = table.Column<string>(type: "text", nullable: true),
                    commandname = table.Column<string>(type: "text", nullable: true),
                    commandsource = table.Column<string>(type: "text", nullable: true),
                    commandtype = table.Column<string>(type: "text", nullable: true),
                    isreply = table.Column<bool>(type: "boolean", nullable: true),
                    rewardname = table.Column<string>(type: "text", nullable: true),
                    rewardprompt = table.Column<string>(type: "text", nullable: true),
                    rewardcost = table.Column<string>(type: "text", nullable: true),
                    counter = table.Column<string>(type: "text", nullable: true),
                    usercounter = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_twitchactionbase", x => x.id);
                    table.ForeignKey(
                        name: "FK_twitchactionbase_twitchsystemactions_systemactionid",
                        column: x => x.systemactionid,
                        principalSchema: "public",
                        principalTable: "twitchsystemactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_twitchactionbase_twitchusers_userid",
                        column: x => x.userid,
                        principalSchema: "public",
                        principalTable: "twitchusers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "twitchactions",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<int>(type: "integer", nullable: true),
                    systemactionid = table.Column<int>(type: "integer", nullable: true),
                    rewardredemptionid = table.Column<int>(type: "integer", nullable: true),
                    commandtriggeredid = table.Column<int>(type: "integer", nullable: true),
                    rowdata = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_twitchactions", x => x.id);
                    table.ForeignKey(
                        name: "FK_twitchactions_twitchactionbase_commandtriggeredid",
                        column: x => x.commandtriggeredid,
                        principalSchema: "public",
                        principalTable: "twitchactionbase",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_twitchactions_twitchactionbase_rewardredemptionid",
                        column: x => x.rewardredemptionid,
                        principalSchema: "public",
                        principalTable: "twitchactionbase",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_twitchactions_twitchsystemactions_systemactionid",
                        column: x => x.systemactionid,
                        principalSchema: "public",
                        principalTable: "twitchsystemactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_twitchactions_twitchusers_userid",
                        column: x => x.userid,
                        principalSchema: "public",
                        principalTable: "twitchusers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_twitchactionbase_systemactionid",
                schema: "public",
                table: "twitchactionbase",
                column: "systemactionid");

            migrationBuilder.CreateIndex(
                name: "IX_twitchactionbase_twitchactionid",
                schema: "public",
                table: "twitchactionbase",
                column: "twitchactionid");

            migrationBuilder.CreateIndex(
                name: "IX_twitchactionbase_userid",
                schema: "public",
                table: "twitchactionbase",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_twitchactions_commandtriggeredid",
                schema: "public",
                table: "twitchactions",
                column: "commandtriggeredid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_twitchactions_rewardredemptionid",
                schema: "public",
                table: "twitchactions",
                column: "rewardredemptionid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_twitchactions_systemactionid",
                schema: "public",
                table: "twitchactions",
                column: "systemactionid");

            migrationBuilder.CreateIndex(
                name: "IX_twitchactions_userid",
                schema: "public",
                table: "twitchactions",
                column: "userid");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_twitchactionbase_twitchactions_twitchactionid",
                schema: "public",
                table: "twitchactionbase");

            migrationBuilder.DropTable(
                name: "twitchactions",
                schema: "public");

            migrationBuilder.DropTable(
                name: "twitchactionbase",
                schema: "public");
        }
    }
}
