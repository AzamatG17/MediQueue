using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateabalysisResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisResult_Account_AccountId",
                table: "AnalysisResult");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "AnalysisResult",
                newName: "SecondDoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_AnalysisResult_AccountId",
                table: "AnalysisResult",
                newName: "IX_AnalysisResult_SecondDoctorId");

            migrationBuilder.AddColumn<int>(
                name: "FirstDoctorId",
                table: "AnalysisResult",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisResult_FirstDoctorId",
                table: "AnalysisResult",
                column: "FirstDoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisResult_Account_FirstDoctorId",
                table: "AnalysisResult",
                column: "FirstDoctorId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisResult_Account_SecondDoctorId",
                table: "AnalysisResult",
                column: "SecondDoctorId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisResult_Account_FirstDoctorId",
                table: "AnalysisResult");

            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisResult_Account_SecondDoctorId",
                table: "AnalysisResult");

            migrationBuilder.DropIndex(
                name: "IX_AnalysisResult_FirstDoctorId",
                table: "AnalysisResult");

            migrationBuilder.DropColumn(
                name: "FirstDoctorId",
                table: "AnalysisResult");

            migrationBuilder.RenameColumn(
                name: "SecondDoctorId",
                table: "AnalysisResult",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_AnalysisResult_SecondDoctorId",
                table: "AnalysisResult",
                newName: "IX_AnalysisResult_AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisResult_Account_AccountId",
                table: "AnalysisResult",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
