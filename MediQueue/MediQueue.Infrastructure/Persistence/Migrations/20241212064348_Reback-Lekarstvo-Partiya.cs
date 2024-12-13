using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class RebackLekarstvoPartiya : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeasurementUnit",
                table: "Partiya");

            migrationBuilder.AddColumn<string>(
                name: "MeasurementUnit",
                table: "Lekarstvo",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeasurementUnit",
                table: "Lekarstvo");

            migrationBuilder.AddColumn<string>(
                name: "MeasurementUnit",
                table: "Partiya",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
