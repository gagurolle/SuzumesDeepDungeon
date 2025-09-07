using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SuzumesDeepDungeon.Migrations
{
    /// <inheritdoc />
    public partial class NewTwitchSystemActionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tags",
                schema: "public",
                table: "twitchusers");

            migrationBuilder.CreateTable(
                name: "twitchsystemactions",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    actionid = table.Column<Guid>(type: "uuid", nullable: true),
                    actionname = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_twitchsystemactions", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "twitchsystemactions",
                schema: "public");

            migrationBuilder.AddColumn<List<string>>(
                name: "tags",
                schema: "public",
                table: "twitchusers",
                type: "text[]",
                nullable: true);
        }
    }
}
