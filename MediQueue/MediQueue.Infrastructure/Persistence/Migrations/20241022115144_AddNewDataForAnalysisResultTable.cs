using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNewDataForAnalysisResultTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisResult_QuestionnaireHistory_QuestionnaireHistoryId",
                table: "AnalysisResult");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "AnalysisResult",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "MeasuredValue",
                table: "AnalysisResult",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisResult_QuestionnaireHistory_QuestionnaireHistoryId",
                table: "AnalysisResult",
                column: "QuestionnaireHistoryId",
                principalTable: "QuestionnaireHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisResult_QuestionnaireHistory_QuestionnaireHistoryId",
                table: "AnalysisResult");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "AnalysisResult",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "MeasuredValue",
                table: "AnalysisResult",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisResult_QuestionnaireHistory_QuestionnaireHistoryId",
                table: "AnalysisResult",
                column: "QuestionnaireHistoryId",
                principalTable: "QuestionnaireHistory",
                principalColumn: "Id");
        }
    }
}
