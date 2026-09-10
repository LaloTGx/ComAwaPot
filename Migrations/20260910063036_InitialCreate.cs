using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComAwaPot.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AportacionesExtraordinarias",
                columns: table => new
                {
                    AportacionExtraordinariaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Concepto = table.Column<string>(type: "TEXT", nullable: false),
                    MontoTotal = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    MontoPorPersona = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaLimite = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Observaciones = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AportacionesExtraordinarias", x => x.AportacionExtraordinariaId);
                });

            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    PersonaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Calle = table.Column<string>(type: "TEXT", nullable: false),
                    NumExt = table.Column<int>(type: "INTEGER", nullable: false),
                    Estado = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.PersonaId);
                });

            migrationBuilder.CreateTable(
                name: "Tarifas",
                columns: table => new
                {
                    TarifaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Periodo = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MontoMensual = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Observaciones = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarifas", x => x.TarifaId);
                });

            migrationBuilder.CreateTable(
                name: "HistorialSituaciones",
                columns: table => new
                {
                    HistorialSituacionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PersonaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Estado = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialSituaciones", x => x.HistorialSituacionId);
                    table.ForeignKey(
                        name: "FK_HistorialSituaciones_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "PersonaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PagosAportacion",
                columns: table => new
                {
                    PagoAportacionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AportacionExtraordinariaId = table.Column<int>(type: "INTEGER", nullable: false),
                    PersonaId = table.Column<int>(type: "INTEGER", nullable: false),
                    MontoPagado = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    FechaPago = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Observaciones = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagosAportacion", x => x.PagoAportacionId);
                    table.ForeignKey(
                        name: "FK_PagosAportacion_AportacionesExtraordinarias_AportacionExtraordinariaId",
                        column: x => x.AportacionExtraordinariaId,
                        principalTable: "AportacionesExtraordinarias",
                        principalColumn: "AportacionExtraordinariaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PagosAportacion_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "PersonaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PagosTarifa",
                columns: table => new
                {
                    PagoTarifaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PersonaId = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaPago = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MontoTotal = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagosTarifa", x => x.PagoTarifaId);
                    table.ForeignKey(
                        name: "FK_PagosTarifa_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "PersonaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetallesPagoTarifa",
                columns: table => new
                {
                    DetallePagoTarifaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PagoTarifaId = table.Column<int>(type: "INTEGER", nullable: false),
                    TarifaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Periodo = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MontoAplicado = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesPagoTarifa", x => x.DetallePagoTarifaId);
                    table.ForeignKey(
                        name: "FK_DetallesPagoTarifa_PagosTarifa_PagoTarifaId",
                        column: x => x.PagoTarifaId,
                        principalTable: "PagosTarifa",
                        principalColumn: "PagoTarifaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesPagoTarifa_Tarifas_TarifaId",
                        column: x => x.TarifaId,
                        principalTable: "Tarifas",
                        principalColumn: "TarifaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPagoTarifa_PagoTarifaId",
                table: "DetallesPagoTarifa",
                column: "PagoTarifaId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPagoTarifa_TarifaId",
                table: "DetallesPagoTarifa",
                column: "TarifaId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialSituaciones_PersonaId",
                table: "HistorialSituaciones",
                column: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_PagosAportacion_AportacionExtraordinariaId",
                table: "PagosAportacion",
                column: "AportacionExtraordinariaId");

            migrationBuilder.CreateIndex(
                name: "IX_PagosAportacion_PersonaId",
                table: "PagosAportacion",
                column: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_PagosTarifa_PersonaId",
                table: "PagosTarifa",
                column: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_Personas_Calle",
                table: "Personas",
                column: "Calle");

            migrationBuilder.CreateIndex(
                name: "IX_Personas_Nombre",
                table: "Personas",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Tarifas_Periodo",
                table: "Tarifas",
                column: "Periodo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallesPagoTarifa");

            migrationBuilder.DropTable(
                name: "HistorialSituaciones");

            migrationBuilder.DropTable(
                name: "PagosAportacion");

            migrationBuilder.DropTable(
                name: "PagosTarifa");

            migrationBuilder.DropTable(
                name: "Tarifas");

            migrationBuilder.DropTable(
                name: "AportacionesExtraordinarias");

            migrationBuilder.DropTable(
                name: "Personas");
        }
    }
}
