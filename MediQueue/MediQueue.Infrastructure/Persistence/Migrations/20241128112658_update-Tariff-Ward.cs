using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateTariffWard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalCost",
                table: "StationaryStay",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TariffWard",
                columns: table => new
                {
                    TariffsId = table.Column<int>(type: "int", nullable: false),
                    WardsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TariffWard", x => new { x.TariffsId, x.WardsId });
                    table.ForeignKey(
                        name: "FK_TariffWard_Tariff_TariffsId",
                        column: x => x.TariffsId,
                        principalTable: "Tariff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TariffWard_Ward_WardsId",
                        column: x => x.WardsId,
                        principalTable: "Ward",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TariffWard_WardsId",
                table: "TariffWard",
                column: "WardsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TariffWard");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "StationaryStay");
        }
    }
}
