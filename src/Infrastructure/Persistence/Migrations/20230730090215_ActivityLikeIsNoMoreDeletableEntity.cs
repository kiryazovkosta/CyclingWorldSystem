using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ActivityLikeIsNoMoreDeletableEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Activities_ActivityId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Likes");

            migrationBuilder.AlterColumn<decimal>(
                name: "Temperature",
                table: "Waypoints",
                type: "decimal(12,3)",
                precision: 12,
                scale: 3,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Elevation",
                table: "Waypoints",
                type: "decimal(12,3)",
                precision: 12,
                scale: 3,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDateTime",
                table: "Activities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 9, 2, 15, 381, DateTimeKind.Utc).AddTicks(9675),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 25, 14, 48, 52, 768, DateTimeKind.Utc).AddTicks(5870));

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Activities_ActivityId",
                table: "Likes",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Activities_ActivityId",
                table: "Likes");

            migrationBuilder.AlterColumn<decimal>(
                name: "Temperature",
                table: "Waypoints",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,3)",
                oldPrecision: 12,
                oldScale: 3);

            migrationBuilder.AlterColumn<decimal>(
                name: "Elevation",
                table: "Waypoints",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,3)",
                oldPrecision: 12,
                oldScale: 3);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Likes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Likes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Likes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Likes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Likes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDateTime",
                table: "Activities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 25, 14, 48, 52, 768, DateTimeKind.Utc).AddTicks(5870),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 30, 9, 2, 15, 381, DateTimeKind.Utc).AddTicks(9675));

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Activities_ActivityId",
                table: "Likes",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
