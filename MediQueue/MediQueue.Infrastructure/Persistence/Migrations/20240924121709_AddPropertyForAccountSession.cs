using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyForAccountSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "AccountSession",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "AccountSession");
        }
    }
}
