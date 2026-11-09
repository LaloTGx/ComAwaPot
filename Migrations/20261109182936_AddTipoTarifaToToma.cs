using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComAwaPot.Migrations
{
    /// <inheritdoc />
    public partial class AddTipoTarifaToToma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoTarifa",
                table: "Tomas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoTarifa",
                table: "Tomas");
        }
    }
}
