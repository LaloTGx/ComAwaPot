using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComAwaPot.Migrations
{
    /// <inheritdoc />
    public partial class AjustesFinales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Tomas");

            migrationBuilder.CreateTable(
                name: "TarifaTomas",
                columns: table => new
                {
                    TarifaTomaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TomaId = table.Column<int>(type: "INTEGER", nullable: false),
                    TarifaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TarifaTomas", x => x.TarifaTomaId);
                    table.ForeignKey(
                        name: "FK_TarifaTomas_Tarifas_TarifaId",
                        column: x => x.TarifaId,
                        principalTable: "Tarifas",
                        principalColumn: "TarifaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TarifaTomas_Tomas_TomaId",
                        column: x => x.TomaId,
                        principalTable: "Tomas",
                        principalColumn: "TomaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TarifaTomas_TarifaId",
                table: "TarifaTomas",
                column: "TarifaId");

            migrationBuilder.CreateIndex(
                name: "IX_TarifaTomas_TomaId_TarifaId",
                table: "TarifaTomas",
                columns: new[] { "TomaId", "TarifaId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TarifaTomas");

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "Tomas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
