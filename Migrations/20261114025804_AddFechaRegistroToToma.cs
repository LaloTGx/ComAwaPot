using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComAwaPot.Migrations
{
    /// <inheritdoc />
    public partial class AddFechaRegistroToToma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
		{
		    migrationBuilder.AddColumn<DateTime>(
		        name: "FechaRegistro",
		        table: "Tomas",
		        type: "TEXT",
		        nullable: false,
		        defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

		    migrationBuilder.Sql(
		        """
		        UPDATE Tomas
		        SET FechaRegistro = (
		            SELECT MIN(HistorialSituaciones.FechaInicio)
		            FROM HistorialSituaciones
		            WHERE HistorialSituaciones.TomaId = Tomas.TomaId
		        )
		        WHERE EXISTS (
		            SELECT 1
		            FROM HistorialSituaciones
		            WHERE HistorialSituaciones.TomaId = Tomas.TomaId
		        );
		        """);
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "Tomas");
        }
    }
}
