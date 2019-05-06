using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LOBCore.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommUserGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommUserGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaveFormTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveFormTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemEnvironment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AppName = table.Column<string>(nullable: true),
                    AndroidVersion = table.Column<string>(nullable: true),
                    AndroidUrl = table.Column<string>(nullable: true),
                    iOSVersion = table.Column<string>(nullable: true),
                    iOSUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemEnvironment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommUserGroupItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CommUserGroupId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommUserGroupItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommUserGroupItems_CommUserGroups_CommUserGroupId",
                        column: x => x.CommUserGroupId,
                        principalTable: "CommUserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LobUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Account = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    TokenVersion = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    DepartmentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LobUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LobUsers_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExceptionRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(nullable: true),
                    DeviceName = table.Column<string>(nullable: true),
                    DeviceModel = table.Column<string>(nullable: true),
                    OSType = table.Column<string>(nullable: true),
                    OSVersion = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    CallStack = table.Column<string>(nullable: true),
                    ExceptionTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExceptionRecords_LobUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "LobUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvoiceNo = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Memo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_LobUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "LobUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaveForms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(nullable: true),
                    BeginTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    TotalHours = table.Column<int>(nullable: false),
                    LeaveFormTypeId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveForms_LeaveFormTypes_LeaveFormTypeId",
                        column: x => x.LeaveFormTypeId,
                        principalTable: "LeaveFormTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeaveForms_LobUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "LobUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTokens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Token = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    OSType = table.Column<int>(nullable: false),
                    RegistrationTime = table.Column<DateTime>(nullable: false),
                    Invalid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationTokens_LobUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "LobUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Suggestions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(nullable: true),
                    SubmitTime = table.Column<DateTime>(nullable: false),
                    Subject = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suggestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suggestions_LobUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "LobUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvoiceId = table.Column<int>(nullable: true),
                    TDate = table.Column<DateTime>(nullable: false),
                    Cnt = table.Column<string>(nullable: true),
                    Qty = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    PictureName = table.Column<string>(nullable: true),
                    Flag = table.Column<bool>(nullable: false),
                    Memo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceDetails_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommUserGroupItems_CommUserGroupId",
                table: "CommUserGroupItems",
                column: "CommUserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionRecords_UserId",
                table: "ExceptionRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_InvoiceId",
                table: "InvoiceDetails",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_UserId",
                table: "Invoices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveForms_LeaveFormTypeId",
                table: "LeaveForms",
                column: "LeaveFormTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveForms_UserId",
                table: "LeaveForms",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LobUsers_DepartmentId",
                table: "LobUsers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTokens_UserId",
                table: "NotificationTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Suggestions_UserId",
                table: "Suggestions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommUserGroupItems");

            migrationBuilder.DropTable(
                name: "ExceptionRecords");

            migrationBuilder.DropTable(
                name: "InvoiceDetails");

            migrationBuilder.DropTable(
                name: "LeaveForms");

            migrationBuilder.DropTable(
                name: "NotificationTokens");

            migrationBuilder.DropTable(
                name: "Suggestions");

            migrationBuilder.DropTable(
                name: "SystemEnvironment");

            migrationBuilder.DropTable(
                name: "CommUserGroups");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "LeaveFormTypes");

            migrationBuilder.DropTable(
                name: "LobUsers");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
