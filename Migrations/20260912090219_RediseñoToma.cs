using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComAwaPot.Migrations
{
    /// <inheritdoc />
    public partial class RediseñoToma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistorialSituaciones_Personas_PersonaId",
                table: "HistorialSituaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_PagosTarifa_Personas_PersonaId",
                table: "PagosTarifa");

            migrationBuilder.DropIndex(
                name: "IX_Tarifas_Periodo",
                table: "Tarifas");

            migrationBuilder.DropIndex(
                name: "IX_Personas_Calle",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Tarifas");

            migrationBuilder.DropColumn(
                name: "Calle",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "NumExt",
                table: "Personas");

            migrationBuilder.RenameColumn(
                name: "PersonaId",
                table: "PagosTarifa",
                newName: "TomaId");

            migrationBuilder.RenameIndex(
                name: "IX_PagosTarifa_PersonaId",
                table: "PagosTarifa",
                newName: "IX_PagosTarifa_TomaId");

            migrationBuilder.RenameColumn(
                name: "PersonaId",
                table: "HistorialSituaciones",
                newName: "TomaId");

            migrationBuilder.RenameIndex(
                name: "IX_HistorialSituaciones_PersonaId",
                table: "HistorialSituaciones",
                newName: "IX_HistorialSituaciones_TomaId");

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "Tarifas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Tomas",
                columns: table => new
                {
                    TomaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumeroContrato = table.Column<int>(type: "INTEGER", nullable: false),
                    PersonaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Calle = table.Column<string>(type: "TEXT", nullable: false),
                    NumExt = table.Column<int>(type: "INTEGER", nullable: false),
                    Estado = table.Column<int>(type: "INTEGER", nullable: false),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tomas", x => x.TomaId);
                    table.ForeignKey(
                        name: "FK_Tomas_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "PersonaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tarifas_Tipo_Periodo",
                table: "Tarifas",
                columns: new[] { "Tipo", "Periodo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tomas_Calle",
                table: "Tomas",
                column: "Calle");

            migrationBuilder.CreateIndex(
                name: "IX_Tomas_NumeroContrato",
                table: "Tomas",
                column: "NumeroContrato",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tomas_PersonaId",
                table: "Tomas",
                column: "PersonaId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialSituaciones_Tomas_TomaId",
                table: "HistorialSituaciones",
                column: "TomaId",
                principalTable: "Tomas",
                principalColumn: "TomaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PagosTarifa_Tomas_TomaId",
                table: "PagosTarifa",
                column: "TomaId",
                principalTable: "Tomas",
                principalColumn: "TomaId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistorialSituaciones_Tomas_TomaId",
                table: "HistorialSituaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_PagosTarifa_Tomas_TomaId",
                table: "PagosTarifa");

            migrationBuilder.DropTable(
                name: "Tomas");

            migrationBuilder.DropIndex(
                name: "IX_Tarifas_Tipo_Periodo",
                table: "Tarifas");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Tarifas");

            migrationBuilder.RenameColumn(
                name: "TomaId",
                table: "PagosTarifa",
                newName: "PersonaId");

            migrationBuilder.RenameIndex(
                name: "IX_PagosTarifa_TomaId",
                table: "PagosTarifa",
                newName: "IX_PagosTarifa_PersonaId");

            migrationBuilder.RenameColumn(
                name: "TomaId",
                table: "HistorialSituaciones",
                newName: "PersonaId");

            migrationBuilder.RenameIndex(
                name: "IX_HistorialSituaciones_TomaId",
                table: "HistorialSituaciones",
                newName: "IX_HistorialSituaciones_PersonaId");

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Tarifas",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Calle",
                table: "Personas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "Personas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumExt",
                table: "Personas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tarifas_Periodo",
                table: "Tarifas",
                column: "Periodo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Personas_Calle",
                table: "Personas",
                column: "Calle");

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialSituaciones_Personas_PersonaId",
                table: "HistorialSituaciones",
                column: "PersonaId",
                principalTable: "Personas",
                principalColumn: "PersonaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PagosTarifa_Personas_PersonaId",
                table: "PagosTarifa",
                column: "PersonaId",
                principalTable: "Personas",
                principalColumn: "PersonaId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
