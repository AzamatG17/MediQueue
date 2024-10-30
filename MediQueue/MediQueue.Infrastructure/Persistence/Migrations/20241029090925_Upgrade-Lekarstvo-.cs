using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeLekarstvo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lekarstvo_Sclad_ScladId",
                table: "Lekarstvo");

            migrationBuilder.DropColumn(
                name: "BeforeDate",
                table: "Lekarstvo");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "Lekarstvo");

            migrationBuilder.DropColumn(
                name: "MeasurementUnit",
                table: "Lekarstvo");

            migrationBuilder.DropColumn(
                name: "PriceQuantity",
                table: "Lekarstvo");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "Lekarstvo");

            migrationBuilder.DropColumn(
                name: "SalePrice",
                table: "Lekarstvo");

            migrationBuilder.DropColumn(
                name: "TotalQuantity",
                table: "Lekarstvo");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Lekarstvo",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateTable(
                name: "Partiya",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    BeforeDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    TotalQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PriceQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PhotoBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeasurementUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LekarstvoId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partiya", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Partiya_Lekarstvo_LekarstvoId",
                        column: x => x.LekarstvoId,
                        principalTable: "Lekarstvo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ScladLekarstvo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ScladId = table.Column<int>(type: "int", nullable: true),
                    PartiyaId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScladLekarstvo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScladLekarstvo_Partiya_PartiyaId",
                        column: x => x.PartiyaId,
                        principalTable: "Partiya",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScladLekarstvo_Sclad_ScladId",
                        column: x => x.ScladId,
                        principalTable: "Sclad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Partiya_LekarstvoId",
                table: "Partiya",
                column: "LekarstvoId");

            migrationBuilder.CreateIndex(
                name: "IX_ScladLekarstvo_PartiyaId",
                table: "ScladLekarstvo",
                column: "PartiyaId");

            migrationBuilder.CreateIndex(
                name: "IX_ScladLekarstvo_ScladId",
                table: "ScladLekarstvo",
                column: "ScladId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lekarstvo_Sclad_ScladId",
                table: "Lekarstvo",
                column: "ScladId",
                principalTable: "Sclad",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lekarstvo_Sclad_ScladId",
                table: "Lekarstvo");

            migrationBuilder.DropTable(
                name: "ScladLekarstvo");

            migrationBuilder.DropTable(
                name: "Partiya");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Lekarstvo",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<DateTime>(
                name: "BeforeDate",
                table: "Lekarstvo",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "Lekarstvo",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeasurementUnit",
                table: "Lekarstvo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceQuantity",
                table: "Lekarstvo",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                table: "Lekarstvo",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SalePrice",
                table: "Lekarstvo",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalQuantity",
                table: "Lekarstvo",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lekarstvo_Sclad_ScladId",
                table: "Lekarstvo",
                column: "ScladId",
                principalTable: "Sclad",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
