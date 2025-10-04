using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARCIAL_programaci_n.Migrations
{
    /// <inheritdoc />
    public partial class FinalSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "Asistencias");

            migrationBuilder.RenameColumn(
                name: "FechaSesion",
                table: "Sesiones",
                newName: "Fecha");

            migrationBuilder.RenameColumn(
                name: "Asistio",
                table: "Asistencias",
                newName: "Presente");

            migrationBuilder.AlterColumn<string>(
                name: "Tema",
                table: "Sesiones",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Asistencias",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Asistencias");

            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "Sesiones",
                newName: "FechaSesion");

            migrationBuilder.RenameColumn(
                name: "Presente",
                table: "Asistencias",
                newName: "Asistio");

            migrationBuilder.AlterColumn<string>(
                name: "Tema",
                table: "Sesiones",
                type: "TEXT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "Asistencias",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
