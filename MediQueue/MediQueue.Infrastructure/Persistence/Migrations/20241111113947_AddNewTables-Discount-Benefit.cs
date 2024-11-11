using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTablesDiscountBenefit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "InitialBenefitPercentage",
                table: "QuestionnaireHistory",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InitialDiscountPercentage",
                table: "QuestionnaireHistory",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Benefit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Percent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuestionnaireHistoryId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Benefit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Benefit_QuestionnaireHistory_QuestionnaireHistoryId",
                        column: x => x.QuestionnaireHistoryId,
                        principalTable: "QuestionnaireHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Discount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Percent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuestionnaireHistoryId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Discount_QuestionnaireHistory_QuestionnaireHistoryId",
                        column: x => x.QuestionnaireHistoryId,
                        principalTable: "QuestionnaireHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Benefit_QuestionnaireHistoryId",
                table: "Benefit",
                column: "QuestionnaireHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_QuestionnaireHistoryId",
                table: "Discount",
                column: "QuestionnaireHistoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Benefit");

            migrationBuilder.DropTable(
                name: "Discount");

            migrationBuilder.DropColumn(
                name: "InitialBenefitPercentage",
                table: "QuestionnaireHistory");

            migrationBuilder.DropColumn(
                name: "InitialDiscountPercentage",
                table: "QuestionnaireHistory");
        }
    }
}
