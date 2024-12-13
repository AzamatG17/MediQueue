using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDataAuditLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Operation",
                table: "AuditLog",
                newName: "Action");

            migrationBuilder.RenameColumn(
                name: "OldValues",
                table: "AuditLog",
                newName: "RecordId");

            migrationBuilder.RenameColumn(
                name: "NewValues",
                table: "AuditLog",
                newName: "Changes");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "AuditLog",
                newName: "Timestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "AuditLog",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "RecordId",
                table: "AuditLog",
                newName: "OldValues");

            migrationBuilder.RenameColumn(
                name: "Changes",
                table: "AuditLog",
                newName: "NewValues");

            migrationBuilder.RenameColumn(
                name: "Action",
                table: "AuditLog",
                newName: "Operation");
        }
    }
}
