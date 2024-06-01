using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarvelApi_Api.Migrations
{
    /// <inheritdoc />
    public partial class ModifyTeamModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2024, 5, 30, 0, 14, 7, 798, DateTimeKind.Utc).AddTicks(8450));

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2024, 5, 30, 0, 14, 7, 798, DateTimeKind.Utc).AddTicks(8450));

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2024, 5, 30, 0, 14, 7, 798, DateTimeKind.Utc).AddTicks(8450));

            migrationBuilder.UpdateData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2024, 5, 30, 0, 14, 7, 798, DateTimeKind.Utc).AddTicks(8500));

            migrationBuilder.UpdateData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2024, 5, 30, 0, 14, 7, 798, DateTimeKind.Utc).AddTicks(8500));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2024, 5, 28, 16, 57, 52, 381, DateTimeKind.Utc).AddTicks(9360));

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2024, 5, 28, 16, 57, 52, 381, DateTimeKind.Utc).AddTicks(9370));

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2024, 5, 28, 16, 57, 52, 381, DateTimeKind.Utc).AddTicks(9370));

            migrationBuilder.UpdateData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2024, 5, 28, 16, 57, 52, 381, DateTimeKind.Utc).AddTicks(9410));

            migrationBuilder.UpdateData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2024, 5, 28, 16, 57, 52, 381, DateTimeKind.Utc).AddTicks(9410));
        }
    }
}
