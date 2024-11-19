using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeServiceUsageAccountId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceUsage_Account_AccountID",
                table: "ServiceUsage");

            migrationBuilder.RenameColumn(
                name: "AccountID",
                table: "ServiceUsage",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceUsage_AccountID",
                table: "ServiceUsage",
                newName: "IX_ServiceUsage_AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceUsage_Account_AccountId",
                table: "ServiceUsage",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceUsage_Account_AccountId",
                table: "ServiceUsage");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "ServiceUsage",
                newName: "AccountID");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceUsage_AccountId",
                table: "ServiceUsage",
                newName: "IX_ServiceUsage_AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceUsage_Account_AccountID",
                table: "ServiceUsage",
                column: "AccountID",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
