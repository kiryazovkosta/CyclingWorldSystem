using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ExtendActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "NegativeElevation",
                table: "Activities",
                type: "decimal(7,3)",
                precision: 7,
                scale: 3,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PositiveElevation",
                table: "Activities",
                type: "decimal(7,3)",
                precision: 7,
                scale: 3,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateTime",
                table: "Activities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 25, 14, 48, 52, 768, DateTimeKind.Utc).AddTicks(5870));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NegativeElevation",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "PositiveElevation",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "StartDateTime",
                table: "Activities");
        }
    }
}
