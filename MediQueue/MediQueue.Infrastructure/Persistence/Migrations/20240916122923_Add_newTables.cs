using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_newTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoBase64",
                table: "Questionnaire",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Addres = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryLekarstvo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryLekarstvo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sclad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Branchid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sclad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sclad_Branch_Branchid",
                        column: x => x.Branchid,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lekarstvo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    BeforeDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PhotoBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeasurementUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryLekarstvoId = table.Column<int>(type: "int", nullable: true),
                    ScladId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lekarstvo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lekarstvo_CategoryLekarstvo_CategoryLekarstvoId",
                        column: x => x.CategoryLekarstvoId,
                        principalTable: "CategoryLekarstvo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Lekarstvo_Sclad_ScladId",
                        column: x => x.ScladId,
                        principalTable: "Sclad",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lekarstvo_CategoryLekarstvoId",
                table: "Lekarstvo",
                column: "CategoryLekarstvoId");

            migrationBuilder.CreateIndex(
                name: "IX_Lekarstvo_ScladId",
                table: "Lekarstvo",
                column: "ScladId");

            migrationBuilder.CreateIndex(
                name: "IX_Sclad_Branchid",
                table: "Sclad",
                column: "Branchid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lekarstvo");

            migrationBuilder.DropTable(
                name: "CategoryLekarstvo");

            migrationBuilder.DropTable(
                name: "Sclad");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropColumn(
                name: "PhotoBase64",
                table: "Questionnaire");
        }
    }
}
