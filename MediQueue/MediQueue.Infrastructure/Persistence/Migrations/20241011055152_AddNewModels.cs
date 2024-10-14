using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNewModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lekarstvo_CategoryLekarstvo_CategoryLekarstvoId",
                table: "Lekarstvo");

            migrationBuilder.DropForeignKey(
                name: "FK_Lekarstvo_Sclad_ScladId",
                table: "Lekarstvo");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentService_Service_ServiceId",
                table: "PaymentService");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "PaymentService",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Conclusion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HealthStatus = table.Column<int>(type: "int", nullable: false),
                    IsFullyRecovered = table.Column<bool>(type: "bit", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    QuestionnaireHistoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conclusion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conclusion_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Conclusion_QuestionnaireHistory_QuestionnaireHistoryId",
                        column: x => x.QuestionnaireHistoryId,
                        principalTable: "QuestionnaireHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Conclusion_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PaymentLekarstvo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OutstandingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentType = table.Column<int>(type: "int", nullable: true),
                    PaymentStatus = table.Column<int>(type: "int", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    LekarstvoId = table.Column<int>(type: "int", nullable: true),
                    QuestionnaireHistoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentLekarstvo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentLekarstvo_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PaymentLekarstvo_Lekarstvo_LekarstvoId",
                        column: x => x.LekarstvoId,
                        principalTable: "Lekarstvo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PaymentLekarstvo_QuestionnaireHistory_QuestionnaireHistoryId",
                        column: x => x.QuestionnaireHistoryId,
                        principalTable: "QuestionnaireHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

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
                name: "IX_PaymentService_AccountId",
                table: "PaymentService",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Conclusion_AccountId",
                table: "Conclusion",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Conclusion_QuestionnaireHistoryId",
                table: "Conclusion",
                column: "QuestionnaireHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Conclusion_ServiceId",
                table: "Conclusion",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ConclusionLekarstva_LekarstvaUsedByDoctorId",
                table: "ConclusionLekarstva",
                column: "LekarstvaUsedByDoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLekarstvo_AccountId",
                table: "PaymentLekarstvo",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLekarstvo_LekarstvoId",
                table: "PaymentLekarstvo",
                column: "LekarstvoId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLekarstvo_QuestionnaireHistoryId",
                table: "PaymentLekarstvo",
                column: "QuestionnaireHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lekarstvo_CategoryLekarstvo_CategoryLekarstvoId",
                table: "Lekarstvo",
                column: "CategoryLekarstvoId",
                principalTable: "CategoryLekarstvo",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Lekarstvo_Sclad_ScladId",
                table: "Lekarstvo",
                column: "ScladId",
                principalTable: "Sclad",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentService_Account_AccountId",
                table: "PaymentService",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentService_Service_ServiceId",
                table: "PaymentService",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lekarstvo_CategoryLekarstvo_CategoryLekarstvoId",
                table: "Lekarstvo");

            migrationBuilder.DropForeignKey(
                name: "FK_Lekarstvo_Sclad_ScladId",
                table: "Lekarstvo");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentService_Account_AccountId",
                table: "PaymentService");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentService_Service_ServiceId",
                table: "PaymentService");

            migrationBuilder.DropTable(
                name: "ConclusionLekarstva");

            migrationBuilder.DropTable(
                name: "PaymentLekarstvo");

            migrationBuilder.DropTable(
                name: "Conclusion");

            migrationBuilder.DropIndex(
                name: "IX_PaymentService_AccountId",
                table: "PaymentService");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "PaymentService");

            migrationBuilder.AddForeignKey(
                name: "FK_Lekarstvo_CategoryLekarstvo_CategoryLekarstvoId",
                table: "Lekarstvo",
                column: "CategoryLekarstvoId",
                principalTable: "CategoryLekarstvo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lekarstvo_Sclad_ScladId",
                table: "Lekarstvo",
                column: "ScladId",
                principalTable: "Sclad",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentService_Service_ServiceId",
                table: "PaymentService",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id");
        }
    }
}
