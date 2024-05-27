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
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharacterAllies",
                columns: table => new
                {
                    AllyId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterAllies", x => new { x.AllyId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_CharacterAllies_Characters_AllyId",
                        column: x => x.AllyId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterAllies_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterEnemies",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    EnemyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterEnemies", x => new { x.CharacterId, x.EnemyId });
                    table.ForeignKey(
                        name: "FK_CharacterEnemies_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterEnemies_Characters_EnemyId",
                        column: x => x.EnemyId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Characters",
                columns: new[] { "Id", "Appearance", "Backstory", "DateCreated", "DateUpdated", "Image", "IsVillain", "Name", "Origin", "Powers" },
                values: new object[,]
                {
                    { 1, "Avengers", "Was  rich man", new DateTime(2024, 5, 27, 11, 10, 38, 401, DateTimeKind.Utc).AddTicks(8190), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Iron man", "New York", "[\"laser\",\"Big laser\"]" },
                    { 2, "Avengers", "Was  rich man", new DateTime(2024, 5, 27, 11, 10, 38, 401, DateTimeKind.Utc).AddTicks(8190), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "Thanos", "New York", "[\"laser\",\"Big laser\"]" },
                    { 3, "Avengers", "Was  rich man", new DateTime(2024, 5, 27, 11, 10, 38, 401, DateTimeKind.Utc).AddTicks(8190), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Daredevil", "New York", "[\"laser\",\"Big laser\"]" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterAllies_CharacterId",
                table: "CharacterAllies",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterEnemies_EnemyId",
                table: "CharacterEnemies",
                column: "EnemyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterAllies");

            migrationBuilder.DropTable(
                name: "CharacterEnemies");

            migrationBuilder.DropTable(
                name: "Characters");
        }
    }
}
