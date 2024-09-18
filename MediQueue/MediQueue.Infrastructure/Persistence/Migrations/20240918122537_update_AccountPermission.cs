using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class update_AccountPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Role_RoleId",
                table: "RolePermission");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "RolePermission",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermission_RoleId",
                table: "RolePermission",
                newName: "IX_RolePermission_AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Account_AccountId",
                table: "RolePermission",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Account_AccountId",
                table: "RolePermission");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "RolePermission",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermission_AccountId",
                table: "RolePermission",
                newName: "IX_RolePermission_RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Role_RoleId",
                table: "RolePermission",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
