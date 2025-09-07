using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SuzumesDeepDungeon.Migrations
{
    /// <inheritdoc />
    public partial class NewTwitchUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "twitchusers",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: true),
                    profileimageurl = table.Column<string>(type: "text", nullable: true),
                    usertype = table.Column<string>(type: "text", nullable: true),
                    isfollowing = table.Column<bool>(type: "boolean", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    accountage = table.Column<double>(type: "double precision", nullable: true),
                    game = table.Column<string>(type: "text", nullable: true),
                    gameid = table.Column<string>(type: "text", nullable: true),
                    channeltitle = table.Column<string>(type: "text", nullable: true),
                    tags = table.Column<List<string>>(type: "text[]", nullable: true),
                    issubscribed = table.Column<bool>(type: "boolean", nullable: true),
                    subscriptiontier = table.Column<string>(type: "text", nullable: true),
                    ismoderator = table.Column<bool>(type: "boolean", nullable: true),
                    isvip = table.Column<bool>(type: "boolean", nullable: true),
                    username = table.Column<string>(type: "text", nullable: true),
                    userlogin = table.Column<string>(type: "text", nullable: true),
                    userid = table.Column<string>(type: "text", nullable: false),
                    lastactive = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    previousactive = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_twitchusers", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "twitchusers",
                schema: "public");
        }
    }
}
