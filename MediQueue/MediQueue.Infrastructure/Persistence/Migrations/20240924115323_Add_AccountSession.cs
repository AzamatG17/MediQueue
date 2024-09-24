using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_AccountSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountSession",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    LastActivitytime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsLoggedOut = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountSession", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountSession");
        }
    }
}
