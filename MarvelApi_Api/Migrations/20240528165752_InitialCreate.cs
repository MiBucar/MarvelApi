using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MarvelApi_Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVillain = table.Column<bool>(type: "bit", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Backstory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Appearance = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Powers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CharacterRelationships",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    RelatedCharacterId = table.Column<int>(type: "int", nullable: false),
                    IsEnemy = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterRelationships", x => new { x.CharacterId, x.RelatedCharacterId });
                    table.ForeignKey(
                        name: "FK_CharacterRelationships_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CharacterRelationships_Characters_RelatedCharacterId",
                        column: x => x.RelatedCharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Characters",
                columns: new[] { "Id", "Appearance", "Backstory", "DateCreated", "DateUpdated", "Image", "IsVillain", "Name", "Origin", "Powers", "TeamId" },
                values: new object[,]
                {
                    { 1, "Avengers", "Was  rich man", new DateTime(2024, 5, 28, 16, 57, 52, 381, DateTimeKind.Utc).AddTicks(9360), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Iron man", "New York", "[\"laser\",\"Big laser\"]", null },
                    { 2, "Avengers", "Was  rich man", new DateTime(2024, 5, 28, 16, 57, 52, 381, DateTimeKind.Utc).AddTicks(9370), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "Thanos", "New York", "[\"laser\",\"Big laser\"]", null },
                    { 3, "Avengers", "Was  rich man", new DateTime(2024, 5, 28, 16, 57, 52, 381, DateTimeKind.Utc).AddTicks(9370), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Daredevil", "New York", "[\"laser\",\"Big laser\"]", null }
                });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "DateCreated", "DateUpdated", "Description", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 5, 28, 16, 57, 52, 381, DateTimeKind.Utc).AddTicks(9410), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hero team.", "Avengers" },
                    { 2, new DateTime(2024, 5, 28, 16, 57, 52, 381, DateTimeKind.Utc).AddTicks(9410), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Team trying to destroy the world.", "Children of Thanos" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterRelationships_RelatedCharacterId",
                table: "CharacterRelationships",
                column: "RelatedCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_TeamId",
                table: "Characters",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterRelationships");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
