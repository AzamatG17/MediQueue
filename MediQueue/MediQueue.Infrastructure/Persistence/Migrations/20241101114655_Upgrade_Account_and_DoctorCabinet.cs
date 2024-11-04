using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class Upgrade_Account_and_DoctorCabinet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DoctorCabinet_AccountId",
                table: "DoctorCabinet");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "DoctorCabinetLekarstvo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DoctorCabinetId",
                table: "Account",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DoctorCabinet_AccountId",
                table: "DoctorCabinet",
                column: "AccountId",
                unique: true,
                filter: "[AccountId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DoctorCabinet_AccountId",
                table: "DoctorCabinet");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "DoctorCabinetLekarstvo");

            migrationBuilder.DropColumn(
                name: "DoctorCabinetId",
                table: "Account");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorCabinet_AccountId",
                table: "DoctorCabinet",
                column: "AccountId");
        }
    }
}
