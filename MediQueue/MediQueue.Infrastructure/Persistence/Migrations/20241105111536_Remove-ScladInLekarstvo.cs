using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveScladInLekarstvo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lekarstvo_Sclad_ScladId",
                table: "Lekarstvo");

            migrationBuilder.DropIndex(
                name: "IX_Lekarstvo_ScladId",
                table: "Lekarstvo");

            migrationBuilder.DropColumn(
                name: "ScladId",
                table: "Lekarstvo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScladId",
                table: "Lekarstvo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lekarstvo_ScladId",
                table: "Lekarstvo",
                column: "ScladId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lekarstvo_Sclad_ScladId",
                table: "Lekarstvo",
                column: "ScladId",
                principalTable: "Sclad",
                principalColumn: "Id");
        }
    }
}
