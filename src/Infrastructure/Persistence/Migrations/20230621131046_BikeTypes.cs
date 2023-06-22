using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BikeTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BikeTypeId",
                table: "Bikes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Bikes",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Bikes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Bikes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "Bikes",
                type: "decimal(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "BikeTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BikeTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bikes_BikeTypeId",
                table: "Bikes",
                column: "BikeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Bikes_UserId",
                table: "Bikes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bikes_AspNetUsers_UserId",
                table: "Bikes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bikes_BikeTypes_BikeTypeId",
                table: "Bikes",
                column: "BikeTypeId",
                principalTable: "BikeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bikes_AspNetUsers_UserId",
                table: "Bikes");

            migrationBuilder.DropForeignKey(
                name: "FK_Bikes_BikeTypes_BikeTypeId",
                table: "Bikes");

            migrationBuilder.DropTable(
                name: "BikeTypes");

            migrationBuilder.DropIndex(
                name: "IX_Bikes_BikeTypeId",
                table: "Bikes");

            migrationBuilder.DropIndex(
                name: "IX_Bikes_UserId",
                table: "Bikes");

            migrationBuilder.DropColumn(
                name: "BikeTypeId",
                table: "Bikes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Bikes");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Bikes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Bikes");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Bikes");
        }
    }
}
