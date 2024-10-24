using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediQueue.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class initial_create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountSession",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    LastActivitytime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsLoggedOut = table.Column<bool>(type: "bit", nullable: false),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountSession", x => x.Id);
                });

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
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
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
                name: "Controllers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ControllerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Controllers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questionnaire",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionnaireId = table.Column<int>(type: "int", maxLength: 255, nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassportSeria = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PassportPinfl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SurName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DateIssue = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateBefore = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    District = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Posolos = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Bithdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SocialSattus = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AdvertisingChannel = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhotoBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questionnaire", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
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
                name: "Service",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Service_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryGroup",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    GroupsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryGroup", x => new { x.CategoriesId, x.GroupsId });
                    table.ForeignKey(
                        name: "FK_CategoryGroup_Category_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryGroup_Group_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Passport = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SurName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhotoBase64 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bithdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
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
                    TotalQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PriceQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Lekarstvo_Sclad_ScladId",
                        column: x => x.ScladId,
                        principalTable: "Sclad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "QuestionnaireHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Historyid = table.Column<int>(type: "int", nullable: false),
                    HistoryDiscription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsPayed = table.Column<bool>(type: "bit", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    QuestionnaireId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionnaireHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionnaireHistory_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_QuestionnaireHistory_Questionnaire_QuestionnaireId",
                        column: x => x.QuestionnaireId,
                        principalTable: "Questionnaire",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ControllerId = table.Column<int>(type: "int", nullable: false),
                    Permissions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermission_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentLekarstvo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OutstandingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentType = table.Column<int>(type: "int", nullable: true),
                    PaymentStatus = table.Column<int>(type: "int", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    LekarstvoId = table.Column<int>(type: "int", nullable: true),
                    QuestionnaireHistoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentLekarstvo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentLekarstvo_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PaymentLekarstvo_Lekarstvo_LekarstvoId",
                        column: x => x.LekarstvoId,
                        principalTable: "Lekarstvo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PaymentLekarstvo_QuestionnaireHistory_QuestionnaireHistoryId",
                        column: x => x.QuestionnaireHistoryId,
                        principalTable: "QuestionnaireHistory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PaymentService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OutstandingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicationType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    LekarstvoId = table.Column<int>(type: "int", nullable: true),
                    QuestionnaireHistoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentService_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PaymentService_Lekarstvo_LekarstvoId",
                        column: x => x.LekarstvoId,
                        principalTable: "Lekarstvo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PaymentService_QuestionnaireHistory_QuestionnaireHistoryId",
                        column: x => x.QuestionnaireHistoryId,
                        principalTable: "QuestionnaireHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PaymentService_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ServiceUsage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    QuantityUsed = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsPayed = table.Column<bool>(type: "bit", nullable: true),
                    QuestionnaireHistoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceUsage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceUsage_QuestionnaireHistory_QuestionnaireHistoryId",
                        column: x => x.QuestionnaireHistoryId,
                        principalTable: "QuestionnaireHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ServiceUsage_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AnalysisResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasuredValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    PhotoBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ResultDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    ServiceUsageId = table.Column<int>(type: "int", nullable: false),
                    QuestionnaireHistoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalysisResult_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnalysisResult_QuestionnaireHistory_QuestionnaireHistoryId",
                        column: x => x.QuestionnaireHistoryId,
                        principalTable: "QuestionnaireHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AnalysisResult_ServiceUsage_ServiceUsageId",
                        column: x => x.ServiceUsageId,
                        principalTable: "ServiceUsage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conclusion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HealthStatus = table.Column<int>(type: "int", nullable: false),
                    IsFullyRecovered = table.Column<bool>(type: "bit", nullable: false),
                    ServiceUsageId = table.Column<int>(type: "int", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    QuestionnaireHistoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conclusion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conclusion_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Conclusion_QuestionnaireHistory_QuestionnaireHistoryId",
                        column: x => x.QuestionnaireHistoryId,
                        principalTable: "QuestionnaireHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Conclusion_ServiceUsage_ServiceUsageId",
                        column: x => x.ServiceUsageId,
                        principalTable: "ServiceUsage",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LekarstvoUsage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConclusionId = table.Column<int>(type: "int", nullable: true),
                    LekarstvoId = table.Column<int>(type: "int", nullable: true),
                    QuantityUsed = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsPayed = table.Column<bool>(type: "bit", nullable: true),
                    QuestionnaireHistoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LekarstvoUsage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LekarstvoUsage_Conclusion_ConclusionId",
                        column: x => x.ConclusionId,
                        principalTable: "Conclusion",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LekarstvoUsage_Lekarstvo_LekarstvoId",
                        column: x => x.LekarstvoId,
                        principalTable: "Lekarstvo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LekarstvoUsage_QuestionnaireHistory_QuestionnaireHistoryId",
                        column: x => x.QuestionnaireHistoryId,
                        principalTable: "QuestionnaireHistory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_RoleId",
                table: "Account",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisResult_AccountId",
                table: "AnalysisResult",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisResult_QuestionnaireHistoryId",
                table: "AnalysisResult",
                column: "QuestionnaireHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisResult_ServiceUsageId",
                table: "AnalysisResult",
                column: "ServiceUsageId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryGroup_GroupsId",
                table: "CategoryGroup",
                column: "GroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_Conclusion_AccountId",
                table: "Conclusion",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Conclusion_QuestionnaireHistoryId",
                table: "Conclusion",
                column: "QuestionnaireHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Conclusion_ServiceUsageId",
                table: "Conclusion",
                column: "ServiceUsageId");

            migrationBuilder.CreateIndex(
                name: "IX_Lekarstvo_CategoryLekarstvoId",
                table: "Lekarstvo",
                column: "CategoryLekarstvoId");

            migrationBuilder.CreateIndex(
                name: "IX_Lekarstvo_ScladId",
                table: "Lekarstvo",
                column: "ScladId");

            migrationBuilder.CreateIndex(
                name: "IX_LekarstvoUsage_ConclusionId",
                table: "LekarstvoUsage",
                column: "ConclusionId");

            migrationBuilder.CreateIndex(
                name: "IX_LekarstvoUsage_LekarstvoId",
                table: "LekarstvoUsage",
                column: "LekarstvoId");

            migrationBuilder.CreateIndex(
                name: "IX_LekarstvoUsage_QuestionnaireHistoryId",
                table: "LekarstvoUsage",
                column: "QuestionnaireHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLekarstvo_AccountId",
                table: "PaymentLekarstvo",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLekarstvo_LekarstvoId",
                table: "PaymentLekarstvo",
                column: "LekarstvoId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLekarstvo_QuestionnaireHistoryId",
                table: "PaymentLekarstvo",
                column: "QuestionnaireHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentService_AccountId",
                table: "PaymentService",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentService_LekarstvoId",
                table: "PaymentService",
                column: "LekarstvoId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentService_QuestionnaireHistoryId",
                table: "PaymentService",
                column: "QuestionnaireHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentService_ServiceId",
                table: "PaymentService",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireHistory_AccountId",
                table: "QuestionnaireHistory",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireHistory_QuestionnaireId",
                table: "QuestionnaireHistory",
                column: "QuestionnaireId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_AccountId",
                table: "RolePermission",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Sclad_Branchid",
                table: "Sclad",
                column: "Branchid");

            migrationBuilder.CreateIndex(
                name: "IX_Service_CategoryId",
                table: "Service",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceUsage_QuestionnaireHistoryId",
                table: "ServiceUsage",
                column: "QuestionnaireHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceUsage_ServiceId",
                table: "ServiceUsage",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountSession");

            migrationBuilder.DropTable(
                name: "AnalysisResult");

            migrationBuilder.DropTable(
                name: "CategoryGroup");

            migrationBuilder.DropTable(
                name: "Controllers");

            migrationBuilder.DropTable(
                name: "LekarstvoUsage");

            migrationBuilder.DropTable(
                name: "PaymentLekarstvo");

            migrationBuilder.DropTable(
                name: "PaymentService");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "Conclusion");

            migrationBuilder.DropTable(
                name: "Lekarstvo");

            migrationBuilder.DropTable(
                name: "ServiceUsage");

            migrationBuilder.DropTable(
                name: "CategoryLekarstvo");

            migrationBuilder.DropTable(
                name: "Sclad");

            migrationBuilder.DropTable(
                name: "QuestionnaireHistory");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Questionnaire");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
