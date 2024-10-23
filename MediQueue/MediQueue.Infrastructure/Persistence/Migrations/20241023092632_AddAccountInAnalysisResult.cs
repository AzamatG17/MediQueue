using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountInAnalysisResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "AnalysisResult",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisResult_AccountId",
                table: "AnalysisResult",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisResult_Account_AccountId",
                table: "AnalysisResult",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisResult_Account_AccountId",
                table: "AnalysisResult");

            migrationBuilder.DropIndex(
                name: "IX_AnalysisResult_AccountId",
                table: "AnalysisResult");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "AnalysisResult");
        }
    }
}
