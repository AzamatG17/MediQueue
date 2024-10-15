using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLekarstvoUsageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConclusionLekarstva");

            migrationBuilder.CreateTable(
                name: "LekarstvoUsage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConclusionId = table.Column<int>(type: "int", nullable: true),
                    LekarstvoId = table.Column<int>(type: "int", nullable: true),
                    QuantityUsed = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LekarstvoUsage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LekarstvoUsage_Conclusion_ConclusionId",
                        column: x => x.ConclusionId,
                        principalTable: "Conclusion",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LekarstvoUsage_Lekarstvo_LekarstvoId",
                        column: x => x.LekarstvoId,
                        principalTable: "Lekarstvo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LekarstvoUsage_ConclusionId",
                table: "LekarstvoUsage",
                column: "ConclusionId");

            migrationBuilder.CreateIndex(
                name: "IX_LekarstvoUsage_LekarstvoId",
                table: "LekarstvoUsage",
                column: "LekarstvoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LekarstvoUsage");

            migrationBuilder.CreateTable(
                name: "ConclusionLekarstva",
                columns: table => new
                {
                    ConclusionsId = table.Column<int>(type: "int", nullable: false),
                    LekarstvaUsedByDoctorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConclusionLekarstva", x => new { x.ConclusionsId, x.LekarstvaUsedByDoctorId });
                    table.ForeignKey(
                        name: "FK_ConclusionLekarstva_Conclusion_ConclusionsId",
                        column: x => x.ConclusionsId,
                        principalTable: "Conclusion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConclusionLekarstva_Lekarstvo_LekarstvaUsedByDoctorId",
                        column: x => x.LekarstvaUsedByDoctorId,
                        principalTable: "Lekarstvo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConclusionLekarstva_LekarstvaUsedByDoctorId",
                table: "ConclusionLekarstva",
                column: "LekarstvaUsedByDoctorId");
        }
    }
}
