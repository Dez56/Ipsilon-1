using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ipsilon_1A.Migrations
{
    /// <inheritdoc />
    public partial class NombreDeLaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "HorEnt",
                table: "Paquetes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HorSal",
                table: "Paquetes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HorEnt",
                table: "Paquetes");

            migrationBuilder.DropColumn(
                name: "HorSal",
                table: "Paquetes");
        }
    }
}
