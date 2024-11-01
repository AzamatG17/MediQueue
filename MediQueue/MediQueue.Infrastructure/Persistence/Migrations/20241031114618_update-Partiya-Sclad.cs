using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatePartiyaSclad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partiya_ScladLekarstvo_ScladLekarstvoId",
                table: "Partiya");

            migrationBuilder.DropTable(
                name: "ScladLekarstvo");

            migrationBuilder.RenameColumn(
                name: "ScladLekarstvoId",
                table: "Partiya",
                newName: "ScladId");

            migrationBuilder.RenameIndex(
                name: "IX_Partiya_ScladLekarstvoId",
                table: "Partiya",
                newName: "IX_Partiya_ScladId");

            migrationBuilder.AddForeignKey(
                name: "FK_Partiya_Sclad_ScladId",
                table: "Partiya",
                column: "ScladId",
                principalTable: "Sclad",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partiya_Sclad_ScladId",
                table: "Partiya");

            migrationBuilder.RenameColumn(
                name: "ScladId",
                table: "Partiya",
                newName: "ScladLekarstvoId");

            migrationBuilder.RenameIndex(
                name: "IX_Partiya_ScladId",
                table: "Partiya",
                newName: "IX_Partiya_ScladLekarstvoId");

            migrationBuilder.CreateTable(
                name: "ScladLekarstvo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScladId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScladLekarstvo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScladLekarstvo_Sclad_ScladId",
                        column: x => x.ScladId,
                        principalTable: "Sclad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScladLekarstvo_ScladId",
                table: "ScladLekarstvo",
                column: "ScladId");

            migrationBuilder.AddForeignKey(
                name: "FK_Partiya_ScladLekarstvo_ScladLekarstvoId",
                table: "Partiya",
                column: "ScladLekarstvoId",
                principalTable: "ScladLekarstvo",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
