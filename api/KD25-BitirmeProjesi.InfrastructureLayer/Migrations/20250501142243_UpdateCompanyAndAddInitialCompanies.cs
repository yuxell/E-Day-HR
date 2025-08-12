using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KD25_BitirmeProjesi.InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCompanyAndAddInitialCompanies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MersisNum = table.Column<long>(type: "bigint", nullable: false),
                    TaxNum = table.Column<long>(type: "bigint", nullable: false),
                    TaxOffice = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    NumberOfEmployees = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    FoundationYear = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    ContractStartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ContractEndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    RecordStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ExpenceTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenceTypeName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecordStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenceTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LeaveRecordTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaveRecordName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecordStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRecordTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CompanyID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecordStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Departments_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SecondName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SecondSurname = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true, defaultValue: "avatar.png"),
                    BirthDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    BirthPlace = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    NationalID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    StartDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    CompanyID = table.Column<int>(type: "int", nullable: true),
                    DepartmentID = table.Column<int>(type: "int", nullable: true),
                    Proficiency = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Salary = table.Column<decimal>(type: "money", maxLength: 20, nullable: true),
                    CurrencyType = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expences",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenceTypeID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "money", nullable: false),
                    CurrencyType = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AppUserID = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    ResponseDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    RecordStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expences", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Expences_AspNetUsers_AppUserID",
                        column: x => x.AppUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expences_ExpenceTypes_ExpenceTypeID",
                        column: x => x.ExpenceTypeID,
                        principalTable: "ExpenceTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaveRecords",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaveRecordTypeID = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    TotalDays = table.Column<short>(type: "smallint", nullable: false),
                    AppUserID = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    ResponseDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecordStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRecords", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LeaveRecords_AspNetUsers_AppUserID",
                        column: x => x.AppUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaveRecords_LeaveRecordTypes_LeaveRecordTypeID",
                        column: x => x.LeaveRecordTypeID,
                        principalTable: "LeaveRecordTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalaryAdvances",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalaryAdvanceType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CurrencyType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "money", nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AppUserID = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    ResponseDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecordStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryAdvances", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SalaryAdvances_AspNetUsers_AppUserID",
                        column: x => x.AppUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "e0144818-05f1-4387-8cd8-303a2381c2ed", "Admin", "ADMIN" },
                    { 2, "41eb5303-8361-4b7a-87b0-91a3e8d5f1a5", "CompanyManager", "COMPANYMANAGER" },
                    { 3, "46d13795-d54d-476b-8eee-137a71c43ed9", "Personel", "PERSONEL" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "BirthDate", "BirthPlace", "CompanyID", "ConcurrencyStamp", "CurrencyType", "DepartmentID", "Email", "EmailConfirmed", "EndDate", "FirstName", "IsActive", "LockoutEnabled", "LockoutEnd", "NationalID", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Proficiency", "Salary", "SecondName", "SecondSurname", "SecurityStamp", "StartDate", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 1, 0, null, null, null, null, "b51b7a8b-589b-4c7e-a12d-e252a3364bb6", 1, null, "admin@admin.com", false, null, "Admin", null, false, null, "12345678910", "ADMIN@ADMIN.COM", "ADMIN", "AQAAAAIAAYagAAAAENUXtS+LjzBfpRupRLRUTeLJsT9uCeKtESuYU9KJZXGBD2wMTNMeDhOPwhiYNXIgCQ==", "12345678910", false, null, null, null, null, "ed944e48-c178-4236-a0d2-08b610402000", null, "Admin", false, "admin" },
                    { 2, 0, null, null, null, null, "fce5fb87-5aa8-49cf-9dd6-c4e4bd3e7c63", 1, null, "manager@manager.com", false, null, "Manager", null, false, null, "12345678910", "MANAGER@MANAGER.COM", "MANAGER", "AQAAAAIAAYagAAAAEJ/5eisTRD47XftVEMJMJAKTjDAWwDshTTqIZyoxqLGaNJcsgb4foG8ZK+tKQpAw5g==", "12345678910", false, null, null, null, null, "c6edf723-0e14-40ef-be76-9c457783ad25", null, "Manager", false, "manager" },
                    { 3, 0, null, null, null, null, "f64593f5-b187-4df4-be6c-2de53da993df", 1, null, "personel@personel.com", false, null, "Personel", null, false, null, "12345678910", "PERSONEL@PERSONEL.COM", "PERSONEL", "AQAAAAIAAYagAAAAEORrMFiZjxHyi0Gc3bgq73nFML4/wUQ9k7C0tD3Q1qt8kK++hRAaqS4BFVP5s2nvnw==", "12345678910", false, null, null, null, null, "dc7a1d0a-b10a-48d1-b72f-cb52e8190e5e", null, "Personel", false, "personel" }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "ID", "Address", "CompanyName", "ContractEndDate", "ContractStartDate", "CreatedAt", "DeletedAt", "Email", "FoundationYear", "IsActive", "Logo", "MersisNum", "NumberOfEmployees", "Phone", "RecordStatus", "TaxNum", "TaxOffice", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Company 1 Address, Istanbul", "company1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 1, 14, 22, 42, 414, DateTimeKind.Utc).AddTicks(6667), null, "info@company1.com", "2001", true, "company1-logo.png", 12345678901L, 50, "+90 212 123 45 67", 1, 111111L, "Istanbul Vergi Dairesi", "Company One Title", null },
                    { 2, "Company 2 Address, Ankara", "company2", new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 1, 14, 22, 42, 414, DateTimeKind.Utc).AddTicks(6693), null, "info@company2.com", "2005", true, "company2-logo.png", 22345678901L, 75, "+90 312 234 56 78", 1, 222222L, "Ankara Vergi Dairesi", "Company Two Title", null },
                    { 3, "Company 3 Address, Izmir", "company3", new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 1, 14, 22, 42, 414, DateTimeKind.Utc).AddTicks(6699), null, "info@company3.com", "2010", true, "company3-logo.png", 32345678901L, 100, "+90 232 345 67 89", 1, 333333L, "Izmir Vergi Dairesi", "Company Three Title", null }
                });

            migrationBuilder.InsertData(
                table: "LeaveRecordTypes",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "LeaveRecordName", "RecordStatus", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 1, 17, 22, 42, 419, DateTimeKind.Local).AddTicks(153), null, "Hastalık", 1, null },
                    { 2, new DateTime(2025, 5, 1, 17, 22, 42, 419, DateTimeKind.Local).AddTicks(173), null, "Doğum", 1, null },
                    { 3, new DateTime(2025, 5, 1, 17, 22, 42, 419, DateTimeKind.Local).AddTicks(175), null, "Vefat", 1, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyID",
                table: "AspNetUsers",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DepartmentID",
                table: "AspNetUsers",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_NationalID",
                table: "AspNetUsers",
                column: "NationalID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CompanyID",
                table: "Departments",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Expences_AppUserID",
                table: "Expences",
                column: "AppUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Expences_ExpenceTypeID",
                table: "Expences",
                column: "ExpenceTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRecords_AppUserID",
                table: "LeaveRecords",
                column: "AppUserID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRecords_LeaveRecordTypeID",
                table: "LeaveRecords",
                column: "LeaveRecordTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryAdvances_AppUserID",
                table: "SalaryAdvances",
                column: "AppUserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Expences");

            migrationBuilder.DropTable(
                name: "LeaveRecords");

            migrationBuilder.DropTable(
                name: "SalaryAdvances");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ExpenceTypes");

            migrationBuilder.DropTable(
                name: "LeaveRecordTypes");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
