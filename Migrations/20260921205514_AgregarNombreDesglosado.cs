using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComAwaPot.Migrations
{
    /// <inheritdoc />
    public partial class AgregarNombreDesglosado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrimerApellido",
                table: "Personas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SegundoApellido",
                table: "Personas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SegundoNombre",
                table: "Personas",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimerApellido",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "SegundoApellido",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "SegundoNombre",
                table: "Personas");
        }
    }
}
