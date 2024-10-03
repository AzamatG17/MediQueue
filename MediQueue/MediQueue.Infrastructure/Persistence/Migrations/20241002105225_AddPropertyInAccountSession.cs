using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyInAccountSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentService_QuestionnaireHistory_QuestionnaireHistoryId",
                table: "PaymentService");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionnaireHistory_Account_AccountId",
                table: "QuestionnaireHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionnaireHistory_Questionnaire_QuestionnaireId",
                table: "QuestionnaireHistory");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiry",
                table: "AccountSession",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentService_QuestionnaireHistory_QuestionnaireHistoryId",
                table: "PaymentService",
                column: "QuestionnaireHistoryId",
                principalTable: "QuestionnaireHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionnaireHistory_Account_AccountId",
                table: "QuestionnaireHistory",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionnaireHistory_Questionnaire_QuestionnaireId",
                table: "QuestionnaireHistory",
                column: "QuestionnaireId",
                principalTable: "Questionnaire",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentService_QuestionnaireHistory_QuestionnaireHistoryId",
                table: "PaymentService");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionnaireHistory_Account_AccountId",
                table: "QuestionnaireHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionnaireHistory_Questionnaire_QuestionnaireId",
                table: "QuestionnaireHistory");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiry",
                table: "AccountSession");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentService_QuestionnaireHistory_QuestionnaireHistoryId",
                table: "PaymentService",
                column: "QuestionnaireHistoryId",
                principalTable: "QuestionnaireHistory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionnaireHistory_Account_AccountId",
                table: "QuestionnaireHistory",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionnaireHistory_Questionnaire_QuestionnaireId",
                table: "QuestionnaireHistory",
                column: "QuestionnaireId",
                principalTable: "Questionnaire",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
