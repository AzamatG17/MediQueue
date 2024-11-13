using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceForAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_Category_CategoryId",
                table: "Service");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "ServiceUsage",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Service",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "AccountService",
                columns: table => new
                {
                    AccountsId = table.Column<int>(type: "int", nullable: false),
                    ServicesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountService", x => new { x.AccountsId, x.ServicesId });
                    table.ForeignKey(
                        name: "FK_AccountService_Account_AccountsId",
                        column: x => x.AccountsId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountService_Service_ServicesId",
                        column: x => x.ServicesId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceUsage_AccountId",
                table: "ServiceUsage",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountService_ServicesId",
                table: "AccountService",
                column: "ServicesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Category_CategoryId",
                table: "Service",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

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
                name: "FK_Service_Category_CategoryId",
                table: "Service");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceUsage_Account_AccountId",
                table: "ServiceUsage");

            migrationBuilder.DropTable(
                name: "AccountService");

            migrationBuilder.DropIndex(
                name: "IX_ServiceUsage_AccountId",
                table: "ServiceUsage");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "ServiceUsage");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Service",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Category_CategoryId",
                table: "Service",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
