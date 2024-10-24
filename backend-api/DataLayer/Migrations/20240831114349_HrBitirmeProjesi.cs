using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hr.DL.Migrations
{
    /// <inheritdoc />
    public partial class HrBitirmeProjesi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstablishmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeCount = table.Column<int>(type: "int", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    SubscriptionEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    IsManager = table.Column<bool>(type: "bit", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                        name: "FK_AspNetUsers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Holidays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Holidays_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employees_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpenseType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpenseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproverComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Leaves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeaveType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leaves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leaves_Employees_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Leaves_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateSent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateRead = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_Employees_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Resumes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resumes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resumes_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShiftType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shifts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Certifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Issuer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateIssued = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResumeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certifications_Resumes_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Degree = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FieldOfStudy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResumeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Educations_Resumes_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Proficiency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResumeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Languages_Resumes_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Proficiency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResumeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_Resumes_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkExperiences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResumeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkExperiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkExperiences_Resumes_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0fcba428-3c83-4ca4-b329-32bf4e78ea92", null, "Admin", "ADMIN" },
                    { "1cc7c608-0e28-4c11-8cd0-7bf6be3a7b48", null, "Employee", "EMPLOYEE" },
                    { "704f6694-7020-48f6-bd2c-10e22732c830", null, "Manager", "MANAGER" }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Address", "CreatedAt", "Email", "EmployeeCount", "EstablishmentDate", "IsApproved", "Name", "PhoneNumber", "RegistrationDate", "SubscriptionEndDate", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Levent Mah. İş Cad. No: 5, Beşiktaş, İstanbul", new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9701), "info@techvista.com", 1, new DateTime(2022, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "TechVista Solutions", "+90 212 555 1234", new DateTime(2022, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2028, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9704) },
                    { 2, "Bahçelievler Mah. Teknoloji Cad. No: 42, Çankaya, Ankara", new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9728), "contact@innovatech.com", 6, new DateTime(2012, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "InnovaTech Yazılım A.Ş.", "+90 312 444 5678", new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9728) },
                    { 3, "Alsancak Mah. Veri Sok. No: 15, Konak, İzmir", new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9732), "info@datasphere.com", 4, new DateTime(2017, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "DataSphere Bilişim Ltd. Şti.", "+90 232 333 9876", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9732) },
                    { 4, "Esentepe Mah. Bulut Cad. No: 78, Şişli, İstanbul", new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9736), "info@cloudpeak.com", 5, new DateTime(2019, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "CloudPeak Teknoloji A.Ş.", "+90 216 777 5432", new DateTime(2023, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9736) }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "CompanyId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsManager", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Position", "RefreshToken", "RefreshTokenExpiryTime", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "22e40406-8a9d-2d82-912c-5d6a640ee001", 0, 2, "486f6e24-aa0c-491b-8220-d2a265b13307", "berk.yılmaz@example.com", true, "Berk", true, "Yılmaz", false, null, "BERK.YILMAZ@EXAMPLE.COM", "BERK.YILMAZ@EXAMPLE.COM", "AQAAAAIAAYagAAAAEFssGRgfzf6y47Xel3J0S1Hrbu6YUPmsH+2qH8pfu/dBSQUuZ1DRZJ0nGMc9rYKO6Q==", null, false, "Genel Müdür", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "b56eeb26-efa3-4388-bff8-34846d2bc7e7", false, "berk.yılmaz@example.com" },
                    { "22e40406-8a9d-2d82-912c-5d6a640ee002", 0, 2, "2867b835-1b45-4a0b-b8d8-8408500cb89c", "elif.öztürk@example.com", true, "Elif", false, "Öztürk", false, null, "ELİF.ÖZTÜRK@EXAMPLE.COM", "ELİF.ÖZTÜRK@EXAMPLE.COM", "AQAAAAIAAYagAAAAEG45J50KmlGz4xw85ao+vDp9um7eq2nyPiS4gCqUDDFfMZgkCGlTxodngU39B+xCrw==", null, false, "Yazılım Mühendisi", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ca456769-1d38-4928-991b-aeac0cbd2289", false, "elif.öztürk@example.com" },
                    { "22e40406-8a9d-2d82-912c-5d6a640ee003", 0, 2, "7f8066ee-0a75-4185-b7bc-a1bffaec5759", "can.kaya@example.com", true, "Can", false, "Kaya", false, null, "CAN.KAYA@EXAMPLE.COM", "CAN.KAYA@EXAMPLE.COM", "AQAAAAIAAYagAAAAEGpXLjUn9mNkM/Xi6AnuSfKhjxSeAaZ18zrS2oocpCevoINZj3eAgGLB60bAd7wBIg==", null, false, "Yazılım Mimarı", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "cf6747d2-e25e-474b-b1cc-04a41d24d4ae", false, "can.kaya@example.com" },
                    { "22e40406-8a9d-2d82-912c-5d6a640ee004", 0, 2, "62efc5f4-d888-4059-b581-e574b50d9ea4", "deniz.aydın@example.com", true, "Deniz", false, "Aydın", false, null, "DENİZ.AYDIN@EXAMPLE.COM", "DENİZ.AYDIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEEsEti2uuDKyVvXUdyBZfMXPJzJH2DBS0f9M14bdExvpZCzC8SKo80vEi0z2vajNug==", null, false, "İK Uzmanı", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "dddb716c-333c-43e8-80f2-1e124a47047c", false, "deniz.aydın@example.com" },
                    { "22e40406-8a9d-2d82-912c-5d6a640ee005", 0, 2, "ab26ca1f-ba83-4693-8737-d2895427353d", "emre.çelik@example.com", true, "Emre", true, "Çelik", false, null, "EMRE.ÇELİK@EXAMPLE.COM", "EMRE.ÇELİK@EXAMPLE.COM", "AQAAAAIAAYagAAAAEGTu1MlwJwbL39Fa6yF1L2gWajCyWdlFCfA/jR6WFh2GAgKRSh3k/Gwlt8pGFJwHwg==", null, false, "İK Müdürü", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "15f4ad87-e3d6-468f-9958-5f7c809ae4df", false, "emre.çelik@example.com" },
                    { "22e40406-8a9d-2d82-912c-5d6a640ee006", 0, 3, "69bd6efe-3dad-4e2a-9f5c-58b5222fbd51", "furkan.demir@example.com", true, "Furkan", false, "Demir", false, null, "FURKAN.DEMİR@EXAMPLE.COM", "FURKAN.DEMİR@EXAMPLE.COM", "AQAAAAIAAYagAAAAEIXC5pchMMzK88sC7Aq7gulQkZbTMTm4VONfo2RT2f+tFqs4TuNy9iOBY2e3/62iUQ==", null, false, "Veri Analisti", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "79b881de-f279-4d1b-a2be-2b748ba98fed", false, "furkan.demir@example.com" },
                    { "22e40406-8a9d-2d82-912c-5d6a640ee007", 0, 3, "25273522-e957-46ad-a297-876f6d8e03e1", "gizem.arslan@example.com", true, "Gizem", false, "Arslan", false, null, "GİZEM.ARSLAN@EXAMPLE.COM", "GİZEM.ARSLAN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEI/W92WheX28ExAfkLhPcKcETXmGHd7w638sV+skKMC30g5f0cF4ivXa4f026hfdZQ==", null, false, "Veri Bilimci", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "868d758b-e7cb-4b31-a3cc-7710a69c7299", false, "gizem.arslan@example.com" },
                    { "22e40406-8a9d-2d82-912c-5d6a640ee008", 0, 3, "6758ed8c-c0fc-408b-abd4-00500c70c6f9", "hakan.yıldırım@example.com", true, "Hakan", false, "Yıldırım", false, null, "HAKAN.YILDIRIM@EXAMPLE.COM", "HAKAN.YILDIRIM@EXAMPLE.COM", "AQAAAAIAAYagAAAAEN8RRB5kKQIBmwL/JcUSVJ55/8eaJix5PG9GE4R+RKq7GTgqwrpeBGH+74G0xEc67w==", null, false, "Müşteri Hizmetleri Temsilcisi", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "37508fc2-c8be-447c-9f44-a7b5f993da57", false, "hakan.yıldırım@example.com" },
                    { "22e40406-8a9d-2d82-912c-5d6a640ee009", 0, 3, "e9c49d3d-aafb-499b-868a-1889c590b024", "irem.koç@example.com", true, "İrem", true, "Koç", false, null, "İREM.KOÇ@EXAMPLE.COM", "İREM.KOÇ@EXAMPLE.COM", "AQAAAAIAAYagAAAAEDEncuv++cujt+RVk5aNfKTDhEoHR6zPHWueilmbbrZN+AMqXX8J5UjUXpP3BS46+A==", null, false, "Müşteri İlişkileri Müdürü", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "170d3728-244f-487c-89e9-753ce27fdfa0", false, "irem.koç@example.com" },
                    { "22e40406-8a9d-2d82-912c-5d6a640ee010", 0, 4, "451ec943-e343-4295-bafb-1bc83af25e74", "kerem.özer@example.com", true, "Kerem", true, "Özer", false, null, "KEREM.ÖZER@EXAMPLE.COM", "KEREM.ÖZER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEIfBo5uhLtaMXPH8RyQGaaos7fqPNXpilSOa2KpTlc9Z/j8v2FQenfPytor6QYnKew==", null, false, "Bulut Mimarı", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "cd45fd8c-6314-4be3-9b47-2ed8573fc755", false, "kerem.özer@example.com" },
                    { "22e40406-8a9d-2d82-912c-5d6a640ee011", 0, 4, "f25fd927-dc8b-41aa-8ae5-c39c0b63cd5f", "leyla.şahin@example.com", true, "Leyla", false, "Şahin", false, null, "LEYLA.ŞAHİN@EXAMPLE.COM", "LEYLA.ŞAHİN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEAua0INe2+jwtzuACpYsVQ0lje2XvYJQPZZyuZ2iFYBvW/6H7PqHWb53i750UMgl5w==", null, false, "DevOps Mühendisi", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a9b74b57-9a44-46a6-9986-0a2c19cd30be", false, "leyla.şahin@example.com" },
                    { "22e40406-8a9d-2d82-912c-5d6a640ee012", 0, 4, "7b371fd1-6c1f-4c71-be1f-0f68ee003217", "murat.akar@example.com", true, "Murat", true, "Akar", false, null, "MURAT.AKAR@EXAMPLE.COM", "MURAT.AKAR@EXAMPLE.COM", "AQAAAAIAAYagAAAAELkGR6/nFm2pPyVQWcwQRm1AMK1pLhWkxiHPmMpSOL+Zb8sOxiTIOHWh8IIUzOpDSg==", null, false, "Proje Yöneticisi", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0e531025-c902-44a0-ac91-c05964daed7a", false, "murat.akar@example.com" },
                    { "22e40406-8a9d-2d82-912c-5d6a640ee013", 0, 4, "60686d35-7a2e-4e1c-805b-70e803bed833", "neslihan.güneş@example.com", true, "Neslihan", false, "Güneş", false, null, "NESLİHAN.GÜNEŞ@EXAMPLE.COM", "NESLİHAN.GÜNEŞ@EXAMPLE.COM", "AQAAAAIAAYagAAAAEMKb8wOii1FxwKWs/TwiVvVzuTulqjtxdP5p+8Bm9odAA1+wSkRsXHS47yXuCYzyXg==", null, false, "UX/UI Tasarımcısı", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "d6c63b87-0a2e-4a25-baec-370d9597aab4", false, "neslihan.güneş@example.com" },
                    { "22e40406-8a9d-2d82-912c-5d6a640ee696", 0, 1, "4d473127-700a-447c-81fb-8a0faf77cb09", "admin@techvista.com", true, "Sistem", true, "Yöneticisi", false, null, "ADMIN@TECHVISTA.COM", "ADMIN@TECHVISTA.COM", "AQAAAAIAAYagAAAAECHmq9M56zs/82DHd9w+eIimgX6+o7v78YdjyAavjMbf1Rp+mQLDz5lF+7MNHdHsmA==", null, false, "Sistem Yöneticisi", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "c0401c8e-fe17-4f84-b17f-83b1e9a07fb8", false, "admin@techvista.com" }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CompanyId", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9960), "Şirket stratejik yönetimi", "Yönetim Kurulu", new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9960) },
                    { 2, 2, new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9971), "Yazılım araştırma ve geliştirme departmanı", "Ar-Ge", new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9971) },
                    { 3, 2, new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9973), "Personel yönetimi ve işe alım departmanı", "İnsan Kaynakları", new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9973) },
                    { 4, 2, new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9975), "Finansal işlemler ve raporlama departmanı", "Muhasebe", new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9975) },
                    { 5, 3, new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9976), "Büyük veri analizi ve raporlama departmanı", "Veri Analizi", new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9977) },
                    { 6, 3, new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9980), "Müşteri hizmetleri ve destek departmanı", "Müşteri İlişkileri", new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9980) },
                    { 7, 3, new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9982), "IT altyapı ve güvenlik yönetimi departmanı", "Sistem Yönetimi", new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9982) },
                    { 8, 4, new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9983), "Bulut tabanlı çözümler geliştirme departmanı", "Bulut Hizmetleri", new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9984) },
                    { 9, 4, new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9985), "Yazılım proje yönetimi departmanı", "Proje Yönetimi", new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9986) },
                    { 10, 4, new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9987), "Yeni ürün geliştirme ve tasarım departmanı", "Ürün Geliştirme", new DateTime(2024, 8, 31, 11, 43, 46, 839, DateTimeKind.Utc).AddTicks(9988) }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "CompanyId", "CreatedAt", "Description", "EventDate", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7031), "Yıllık teknoloji trendleri ve inovasyon zirvesi", new DateTime(2024, 5, 15, 9, 0, 0, 0, DateTimeKind.Unspecified), "Teknoloji Zirvesi", new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7035) },
                    { 2, 4, new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7050), "Yeni bulut projesinin başlangıç toplantısı", new DateTime(2024, 6, 1, 14, 0, 0, 0, DateTimeKind.Unspecified), "Proje Kickoff Toplantısı", new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7067) },
                    { 3, 3, new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7070), "Güncel veri güvenliği uygulamaları hakkında seminer", new DateTime(2024, 7, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), "Veri Güvenliği Semineri", new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7071) },
                    { 4, 2, new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7074), "2024 yılı performans ve hedef değerlendirmesi", new DateTime(2024, 12, 20, 15, 0, 0, 0, DateTimeKind.Unspecified), "Yıl Sonu Değerlendirme Toplantısı", new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7075) },
                    { 5, 4, new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7078), "Proje yönetiminde agile metodolojiler üzerine eğitim", new DateTime(2024, 8, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), "Agile Metodolojiler Eğitimi", new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7078) },
                    { 6, 3, new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7083), "Müşteri memnuniyetini artırma stratejileri üzerine çalıştay", new DateTime(2024, 9, 15, 13, 0, 0, 0, DateTimeKind.Unspecified), "Müşteri Deneyimi Çalıştayı", new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7083) },
                    { 7, 2, new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7086), "AI kullanımında etik konular üzerine panel tartışması", new DateTime(2024, 10, 1, 11, 0, 0, 0, DateTimeKind.Unspecified), "Yapay Zeka ve Etik Paneli", new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7087) },
                    { 8, 4, new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7090), "Güncel siber tehditler ve korunma yöntemleri konferansı", new DateTime(2024, 11, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), "Siber Güvenlik Konferansı", new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7090) }
                });

            migrationBuilder.InsertData(
                table: "Holidays",
                columns: new[] { "Id", "CompanyId", "CreatedAt", "Date", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7443), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yeni yıl kutlaması", "Yeni Yıl", new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7443) },
                    { 2, 2, new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7448), new DateTime(2024, 4, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "23 Nisan kutlamaları", "Ulusal Egemenlik ve Çocuk Bayramı", new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7449) },
                    { 3, 3, new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7452), new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1 Mayıs İşçi Bayramı", "Emek ve Dayanışma Günü", new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7452) },
                    { 4, 3, new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7455), new DateTime(2024, 5, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "19 Mayıs kutlamaları", "Atatürk'ü Anma, Gençlik ve Spor Bayramı", new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7457) },
                    { 5, 4, new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7460), new DateTime(2024, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "15 Temmuz anma etkinlikleri", "Demokrasi ve Milli Birlik Günü", new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7461) },
                    { 6, 4, new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7464), new DateTime(2024, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "30 Ağustos kutlamaları", "Zafer Bayramı", new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7465) },
                    { 7, 2, new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7471), new DateTime(2024, 10, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "29 Ekim kutlamaları", "Cumhuriyet Bayramı", new DateTime(2024, 8, 31, 11, 43, 48, 243, DateTimeKind.Utc).AddTicks(7471) }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "704f6694-7020-48f6-bd2c-10e22732c830", "22e40406-8a9d-2d82-912c-5d6a640ee001" },
                    { "1cc7c608-0e28-4c11-8cd0-7bf6be3a7b48", "22e40406-8a9d-2d82-912c-5d6a640ee002" },
                    { "1cc7c608-0e28-4c11-8cd0-7bf6be3a7b48", "22e40406-8a9d-2d82-912c-5d6a640ee003" },
                    { "1cc7c608-0e28-4c11-8cd0-7bf6be3a7b48", "22e40406-8a9d-2d82-912c-5d6a640ee004" },
                    { "704f6694-7020-48f6-bd2c-10e22732c830", "22e40406-8a9d-2d82-912c-5d6a640ee005" },
                    { "1cc7c608-0e28-4c11-8cd0-7bf6be3a7b48", "22e40406-8a9d-2d82-912c-5d6a640ee006" },
                    { "1cc7c608-0e28-4c11-8cd0-7bf6be3a7b48", "22e40406-8a9d-2d82-912c-5d6a640ee007" },
                    { "1cc7c608-0e28-4c11-8cd0-7bf6be3a7b48", "22e40406-8a9d-2d82-912c-5d6a640ee008" },
                    { "704f6694-7020-48f6-bd2c-10e22732c830", "22e40406-8a9d-2d82-912c-5d6a640ee009" },
                    { "704f6694-7020-48f6-bd2c-10e22732c830", "22e40406-8a9d-2d82-912c-5d6a640ee010" },
                    { "1cc7c608-0e28-4c11-8cd0-7bf6be3a7b48", "22e40406-8a9d-2d82-912c-5d6a640ee011" },
                    { "704f6694-7020-48f6-bd2c-10e22732c830", "22e40406-8a9d-2d82-912c-5d6a640ee012" },
                    { "1cc7c608-0e28-4c11-8cd0-7bf6be3a7b48", "22e40406-8a9d-2d82-912c-5d6a640ee013" },
                    { "0fcba428-3c83-4ca4-b329-32bf4e78ea92", "22e40406-8a9d-2d82-912c-5d6a640ee696" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Birthdate", "CompanyId", "CreatedAt", "DepartmentId", "Email", "FirstName", "HireDate", "Id", "LastName", "PhoneNumber", "Position", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("22e40406-8a9d-2d82-912c-5d6a640ee001"), new DateTime(1998, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2024, 8, 31, 11, 43, 46, 970, DateTimeKind.Utc).AddTicks(842), 2, "berk.yılmaz@example.com", "Berk", new DateTime(2023, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Yılmaz", "+90 523 874 61 92", "Genel Müdür", new DateTime(2024, 8, 31, 11, 43, 46, 970, DateTimeKind.Utc).AddTicks(850) },
                    { new Guid("22e40406-8a9d-2d82-912c-5d6a640ee002"), new DateTime(1985, 1, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2024, 8, 31, 11, 43, 47, 69, DateTimeKind.Utc).AddTicks(4354), 2, "elif.öztürk@example.com", "Elif", new DateTime(2021, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Öztürk", "+90 536 630 27 99", "Yazılım Mühendisi", new DateTime(2024, 8, 31, 11, 43, 47, 69, DateTimeKind.Utc).AddTicks(4360) },
                    { new Guid("22e40406-8a9d-2d82-912c-5d6a640ee003"), new DateTime(1987, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2024, 8, 31, 11, 43, 47, 185, DateTimeKind.Utc).AddTicks(1226), 2, "can.kaya@example.com", "Can", new DateTime(2020, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Kaya", "+90 547 826 90 33", "Yazılım Mimarı", new DateTime(2024, 8, 31, 11, 43, 47, 185, DateTimeKind.Utc).AddTicks(1229) },
                    { new Guid("22e40406-8a9d-2d82-912c-5d6a640ee004"), new DateTime(1980, 5, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2024, 8, 31, 11, 43, 47, 320, DateTimeKind.Utc).AddTicks(7010), 3, "deniz.aydın@example.com", "Deniz", new DateTime(2021, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Aydın", "+90 511 498 31 19", "İK Uzmanı", new DateTime(2024, 8, 31, 11, 43, 47, 320, DateTimeKind.Utc).AddTicks(7014) },
                    { new Guid("22e40406-8a9d-2d82-912c-5d6a640ee005"), new DateTime(1983, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2024, 8, 31, 11, 43, 47, 409, DateTimeKind.Utc).AddTicks(1276), 3, "emre.çelik@example.com", "Emre", new DateTime(2020, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "Çelik", "+90 529 152 59 99", "İK Müdürü", new DateTime(2024, 8, 31, 11, 43, 47, 409, DateTimeKind.Utc).AddTicks(1281) },
                    { new Guid("22e40406-8a9d-2d82-912c-5d6a640ee006"), new DateTime(1994, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2024, 8, 31, 11, 43, 47, 500, DateTimeKind.Utc).AddTicks(9497), 5, "furkan.demir@example.com", "Furkan", new DateTime(2022, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "Demir", "+90 550 208 76 71", "Veri Analisti", new DateTime(2024, 8, 31, 11, 43, 47, 500, DateTimeKind.Utc).AddTicks(9500) },
                    { new Guid("22e40406-8a9d-2d82-912c-5d6a640ee007"), new DateTime(1987, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2024, 8, 31, 11, 43, 47, 623, DateTimeKind.Utc).AddTicks(5555), 5, "gizem.arslan@example.com", "Gizem", new DateTime(2023, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "Arslan", "+90 549 828 63 39", "Veri Bilimci", new DateTime(2024, 8, 31, 11, 43, 47, 623, DateTimeKind.Utc).AddTicks(5559) },
                    { new Guid("22e40406-8a9d-2d82-912c-5d6a640ee008"), new DateTime(1997, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2024, 8, 31, 11, 43, 47, 732, DateTimeKind.Utc).AddTicks(5302), 6, "hakan.yıldırım@example.com", "Hakan", new DateTime(2020, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, "Yıldırım", "+90 516 531 60 94", "Müşteri Hizmetleri Temsilcisi", new DateTime(2024, 8, 31, 11, 43, 47, 732, DateTimeKind.Utc).AddTicks(5306) },
                    { new Guid("22e40406-8a9d-2d82-912c-5d6a640ee009"), new DateTime(1983, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2024, 8, 31, 11, 43, 47, 832, DateTimeKind.Utc).AddTicks(2024), 6, "irem.koç@example.com", "İrem", new DateTime(2022, 5, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, "Koç", "+90 530 681 46 58", "Müşteri İlişkileri Müdürü", new DateTime(2024, 8, 31, 11, 43, 47, 832, DateTimeKind.Utc).AddTicks(2027) },
                    { new Guid("22e40406-8a9d-2d82-912c-5d6a640ee010"), new DateTime(1986, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(2024, 8, 31, 11, 43, 47, 915, DateTimeKind.Utc).AddTicks(3275), 8, "kerem.özer@example.com", "Kerem", new DateTime(2021, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "Özer", "+90 543 872 20 73", "Bulut Mimarı", new DateTime(2024, 8, 31, 11, 43, 47, 915, DateTimeKind.Utc).AddTicks(3278) },
                    { new Guid("22e40406-8a9d-2d82-912c-5d6a640ee011"), new DateTime(1998, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(2024, 8, 31, 11, 43, 47, 987, DateTimeKind.Utc).AddTicks(3765), 8, "leyla.şahin@example.com", "Leyla", new DateTime(2021, 7, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, "Şahin", "+90 529 653 55 77", "DevOps Mühendisi", new DateTime(2024, 8, 31, 11, 43, 47, 987, DateTimeKind.Utc).AddTicks(3767) },
                    { new Guid("22e40406-8a9d-2d82-912c-5d6a640ee012"), new DateTime(1993, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(2024, 8, 31, 11, 43, 48, 58, DateTimeKind.Utc).AddTicks(4498), 9, "murat.akar@example.com", "Murat", new DateTime(2021, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, "Akar", "+90 552 763 42 21", "Proje Yöneticisi", new DateTime(2024, 8, 31, 11, 43, 48, 58, DateTimeKind.Utc).AddTicks(4502) },
                    { new Guid("22e40406-8a9d-2d82-912c-5d6a640ee013"), new DateTime(1989, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(2024, 8, 31, 11, 43, 48, 138, DateTimeKind.Utc).AddTicks(2394), 10, "neslihan.güneş@example.com", "Neslihan", new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, "Güneş", "+90 547 763 78 90", "UX/UI Tasarımcısı", new DateTime(2024, 8, 31, 11, 43, 48, 138, DateTimeKind.Utc).AddTicks(2397) }
                });

            migrationBuilder.InsertData(
                table: "Expenses",
                columns: new[] { "Id", "Amount", "ApproverComments", "CreatedAt", "Description", "EmployeeId", "ExpenseDate", "ExpenseType", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1500.00m, "Onaylandı, şirket için faydalı olacak", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7261), "Yıllık Yazılım Geliştirme Konferansı katılım ücreti", new Guid("22e40406-8a9d-2d82-912c-5d6a640ee001"), new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Konferans", "Approved", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7280) },
                    { 2, 2500.50m, null, new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7288), "Yeni geliştirme laptopı", new Guid("22e40406-8a9d-2d82-912c-5d6a640ee002"), new DateTime(2023, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ekipman", "Pending", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7290) },
                    { 3, 850.75m, "Onaylandı", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7296), "Müşteri ziyareti uçak bileti", new Guid("22e40406-8a9d-2d82-912c-5d6a640ee003"), new DateTime(2023, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Seyahat", "Approved", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7298) },
                    { 4, 1200.00m, "Onaylandı, kariyer gelişimi için önemli", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7311), "İnsan Kaynakları Yönetimi Sertifikası", new Guid("22e40406-8a9d-2d82-912c-5d6a640ee004"), new DateTime(2023, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Eğitim", "Approved", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7313) },
                    { 5, 3000.00m, "Onaylandı, gerekli yazılım", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7320), "Yıllık kurumsal yazılım lisansı yenilemesi", new Guid("22e40406-8a9d-2d82-912c-5d6a640ee005"), new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yazılım Lisansı", "Approved", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7321) },
                    { 6, 450.25m, "Onaylandı", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7329), "Kırtasiye ve ofis sarf malzemeleri", new Guid("22e40406-8a9d-2d82-912c-5d6a640ee006"), new DateTime(2023, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ofis Malzemeleri", "Approved", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7332) },
                    { 7, 300.00m, null, new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7339), "Sektör networking etkinliği katılım ücreti", new Guid("22e40406-8a9d-2d82-912c-5d6a640ee007"), new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Networking", "Pending", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7341) },
                    { 8, 550.50m, "Onaylandı, iş geliştirme faaliyeti", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7347), "Potansiyel müşteri ile akşam yemeği", new Guid("22e40406-8a9d-2d82-912c-5d6a640ee008"), new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Müşteri Ağırlama", "Approved", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7349) }
                });

            migrationBuilder.InsertData(
                table: "Leaves",
                columns: new[] { "Id", "ApproverId", "Comments", "CreatedAt", "EmployeeId", "EndDate", "LeaveType", "Reason", "StartDate", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new Guid("22e40406-8a9d-2d82-912c-5d6a640ee005"), "İyi tatiller", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7613), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee001"), new DateTime(2023, 7, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yıllık İzin", "Yaz tatili", new DateTime(2023, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7616) },
                    { 2, new Guid("22e40406-8a9d-2d82-912c-5d6a640ee005"), "Geçmiş olsun", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7626), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee002"), new DateTime(2023, 8, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hastalık İzni", "Grip", new DateTime(2023, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7628) },
                    { 3, new Guid("22e40406-8a9d-2d82-912c-5d6a640ee005"), "Onaylandı", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7637), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee003"), new DateTime(2023, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Özel İzin", "Aile etkinliği", new DateTime(2023, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7638) },
                    { 4, null, "İnceleme aşamasında", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7646), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee004"), new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yıllık İzin", "Kişisel gezi", new DateTime(2023, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7648) },
                    { 5, new Guid("22e40406-8a9d-2d82-912c-5d6a640ee001"), "Konferans raporu bekleniyor", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7677), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee005"), new DateTime(2023, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Konferans İzni", "İK Konferansı katılımı", new DateTime(2023, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7679) },
                    { 6, new Guid("22e40406-8a9d-2d82-912c-5d6a640ee009"), "İyi tatiller", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7691), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee006"), new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yıllık İzin", "Yılbaşı tatili", new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7693) },
                    { 7, new Guid("22e40406-8a9d-2d82-912c-5d6a640ee009"), "Doktor raporu alındı", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7704), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee007"), new DateTime(2024, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hastalık İzni", "Soğuk algınlığı", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7711) },
                    { 8, null, "Değerlendirme aşamasında", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7774), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee008"), new DateTime(2024, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yıllık İzin", "Kış tatili", new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7777) },
                    { 9, new Guid("22e40406-8a9d-2d82-912c-5d6a640ee001"), "Onaylandı", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7791), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee009"), new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Özel İzin", "Kişisel işler", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved", new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(7793) }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "CompanyId", "CreatedAt", "DateRead", "DateSent", "EmployeeId", "ExpiryDate", "Message", "NotificationType", "Priority", "SenderId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(9965), null, new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee001"), new DateTime(2023, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yeni proje başlatıldı: CloudSync", "Proje", "High", new Guid("22e40406-8a9d-2d82-912c-5d6a640ee005"), new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(9970) },
                    { 2, 2, new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(9981), new DateTime(2023, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee002"), new DateTime(2023, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yıllık performans değerlendirmeleri başlıyor", "İK", "Medium", new Guid("22e40406-8a9d-2d82-912c-5d6a640ee005"), new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(9982) },
                    { 3, 3, new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(9990), null, new DateTime(2023, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee003"), new DateTime(2023, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yeni güvenlik politikası yayınlandı", "Güvenlik", "High", new Guid("22e40406-8a9d-2d82-912c-5d6a640ee009"), new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(9991) },
                    { 4, 2, new DateTime(2024, 8, 31, 14, 43, 48, 243, DateTimeKind.Local).AddTicks(9999), new DateTime(2023, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee004"), new DateTime(2023, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ofis taşınma süreci başlıyor", "Genel", "Medium", new Guid("22e40406-8a9d-2d82-912c-5d6a640ee001"), new DateTime(2024, 8, 31, 14, 43, 48, 244, DateTimeKind.Local) },
                    { 5, 2, new DateTime(2024, 8, 31, 14, 43, 48, 244, DateTimeKind.Local).AddTicks(8), null, new DateTime(2023, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee005"), new DateTime(2023, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yeni müşteri kazanıldı: TechCorp", "Satış", "High", new Guid("22e40406-8a9d-2d82-912c-5d6a640ee001"), new DateTime(2024, 8, 31, 14, 43, 48, 244, DateTimeKind.Local).AddTicks(9) },
                    { 6, 3, new DateTime(2024, 8, 31, 14, 43, 48, 244, DateTimeKind.Local).AddTicks(18), null, new DateTime(2023, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee006"), new DateTime(2023, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yeni eğitim programı başlıyor: AI ve ML Temelleri", "Eğitim", "Medium", new Guid("22e40406-8a9d-2d82-912c-5d6a640ee005"), new DateTime(2024, 8, 31, 14, 43, 48, 244, DateTimeKind.Local).AddTicks(19) },
                    { 7, 4, new DateTime(2024, 8, 31, 14, 43, 48, 244, DateTimeKind.Local).AddTicks(27), new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee007"), new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sistem bakımı planlı kesinti", "IT", "High", new Guid("22e40406-8a9d-2d82-912c-5d6a640ee010"), new DateTime(2024, 8, 31, 14, 43, 48, 244, DateTimeKind.Local).AddTicks(28) },
                    { 8, 3, new DateTime(2024, 8, 31, 14, 43, 48, 244, DateTimeKind.Local).AddTicks(35), null, new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee008"), new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yılbaşı kutlaması hatırlatması", "Etkinlik", "Low", new Guid("22e40406-8a9d-2d82-912c-5d6a640ee005"), new DateTime(2024, 8, 31, 14, 43, 48, 244, DateTimeKind.Local).AddTicks(36) },
                    { 9, 4, new DateTime(2024, 8, 31, 14, 43, 48, 244, DateTimeKind.Local).AddTicks(44), new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee009"), new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yeni yıl hedefleri toplantısı", "Toplantı", "Medium", new Guid("22e40406-8a9d-2d82-912c-5d6a640ee001"), new DateTime(2024, 8, 31, 14, 43, 48, 244, DateTimeKind.Local).AddTicks(46) }
                });

            migrationBuilder.InsertData(
                table: "Resumes",
                columns: new[] { "Id", "AdditionalInformation", "CreatedAt", "EmployeeId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "10+ yıl yazılım geliştirme ve yöneticilik deneyimi", new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(168), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee001"), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(169) },
                    { 2, "Full-stack geliştirici, mikroservis mimarileri konusunda uzman", new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(174), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee002"), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(174) },
                    { 3, "Yazılım mimarisi ve büyük ölçekli sistemler konusunda deneyimli", new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(177), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee003"), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(177) },
                    { 4, "İK süreçleri optimizasyonu ve çalışan deneyimi konularında uzman", new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(180), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee004"), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(180) },
                    { 5, "Organizasyonel gelişim ve yetenek yönetimi konularında deneyimli", new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(183), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee005"), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(184) },
                    { 6, "Makine öğrenmesi ve veri madenciliği alanlarında uzman", new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(187), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee006"), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(187) },
                    { 7, "Büyük veri analizi ve iş zekası çözümleri konusunda deneyimli", new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(190), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee007"), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(190) },
                    { 8, "Müşteri ilişkileri yönetimi ve sorun çözme becerileri güçlü", new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(193), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee008"), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(194) },
                    { 9, "Müşteri memnuniyeti stratejileri geliştirme konusunda uzman", new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(196), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee009"), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(197) },
                    { 10, "Bulut mimarisi ve DevOps uygulamaları konusunda ileri düzey bilgi", new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(200), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee010"), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(200) },
                    { 11, "CI/CD ve konteynerizasyon teknolojileri konusunda uzman", new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(203), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee011"), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(203) },
                    { 12, "Agile ve Scrum metodolojileri konusunda sertifikalı", new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(206), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee012"), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(206) },
                    { 13, "Kullanıcı deneyimi tasarımı ve kullanılabilirlik testleri konusunda deneyimli", new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(226), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee013"), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(226) }
                });

            migrationBuilder.InsertData(
                table: "Shifts",
                columns: new[] { "Id", "CreatedAt", "EmployeeId", "EndTime", "Notes", "ShiftType", "StartTime", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1662), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee001"), new DateTime(2023, 8, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), "Sprint planlama toplantısı", "Tam Gün", new DateTime(2023, 8, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1664) },
                    { 2, new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1671), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee002"), new DateTime(2023, 8, 2, 18, 0, 0, 0, DateTimeKind.Unspecified), "Kod gözden geçirme", "Tam Gün", new DateTime(2023, 8, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1672) },
                    { 3, new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1675), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee003"), new DateTime(2023, 8, 3, 18, 0, 0, 0, DateTimeKind.Unspecified), "Mimari tasarım toplantısı", "Tam Gün", new DateTime(2023, 8, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1675) },
                    { 4, new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1678), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee004"), new DateTime(2023, 8, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), "İşe alım görüşmeleri", "Tam Gün", new DateTime(2023, 8, 4, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1678) },
                    { 5, new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1681), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee005"), new DateTime(2023, 8, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), "Performans değerlendirme toplantıları", "Tam Gün", new DateTime(2023, 8, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1681) },
                    { 6, new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1686), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee006"), new DateTime(2023, 8, 6, 18, 0, 0, 0, DateTimeKind.Unspecified), "Veri analizi projesi", "Tam Gün", new DateTime(2023, 8, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1686) },
                    { 7, new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1689), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee007"), new DateTime(2023, 8, 7, 18, 0, 0, 0, DateTimeKind.Unspecified), "Rapor hazırlama", "Tam Gün", new DateTime(2023, 8, 7, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1690) },
                    { 8, new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1693), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee008"), new DateTime(2023, 8, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), "Müşteri destek çağrıları", "Tam Gün", new DateTime(2023, 8, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1693) },
                    { 9, new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1696), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee009"), new DateTime(2023, 8, 9, 18, 0, 0, 0, DateTimeKind.Unspecified), "Müşteri memnuniyeti analizi", "Tam Gün", new DateTime(2023, 8, 9, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1696) },
                    { 10, new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1700), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee010"), new DateTime(2023, 8, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), "Bulut altyapı optimizasyonu", "Tam Gün", new DateTime(2023, 8, 10, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1700) },
                    { 11, new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1703), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee011"), new DateTime(2023, 8, 11, 18, 0, 0, 0, DateTimeKind.Unspecified), "CI/CD pipeline güncellemesi", "Tam Gün", new DateTime(2023, 8, 11, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1703) },
                    { 12, new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1713), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee012"), new DateTime(2023, 8, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), "Sprint retrospektif toplantısı", "Tam Gün", new DateTime(2023, 8, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1713) },
                    { 13, new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1716), new Guid("22e40406-8a9d-2d82-912c-5d6a640ee013"), new DateTime(2023, 8, 13, 18, 0, 0, 0, DateTimeKind.Unspecified), "Kullanıcı arayüzü tasarım revizyonu", "Tam Gün", new DateTime(2023, 8, 13, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 31, 11, 43, 48, 244, DateTimeKind.Utc).AddTicks(1716) }
                });

            migrationBuilder.InsertData(
                table: "Certifications",
                columns: new[] { "Id", "DateIssued", "Issuer", "Name", "ResumeId" },
                values: new object[,]
                {
                    { 1, new DateTime(2018, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "PMI", "PMP (Project Management Professional)", 1 },
                    { 2, new DateTime(2019, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Amazon Web Services", "AWS Certified Solutions Architect", 2 },
                    { 3, new DateTime(2020, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Open Group", "TOGAF Certified", 3 },
                    { 4, new DateTime(2017, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "SHRM", "SHRM-CP (Certified Professional)", 4 },
                    { 5, new DateTime(2019, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "HRCI", "SPHR (Senior Professional in Human Resources)", 5 },
                    { 6, new DateTime(2021, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Google", "TensorFlow Developer Certificate", 6 },
                    { 7, new DateTime(2020, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "INFORMS", "Certified Analytics Professional", 7 },
                    { 8, new DateTime(2022, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "CXPA", "CCXP (Certified Customer Experience Professional)", 8 },
                    { 9, new DateTime(2019, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "ICMI", "CCSP (Certified Customer Service Professional)", 9 },
                    { 10, new DateTime(2021, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Google", "Google Cloud Professional Cloud Architect", 10 },
                    { 11, new DateTime(2022, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Linux Foundation", "Certified Kubernetes Administrator", 11 },
                    { 12, new DateTime(2020, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "PMI", "PMI-ACP (Agile Certified Practitioner)", 12 },
                    { 13, new DateTime(2021, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adobe", "Adobe Certified Expert - Experience Designer", 13 }
                });

            migrationBuilder.InsertData(
                table: "Educations",
                columns: new[] { "Id", "Degree", "EndDate", "FieldOfStudy", "ResumeId", "SchoolName", "StartDate" },
                values: new object[,]
                {
                    { 1, "Yüksek Lisans", new DateTime(2010, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bilgisayar Mühendisliği", 1, "İstanbul Teknik Üniversitesi", new DateTime(2008, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Lisans", new DateTime(2014, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yazılım Mühendisliği", 2, "Boğaziçi Üniversitesi", new DateTime(2010, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Doktora", new DateTime(2016, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bilgisayar Bilimleri", 3, "Orta Doğu Teknik Üniversitesi", new DateTime(2012, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Lisans", new DateTime(2015, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İnsan Kaynakları Yönetimi", 4, "Marmara Üniversitesi", new DateTime(2011, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Yüksek Lisans", new DateTime(2015, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İşletme", 5, "Yıldız Teknik Üniversitesi", new DateTime(2013, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "Lisans", new DateTime(2016, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İstatistik", 6, "Hacettepe Üniversitesi", new DateTime(2012, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "Yüksek Lisans", new DateTime(2017, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Veri Bilimi", 7, "Ankara Üniversitesi", new DateTime(2015, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, "Lisans", new DateTime(2017, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İşletme", 8, "Ege Üniversitesi", new DateTime(2013, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, "Yüksek Lisans", new DateTime(2016, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Müşteri İlişkileri Yönetimi", 9, "Dokuz Eylül Üniversitesi", new DateTime(2014, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, "Lisans", new DateTime(2015, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bilgisayar Mühendisliği", 10, "Bilkent Üniversitesi", new DateTime(2011, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, "Yüksek Lisans", new DateTime(2018, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bilgi Teknolojileri", 11, "Sabancı Üniversitesi", new DateTime(2016, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, "Lisans", new DateTime(2016, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Endüstri Mühendisliği", 12, "Koç Üniversitesi", new DateTime(2012, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, "Lisans", new DateTime(2018, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bilgisayar Mühendisliği", 13, "Galatasaray Üniversitesi", new DateTime(2014, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Name", "Proficiency", "ResumeId" },
                values: new object[,]
                {
                    { 1, "İngilizce", "İleri Düzey", 1 },
                    { 2, "İngilizce", "Akıcı", 2 },
                    { 3, "İngilizce", "İleri Düzey", 3 },
                    { 4, "İngilizce", "Orta Düzey", 4 },
                    { 5, "İngilizce", "İleri Düzey", 5 },
                    { 6, "İngilizce", "Akıcı", 6 },
                    { 7, "İngilizce", "İleri Düzey", 7 },
                    { 8, "İngilizce", "Orta Düzey", 8 },
                    { 9, "İngilizce", "İleri Düzey", 9 },
                    { 10, "İngilizce", "Akıcı", 10 },
                    { 11, "İngilizce", "İleri Düzey", 11 },
                    { 12, "İngilizce", "Akıcı", 12 },
                    { 13, "İngilizce", "İleri Düzey", 13 }
                });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "Name", "Proficiency", "ResumeId" },
                values: new object[,]
                {
                    { 1, "Proje Yönetimi", "Uzman", 1 },
                    { 2, "Mikroservis Mimarileri", "İleri Düzey", 2 },
                    { 3, "Sistem Tasarımı", "Uzman", 3 },
                    { 4, "İşe Alım Süreçleri", "İleri Düzey", 4 },
                    { 5, "Yetenek Yönetimi", "Uzman", 5 },
                    { 6, "Makine Öğrenmesi", "İleri Düzey", 6 },
                    { 7, "Veri Analizi", "Uzman", 7 },
                    { 8, "Müşteri İlişkileri", "İleri Düzey", 8 },
                    { 9, "Müşteri Hizmetleri Stratejisi", "Uzman", 9 },
                    { 10, "Bulut Mimarisi", "Uzman", 10 },
                    { 11, "DevOps", "İleri Düzey", 11 },
                    { 12, "Agile Metodolojileri", "Uzman", 12 },
                    { 13, "UX/UI Tasarımı", "İleri Düzey", 13 }
                });

            migrationBuilder.InsertData(
                table: "WorkExperiences",
                columns: new[] { "Id", "CompanyName", "Description", "EndDate", "Position", "ResumeId", "StartDate" },
                values: new object[,]
                {
                    { 1, "TechSoft A.Ş.", "Büyük ölçekli yazılım projelerini yönetti ve ekip liderliği yaptı.", new DateTime(2020, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yazılım Geliştirme Müdürü", 1, new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "CloudTech", "Mikroservis mimarileri üzerinde çalıştı ve DevOps süreçlerini iyileştirdi.", new DateTime(2021, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kıdemli Yazılım Mühendisi", 2, new DateTime(2016, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "DataWiz", "Yüksek ölçeklenebilir sistemler tasarladı ve uyguladı.", new DateTime(2022, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yazılım Mimarı", 3, new DateTime(2017, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "PeopleTech", "İşe alım süreçlerini optimize etti ve çalışan memnuniyetini artırdı.", new DateTime(2020, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "İK Uzmanı", 4, new DateTime(2015, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "TalentCorp", "Yetenek yönetimi stratejileri geliştirdi ve uyguladı.", new DateTime(2022, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "İK Müdürü", 5, new DateTime(2017, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "AILabs", "Makine öğrenmesi modellerini geliştirdi ve uyguladı.", new DateTime(2023, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Veri Bilimci", 6, new DateTime(2018, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "BigDataCo", "Büyük veri setlerini analiz etti ve iş zekası raporları oluşturdu.", new DateTime(2022, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Veri Analisti", 7, new DateTime(2017, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, "CustomerFirst", "Müşteri sorunlarını çözdü ve müşteri memnuniyetini artırdı.", new DateTime(2023, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Müşteri Hizmetleri Temsilcisi", 8, new DateTime(2018, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, "ServicePro", "Müşteri hizmetleri stratejilerini geliştirdi ve uyguladı.", new DateTime(2022, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Müşteri İlişkileri Müdürü", 9, new DateTime(2016, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, "CloudMasters", "Büyük ölçekli bulut altyapıları tasarladı ve uyguladı.", new DateTime(2022, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bulut Mimarı", 10, new DateTime(2015, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, "DevOpsHub", "CI/CD süreçlerini otomatikleştirdi ve konteyner teknolojilerini uyguladı.", new DateTime(2023, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "DevOps Mühendisi", 11, new DateTime(2018, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, "AgileWorks", "Agile metodolojileri kullanarak büyük projeleri yönetti.", new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Proje Yöneticisi", 12, new DateTime(2017, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, "UXStudio", "Kullanıcı odaklı arayüzler tasarladı ve kullanılabilirlik testleri yaptı.", new DateTime(2023, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "UX/UI Tasarımcısı", 13, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
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
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Certifications_ResumeId",
                table: "Certifications",
                column: "ResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CompanyId",
                table: "Departments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_ResumeId",
                table: "Educations",
                column: "ResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId",
                table: "Employees",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CompanyId",
                table: "Events",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_EmployeeId",
                table: "Expenses",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Holidays_CompanyId",
                table: "Holidays",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_ResumeId",
                table: "Languages",
                column: "ResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_ApproverId",
                table: "Leaves",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_EmployeeId",
                table: "Leaves",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CompanyId",
                table: "Notifications",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_EmployeeId",
                table: "Notifications",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SenderId",
                table: "Notifications",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_EmployeeId",
                table: "Resumes",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_EmployeeId",
                table: "Shifts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_ResumeId",
                table: "Skills",
                column: "ResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperiences_ResumeId",
                table: "WorkExperiences",
                column: "ResumeId");
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
                name: "Certifications");

            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "Holidays");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Leaves");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "WorkExperiences");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Resumes");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
