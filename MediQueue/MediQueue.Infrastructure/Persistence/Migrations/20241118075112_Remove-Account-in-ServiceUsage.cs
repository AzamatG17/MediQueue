using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAccountinServiceUsage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceUsage_Account_AccountId",
                table: "ServiceUsage");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceUsage_Account_AccountId",
                table: "ServiceUsage",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceUsage_Account_AccountId",
                table: "ServiceUsage");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceUsage_Account_AccountId",
                table: "ServiceUsage",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
