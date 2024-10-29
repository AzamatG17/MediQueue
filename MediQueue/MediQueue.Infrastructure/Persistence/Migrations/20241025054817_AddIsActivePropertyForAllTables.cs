using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActivePropertyForAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ServiceUsage",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Service",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Sclad",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "RolePermission",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Role",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "QuestionnaireHistory",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Questionnaire",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Permission",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PaymentService",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PaymentLekarstvo",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "LekarstvoUsage",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Lekarstvo",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Group",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Controllers",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Conclusion",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CategoryLekarstvo",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Category",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Branch",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AnalysisResult",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AccountSession",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Account",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ServiceUsage");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Sclad");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "RolePermission");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "QuestionnaireHistory");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Questionnaire");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PaymentService");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PaymentLekarstvo");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "LekarstvoUsage");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Lekarstvo");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Group");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Controllers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Conclusion");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CategoryLekarstvo");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AnalysisResult");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AccountSession");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Account");
        }
    }
}
