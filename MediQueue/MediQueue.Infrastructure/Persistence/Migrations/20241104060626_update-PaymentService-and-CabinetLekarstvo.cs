using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatePaymentServiceandCabinetLekarstvo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentService_Lekarstvo_LekarstvoId",
                table: "PaymentService");

            migrationBuilder.RenameColumn(
                name: "LekarstvoId",
                table: "PaymentService",
                newName: "DoctorCabinetLekarstvoId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentService_LekarstvoId",
                table: "PaymentService",
                newName: "IX_PaymentService_DoctorCabinetLekarstvoId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentService_DoctorCabinetLekarstvo_DoctorCabinetLekarstvoId",
                table: "PaymentService",
                column: "DoctorCabinetLekarstvoId",
                principalTable: "DoctorCabinetLekarstvo",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentService_DoctorCabinetLekarstvo_DoctorCabinetLekarstvoId",
                table: "PaymentService");

            migrationBuilder.RenameColumn(
                name: "DoctorCabinetLekarstvoId",
                table: "PaymentService",
                newName: "LekarstvoId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentService_DoctorCabinetLekarstvoId",
                table: "PaymentService",
                newName: "IX_PaymentService_LekarstvoId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentService_Lekarstvo_LekarstvoId",
                table: "PaymentService",
                column: "LekarstvoId",
                principalTable: "Lekarstvo",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
