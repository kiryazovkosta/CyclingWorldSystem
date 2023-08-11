using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ActivityIdIndexInWaypoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDateTime",
                table: "Activities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 11, 5, 48, 50, 399, DateTimeKind.Utc).AddTicks(2964),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 8, 10, 10, 0, 4, 627, DateTimeKind.Utc).AddTicks(2186));

            migrationBuilder.CreateIndex(
                name: "UX_Waypoints_ActivityId",
                table: "Waypoints",
                column: "ActivityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Waypoints_ActivityId",
                table: "Waypoints");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDateTime",
                table: "Activities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 10, 10, 0, 4, 627, DateTimeKind.Utc).AddTicks(2186),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 8, 11, 5, 48, 50, 399, DateTimeKind.Utc).AddTicks(2964));
        }
    }
}
