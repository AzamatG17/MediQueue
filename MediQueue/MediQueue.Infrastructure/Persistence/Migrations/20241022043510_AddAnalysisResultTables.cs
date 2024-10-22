using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAnalysisResultTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conclusion_Service_ServiceId",
                table: "Conclusion");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionnaireHistory_Service_ServiceId",
                table: "QuestionnaireHistory");

            migrationBuilder.DropIndex(
                name: "IX_QuestionnaireHistory_ServiceId",
                table: "QuestionnaireHistory");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "QuestionnaireHistory");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "Conclusion",
                newName: "ServiceUsageId");

            migrationBuilder.RenameIndex(
                name: "IX_Conclusion_ServiceId",
                table: "Conclusion",
                newName: "IX_Conclusion_ServiceUsageId");

            migrationBuilder.CreateTable(
                name: "AnalysisResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasuredValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    PhotoBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ResultDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServiceUsageId = table.Column<int>(type: "int", nullable: false),
                    QuestionnaireHistoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalysisResult_QuestionnaireHistory_QuestionnaireHistoryId",
                        column: x => x.QuestionnaireHistoryId,
                        principalTable: "QuestionnaireHistory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnalysisResult_ServiceUsage_ServiceUsageId",
                        column: x => x.ServiceUsageId,
                        principalTable: "ServiceUsage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisResult_QuestionnaireHistoryId",
                table: "AnalysisResult",
                column: "QuestionnaireHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisResult_ServiceUsageId",
                table: "AnalysisResult",
                column: "ServiceUsageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conclusion_ServiceUsage_ServiceUsageId",
                table: "Conclusion",
                column: "ServiceUsageId",
                principalTable: "ServiceUsage",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conclusion_ServiceUsage_ServiceUsageId",
                table: "Conclusion");

            migrationBuilder.DropTable(
                name: "AnalysisResult");

            migrationBuilder.RenameColumn(
                name: "ServiceUsageId",
                table: "Conclusion",
                newName: "ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Conclusion_ServiceUsageId",
                table: "Conclusion",
                newName: "IX_Conclusion_ServiceId");

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "QuestionnaireHistory",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireHistory_ServiceId",
                table: "QuestionnaireHistory",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conclusion_Service_ServiceId",
                table: "Conclusion",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionnaireHistory_Service_ServiceId",
                table: "QuestionnaireHistory",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id");
        }
    }
}
