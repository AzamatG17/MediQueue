using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConectStatianaryStayUsagePaymentService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WardPlace_StationaryStay_StationaryStayId",
                table: "WardPlace");

            migrationBuilder.DropTable(
                name: "PaymentLekarstvo");

            migrationBuilder.DropTable(
                name: "StationaryStay");

            migrationBuilder.AddColumn<int>(
                name: "StationaryStayUsageId",
                table: "PaymentService",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StationaryStayUsage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    NumberOfDays = table.Column<int>(type: "int", nullable: true),
                    TariffId = table.Column<int>(type: "int", nullable: true),
                    WardPlaceId = table.Column<int>(type: "int", nullable: true),
                    NutritionId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    QuantityUsed = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsPayed = table.Column<bool>(type: "bit", nullable: true),
                    QuestionnaireHistoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationaryStayUsage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StationaryStayUsage_Nutritions_NutritionId",
                        column: x => x.NutritionId,
                        principalTable: "Nutritions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_StationaryStayUsage_QuestionnaireHistory_QuestionnaireHistoryId",
                        column: x => x.QuestionnaireHistoryId,
                        principalTable: "QuestionnaireHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_StationaryStayUsage_Tariff_TariffId",
                        column: x => x.TariffId,
                        principalTable: "Tariff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentService_StationaryStayUsageId",
                table: "PaymentService",
                column: "StationaryStayUsageId");

            migrationBuilder.CreateIndex(
                name: "IX_StationaryStayUsage_NutritionId",
                table: "StationaryStayUsage",
                column: "NutritionId");

            migrationBuilder.CreateIndex(
                name: "IX_StationaryStayUsage_QuestionnaireHistoryId",
                table: "StationaryStayUsage",
                column: "QuestionnaireHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StationaryStayUsage_TariffId",
                table: "StationaryStayUsage",
                column: "TariffId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentService_StationaryStayUsage_StationaryStayUsageId",
                table: "PaymentService",
                column: "StationaryStayUsageId",
                principalTable: "StationaryStayUsage",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_WardPlace_StationaryStayUsage_StationaryStayId",
                table: "WardPlace",
                column: "StationaryStayId",
                principalTable: "StationaryStayUsage",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentService_StationaryStayUsage_StationaryStayUsageId",
                table: "PaymentService");

            migrationBuilder.DropForeignKey(
                name: "FK_WardPlace_StationaryStayUsage_StationaryStayId",
                table: "WardPlace");

            migrationBuilder.DropTable(
                name: "StationaryStayUsage");

            migrationBuilder.DropIndex(
                name: "IX_PaymentService_StationaryStayUsageId",
                table: "PaymentService");

            migrationBuilder.DropColumn(
                name: "StationaryStayUsageId",
                table: "PaymentService");

            migrationBuilder.CreateTable(
                name: "PaymentLekarstvo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    LekarstvoId = table.Column<int>(type: "int", nullable: true),
                    QuestionnaireHistoryId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    OutstandingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentStatus = table.Column<int>(type: "int", nullable: true),
                    PaymentType = table.Column<int>(type: "int", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
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
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StationaryStay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NutritionId = table.Column<int>(type: "int", nullable: true),
                    QuestionnaireHistoryId = table.Column<int>(type: "int", nullable: true),
                    TariffId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfDays = table.Column<int>(type: "int", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WardPlaceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationaryStay", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StationaryStay_Nutritions_NutritionId",
                        column: x => x.NutritionId,
                        principalTable: "Nutritions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_StationaryStay_QuestionnaireHistory_QuestionnaireHistoryId",
                        column: x => x.QuestionnaireHistoryId,
                        principalTable: "QuestionnaireHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_StationaryStay_Tariff_TariffId",
                        column: x => x.TariffId,
                        principalTable: "Tariff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_StationaryStay_NutritionId",
                table: "StationaryStay",
                column: "NutritionId");

            migrationBuilder.CreateIndex(
                name: "IX_StationaryStay_QuestionnaireHistoryId",
                table: "StationaryStay",
                column: "QuestionnaireHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StationaryStay_TariffId",
                table: "StationaryStay",
                column: "TariffId");

            migrationBuilder.AddForeignKey(
                name: "FK_WardPlace_StationaryStay_StationaryStayId",
                table: "WardPlace",
                column: "StationaryStayId",
                principalTable: "StationaryStay",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
