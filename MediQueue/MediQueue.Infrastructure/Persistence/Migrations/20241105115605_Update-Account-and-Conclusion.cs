using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccountandConclusion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conclusion_Account_AccountId",
                table: "Conclusion");

            migrationBuilder.AddForeignKey(
                name: "FK_Conclusion_Account_AccountId",
                table: "Conclusion",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conclusion_Account_AccountId",
                table: "Conclusion");

            migrationBuilder.AddForeignKey(
                name: "FK_Conclusion_Account_AccountId",
                table: "Conclusion",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");
        }
    }
}
