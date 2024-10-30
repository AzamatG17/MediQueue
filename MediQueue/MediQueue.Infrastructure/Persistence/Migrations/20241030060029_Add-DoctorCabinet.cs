using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorCabinet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LekarstvoUsage_Conclusion_ConclusionId",
                table: "LekarstvoUsage");

            migrationBuilder.DropForeignKey(
                name: "FK_LekarstvoUsage_Lekarstvo_LekarstvoId",
                table: "LekarstvoUsage");

            migrationBuilder.RenameColumn(
                name: "LekarstvoId",
                table: "LekarstvoUsage",
                newName: "DoctorCabinetLekarstvoId");

            migrationBuilder.RenameIndex(
                name: "IX_LekarstvoUsage_LekarstvoId",
                table: "LekarstvoUsage",
                newName: "IX_LekarstvoUsage_DoctorCabinetLekarstvoId");

            migrationBuilder.AddColumn<int>(
                name: "PartiyaId1",
                table: "ScladLekarstvo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DoctorCabinet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorCabinet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorCabinet_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DoctorCabinetLekarstvo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DoctorCabinetId = table.Column<int>(type: "int", nullable: false),
                    PartiyaId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorCabinetLekarstvo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorCabinetLekarstvo_DoctorCabinet_DoctorCabinetId",
                        column: x => x.DoctorCabinetId,
                        principalTable: "DoctorCabinet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorCabinetLekarstvo_Partiya_PartiyaId",
                        column: x => x.PartiyaId,
                        principalTable: "Partiya",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScladLekarstvo_PartiyaId1",
                table: "ScladLekarstvo",
                column: "PartiyaId1");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorCabinet_AccountId",
                table: "DoctorCabinet",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorCabinetLekarstvo_DoctorCabinetId",
                table: "DoctorCabinetLekarstvo",
                column: "DoctorCabinetId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorCabinetLekarstvo_PartiyaId",
                table: "DoctorCabinetLekarstvo",
                column: "PartiyaId");

            migrationBuilder.AddForeignKey(
                name: "FK_LekarstvoUsage_Conclusion_ConclusionId",
                table: "LekarstvoUsage",
                column: "ConclusionId",
                principalTable: "Conclusion",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_LekarstvoUsage_DoctorCabinetLekarstvo_DoctorCabinetLekarstvoId",
                table: "LekarstvoUsage",
                column: "DoctorCabinetLekarstvoId",
                principalTable: "DoctorCabinetLekarstvo",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ScladLekarstvo_Partiya_PartiyaId1",
                table: "ScladLekarstvo",
                column: "PartiyaId1",
                principalTable: "Partiya",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LekarstvoUsage_Conclusion_ConclusionId",
                table: "LekarstvoUsage");

            migrationBuilder.DropForeignKey(
                name: "FK_LekarstvoUsage_DoctorCabinetLekarstvo_DoctorCabinetLekarstvoId",
                table: "LekarstvoUsage");

            migrationBuilder.DropForeignKey(
                name: "FK_ScladLekarstvo_Partiya_PartiyaId1",
                table: "ScladLekarstvo");

            migrationBuilder.DropTable(
                name: "DoctorCabinetLekarstvo");

            migrationBuilder.DropTable(
                name: "DoctorCabinet");

            migrationBuilder.DropIndex(
                name: "IX_ScladLekarstvo_PartiyaId1",
                table: "ScladLekarstvo");

            migrationBuilder.DropColumn(
                name: "PartiyaId1",
                table: "ScladLekarstvo");

            migrationBuilder.RenameColumn(
                name: "DoctorCabinetLekarstvoId",
                table: "LekarstvoUsage",
                newName: "LekarstvoId");

            migrationBuilder.RenameIndex(
                name: "IX_LekarstvoUsage_DoctorCabinetLekarstvoId",
                table: "LekarstvoUsage",
                newName: "IX_LekarstvoUsage_LekarstvoId");

            migrationBuilder.AddForeignKey(
                name: "FK_LekarstvoUsage_Conclusion_ConclusionId",
                table: "LekarstvoUsage",
                column: "ConclusionId",
                principalTable: "Conclusion",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LekarstvoUsage_Lekarstvo_LekarstvoId",
                table: "LekarstvoUsage",
                column: "LekarstvoId",
                principalTable: "Lekarstvo",
                principalColumn: "Id");
        }
    }
}
