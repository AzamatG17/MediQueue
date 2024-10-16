using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaymentServiceForOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentLekarstvo_QuestionnaireHistory_QuestionnaireHistoryId",
                table: "PaymentLekarstvo");

            migrationBuilder.DropTable(
                name: "QuestionnaireHistoryService");

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "QuestionnaireHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LekarstvoId",
                table: "PaymentService",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicationType",
                table: "PaymentService",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPayed",
                table: "LekarstvoUsage",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestionnaireHistoryId",
                table: "LekarstvoUsage",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ServiceUsage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    QuantityUsed = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsPayed = table.Column<bool>(type: "bit", nullable: true),
                    QuestionnaireHistoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceUsage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceUsage_QuestionnaireHistory_QuestionnaireHistoryId",
                        column: x => x.QuestionnaireHistoryId,
                        principalTable: "QuestionnaireHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ServiceUsage_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireHistory_ServiceId",
                table: "QuestionnaireHistory",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentService_LekarstvoId",
                table: "PaymentService",
                column: "LekarstvoId");

            migrationBuilder.CreateIndex(
                name: "IX_LekarstvoUsage_QuestionnaireHistoryId",
                table: "LekarstvoUsage",
                column: "QuestionnaireHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceUsage_QuestionnaireHistoryId",
                table: "ServiceUsage",
                column: "QuestionnaireHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceUsage_ServiceId",
                table: "ServiceUsage",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_LekarstvoUsage_QuestionnaireHistory_QuestionnaireHistoryId",
                table: "LekarstvoUsage",
                column: "QuestionnaireHistoryId",
                principalTable: "QuestionnaireHistory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentLekarstvo_QuestionnaireHistory_QuestionnaireHistoryId",
                table: "PaymentLekarstvo",
                column: "QuestionnaireHistoryId",
                principalTable: "QuestionnaireHistory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentService_Lekarstvo_LekarstvoId",
                table: "PaymentService",
                column: "LekarstvoId",
                principalTable: "Lekarstvo",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionnaireHistory_Service_ServiceId",
                table: "QuestionnaireHistory",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LekarstvoUsage_QuestionnaireHistory_QuestionnaireHistoryId",
                table: "LekarstvoUsage");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentLekarstvo_QuestionnaireHistory_QuestionnaireHistoryId",
                table: "PaymentLekarstvo");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentService_Lekarstvo_LekarstvoId",
                table: "PaymentService");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionnaireHistory_Service_ServiceId",
                table: "QuestionnaireHistory");

            migrationBuilder.DropTable(
                name: "ServiceUsage");

            migrationBuilder.DropIndex(
                name: "IX_QuestionnaireHistory_ServiceId",
                table: "QuestionnaireHistory");

            migrationBuilder.DropIndex(
                name: "IX_PaymentService_LekarstvoId",
                table: "PaymentService");

            migrationBuilder.DropIndex(
                name: "IX_LekarstvoUsage_QuestionnaireHistoryId",
                table: "LekarstvoUsage");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "QuestionnaireHistory");

            migrationBuilder.DropColumn(
                name: "LekarstvoId",
                table: "PaymentService");

            migrationBuilder.DropColumn(
                name: "MedicationType",
                table: "PaymentService");

            migrationBuilder.DropColumn(
                name: "IsPayed",
                table: "LekarstvoUsage");

            migrationBuilder.DropColumn(
                name: "QuestionnaireHistoryId",
                table: "LekarstvoUsage");

            migrationBuilder.CreateTable(
                name: "QuestionnaireHistoryService",
                columns: table => new
                {
                    QuestionnaireHistoriesId = table.Column<int>(type: "int", nullable: false),
                    ServicesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionnaireHistoryService", x => new { x.QuestionnaireHistoriesId, x.ServicesId });
                    table.ForeignKey(
                        name: "FK_QuestionnaireHistoryService_QuestionnaireHistory_QuestionnaireHistoriesId",
                        column: x => x.QuestionnaireHistoriesId,
                        principalTable: "QuestionnaireHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionnaireHistoryService_Service_ServicesId",
                        column: x => x.ServicesId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireHistoryService_ServicesId",
                table: "QuestionnaireHistoryService",
                column: "ServicesId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentLekarstvo_QuestionnaireHistory_QuestionnaireHistoryId",
                table: "PaymentLekarstvo",
                column: "QuestionnaireHistoryId",
                principalTable: "QuestionnaireHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
