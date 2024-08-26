using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarvelApi_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddImageTypeToCharacterAndTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageType",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageType",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageType",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "ImageType",
                table: "Characters");
        }
    }
}
