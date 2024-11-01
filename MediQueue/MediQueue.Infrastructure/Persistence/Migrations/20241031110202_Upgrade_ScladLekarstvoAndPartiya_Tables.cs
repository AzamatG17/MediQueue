using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class Upgrade_ScladLekarstvoAndPartiya_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScladLekarstvo_Partiya_PartiyaId",
                table: "ScladLekarstvo");

            migrationBuilder.DropForeignKey(
                name: "FK_ScladLekarstvo_Partiya_PartiyaId1",
                table: "ScladLekarstvo");

            migrationBuilder.DropIndex(
                name: "IX_ScladLekarstvo_PartiyaId",
                table: "ScladLekarstvo");

            migrationBuilder.DropIndex(
                name: "IX_ScladLekarstvo_PartiyaId1",
                table: "ScladLekarstvo");

            migrationBuilder.DropColumn(
                name: "PartiyaId",
                table: "ScladLekarstvo");

            migrationBuilder.DropColumn(
                name: "PartiyaId1",
                table: "ScladLekarstvo");

            migrationBuilder.AddColumn<int>(
                name: "ScladLekarstvoId",
                table: "Partiya",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Partiya_ScladLekarstvoId",
                table: "Partiya",
                column: "ScladLekarstvoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Partiya_ScladLekarstvo_ScladLekarstvoId",
                table: "Partiya",
                column: "ScladLekarstvoId",
                principalTable: "ScladLekarstvo",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partiya_ScladLekarstvo_ScladLekarstvoId",
                table: "Partiya");

            migrationBuilder.DropIndex(
                name: "IX_Partiya_ScladLekarstvoId",
                table: "Partiya");

            migrationBuilder.DropColumn(
                name: "ScladLekarstvoId",
                table: "Partiya");

            migrationBuilder.AddColumn<int>(
                name: "PartiyaId",
                table: "ScladLekarstvo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PartiyaId1",
                table: "ScladLekarstvo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScladLekarstvo_PartiyaId",
                table: "ScladLekarstvo",
                column: "PartiyaId");

            migrationBuilder.CreateIndex(
                name: "IX_ScladLekarstvo_PartiyaId1",
                table: "ScladLekarstvo",
                column: "PartiyaId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ScladLekarstvo_Partiya_PartiyaId",
                table: "ScladLekarstvo",
                column: "PartiyaId",
                principalTable: "Partiya",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScladLekarstvo_Partiya_PartiyaId1",
                table: "ScladLekarstvo",
                column: "PartiyaId1",
                principalTable: "Partiya",
                principalColumn: "Id");
        }
    }
}
