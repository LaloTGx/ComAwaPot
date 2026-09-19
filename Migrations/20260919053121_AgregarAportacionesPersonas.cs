using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComAwaPot.Migrations
{
    /// <inheritdoc />
    public partial class AgregarAportacionesPersonas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MontoTotal",
                table: "AportacionesExtraordinarias");

            migrationBuilder.CreateTable(
                name: "AportacionesPersonas",
                columns: table => new
                {
                    AportacionPersonaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AportacionExtraordinariaId = table.Column<int>(type: "INTEGER", nullable: false),
                    PersonaId = table.Column<int>(type: "INTEGER", nullable: false),
                    MontoEsperado = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    PersonaId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AportacionesPersonas", x => x.AportacionPersonaId);
                    table.ForeignKey(
                        name: "FK_AportacionesPersonas_AportacionesExtraordinarias_AportacionExtraordinariaId",
                        column: x => x.AportacionExtraordinariaId,
                        principalTable: "AportacionesExtraordinarias",
                        principalColumn: "AportacionExtraordinariaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AportacionesPersonas_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "PersonaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AportacionesPersonas_Personas_PersonaId1",
                        column: x => x.PersonaId1,
                        principalTable: "Personas",
                        principalColumn: "PersonaId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AportacionesPersonas_AportacionExtraordinariaId_PersonaId",
                table: "AportacionesPersonas",
                columns: new[] { "AportacionExtraordinariaId", "PersonaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AportacionesPersonas_PersonaId",
                table: "AportacionesPersonas",
                column: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_AportacionesPersonas_PersonaId1",
                table: "AportacionesPersonas",
                column: "PersonaId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AportacionesPersonas");

            migrationBuilder.AddColumn<decimal>(
                name: "MontoTotal",
                table: "AportacionesExtraordinarias",
                type: "TEXT",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
