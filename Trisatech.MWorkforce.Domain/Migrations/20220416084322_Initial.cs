using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Trisatech.MWorkforce.Domain.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssignmentStatuses",
                columns: table => new
                {
                    AssignmentStatusCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    AssignmentStatusName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentStatuses", x => x.AssignmentStatusCode);
                });

            migrationBuilder.CreateTable(
                name: "CustomerContactAgents",
                columns: table => new
                {
                    CustomerContactAgentId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ContactId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SalesId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<byte>(type: "smallint", nullable: false),
                    IsDeleted = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerContactAgents", x => x.CustomerContactAgentId);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomerEmail = table.Column<string>(type: "text", nullable: true),
                    CustomerName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CustomerCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CustomerAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CustomerPhoneNumber = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    CustomerCity = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CustomerPhoto = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CustomerLatitude = table.Column<double>(type: "double precision", nullable: true),
                    CustomerLongitude = table.Column<double>(type: "double precision", nullable: true),
                    Desc = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CustomerDistrict = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CustomerVillage = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CustomerPhotoId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CustomerPhotoNPWP = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CustomerPhotoBlobId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CustomerPhotoIdBlobId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CustomerPhotoNPWPBlobId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<byte>(type: "smallint", nullable: false),
                    IsDeleted = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "DataProtectionKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FriendlyName = table.Column<string>(type: "text", nullable: true),
                    Xml = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProtectionKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    NewsId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: true),
                    Desc = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    IsPublish = table.Column<bool>(type: "boolean", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<byte>(type: "smallint", nullable: false),
                    IsDeleted = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.NewsId);
                });

            migrationBuilder.CreateTable(
                name: "Otps",
                columns: table => new
                {
                    OtpId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: true),
                    ItemId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Value = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    IsValid = table.Column<bool>(type: "boolean", nullable: false),
                    ValidTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Otps", x => x.OtpId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProductCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    ProductName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ProductModel = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ProductPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    ProductImage = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<byte>(type: "smallint", nullable: false),
                    IsDeleted = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "RefAssigments",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    GroupId = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Desc = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefAssigments", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RoleCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RoleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    SurveyId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SurveyName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    SurveyLink = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<byte>(type: "smallint", nullable: false),
                    IsDeleted = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.SurveyId);
                });

            migrationBuilder.CreateTable(
                name: "Territories",
                columns: table => new
                {
                    TerritoryId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: true),
                    Desc = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<byte>(type: "smallint", nullable: false),
                    IsDeleted = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Territories", x => x.TerritoryId);
                });

            migrationBuilder.CreateTable(
                name: "UserLocations",
                columns: table => new
                {
                    UserLocationId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserCode = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Reserved1 = table.Column<string>(type: "text", nullable: true),
                    Reserved2 = table.Column<string>(type: "text", nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLocations", x => x.UserLocationId);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ContactId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ContactName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    Position = table.Column<string>(type: "text", nullable: true),
                    ContactNumber = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SecondaryEmail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ContactPhoto = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Remarks = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CustomerId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<byte>(type: "smallint", nullable: false),
                    IsDeleted = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ContactId);
                    table.ForeignKey(
                        name: "FK_Contacts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId");
                });

            migrationBuilder.CreateTable(
                name: "CustomerDetails",
                columns: table => new
                {
                    CustomerDetailId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<string>(type: "character varying(50)", nullable: true),
                    BrandName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    BrandAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    BrandCity = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    BrandDistrict = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    BrandVillage = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    BrandStatus = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    BrandAge = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    BrandType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    BrandingName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Desc = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    BrandPhotoUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    BrandPhotoUrl1 = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    BrandLatitude = table.Column<double>(type: "double precision", nullable: true),
                    BrandLongitude = table.Column<double>(type: "double precision", nullable: true),
                    Reserved = table.Column<string>(type: "text", nullable: true),
                    BrandPhotoBlobId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    brandPhoto1BlobId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDetails", x => x.CustomerDetailId);
                    table.ForeignKey(
                        name: "FK_CustomerDetails_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId");
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    AssignmentId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AssignmentCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AssignmentName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AssignmentStatusCode = table.Column<string>(type: "character varying(20)", nullable: false),
                    RefAssignmentCode = table.Column<string>(type: "text", nullable: true),
                    AssignmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AssignmentDueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Remarks = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AssignmentType = table.Column<int>(type: "integer", nullable: false),
                    RefAssigmentCode = table.Column<string>(type: "character varying(6)", nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<byte>(type: "smallint", nullable: false),
                    IsDeleted = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.AssignmentId);
                    table.ForeignKey(
                        name: "FK_Assignments_AssignmentStatuses_AssignmentStatusCode",
                        column: x => x.AssignmentStatusCode,
                        principalTable: "AssignmentStatuses",
                        principalColumn: "AssignmentStatusCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_RefAssigments_RefAssigmentCode",
                        column: x => x.RefAssigmentCode,
                        principalTable: "RefAssigments",
                        principalColumn: "Code");
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CurrentToken = table.Column<string>(type: "text", nullable: true),
                    RoleCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RoleId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LastLoginDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsPushNotifActive = table.Column<bool>(type: "boolean", nullable: false),
                    FCMToken = table.Column<string>(type: "text", nullable: true),
                    DeviceId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<byte>(type: "smallint", nullable: false),
                    IsDeleted = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Accounts_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    InvoiceCode = table.Column<string>(type: "text", nullable: true),
                    AssignmentCode = table.Column<string>(type: "text", nullable: true),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AssignmentId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<byte>(type: "smallint", nullable: false),
                    IsDeleted = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_Invoices_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "AssignmentId");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AssignmentCode = table.Column<string>(type: "text", nullable: true),
                    AssignmentId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ProductCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ProductName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    ProductAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    Discount = table.Column<double>(type: "double precision", nullable: false),
                    CustomerCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CustomerId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<byte>(type: "smallint", nullable: false),
                    IsDeleted = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "AssignmentId");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    InvoiceCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AssignmentCode = table.Column<string>(type: "text", nullable: true),
                    AssignmentId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    InvoiceAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    PaymentDebt = table.Column<decimal>(type: "numeric", nullable: false),
                    PaymentChannel = table.Column<int>(type: "integer", nullable: false),
                    TransferBank = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    TransferAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    TransferDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GiroNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    GiroPhoto = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    GiroAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    GiroDueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CashAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    BlobName = table.Column<string>(type: "text", nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<byte>(type: "smallint", nullable: false),
                    IsDeleted = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "AssignmentId");
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    UserLoginId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    GeneratedToken = table.Column<string>(type: "text", nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.UserLoginId);
                    table.ForeignKey(
                        name: "FK_UserLogins_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AccountId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UserCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    UserName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UserEmail = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UserPhone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<byte>(type: "smallint", nullable: false),
                    IsDeleted = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    AnswerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssignmentCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AssignmentId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UserId = table.Column<string>(type: "character varying(50)", nullable: false),
                    SurveyId = table.Column<string>(type: "character varying(50)", nullable: false),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<byte>(type: "smallint", nullable: false),
                    IsDeleted = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.AnswerId);
                    table.ForeignKey(
                        name: "FK_Answers_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "AssignmentId");
                    table.ForeignKey(
                        name: "FK_Answers_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "SurveyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Answers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentDetails",
                columns: table => new
                {
                    AssignmentDetailId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AssignmentId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UserId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UserCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ContactId = table.Column<string>(type: "character varying(50)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LostTime = table.Column<int>(type: "integer", nullable: false),
                    SalesTime = table.Column<int>(type: "integer", nullable: false),
                    GoogleTime = table.Column<int>(type: "integer", nullable: false),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    Remarks = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Attachment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AttachmentBlobId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Attachment1 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AttachmentBlobId1 = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Attachment2 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AttachmentBlobId2 = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<byte>(type: "smallint", nullable: false),
                    IsDeleted = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentDetails", x => x.AssignmentDetailId);
                    table.ForeignKey(
                        name: "FK_AssignmentDetails_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "AssignmentId");
                    table.ForeignKey(
                        name: "FK_AssignmentDetails_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "ContactId");
                    table.ForeignKey(
                        name: "FK_AssignmentDetails_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "AssignmentGroups",
                columns: table => new
                {
                    AssignmentGroupId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StartLongitude = table.Column<double>(type: "double precision", nullable: true),
                    StartLatitude = table.Column<double>(type: "double precision", nullable: true),
                    EndLatitude = table.Column<double>(type: "double precision", nullable: true),
                    EndLongitude = table.Column<double>(type: "double precision", nullable: true),
                    TotalLostTime = table.Column<int>(type: "integer", nullable: false),
                    StartDistance = table.Column<double>(type: "double precision", nullable: false),
                    EndDistance = table.Column<double>(type: "double precision", nullable: false),
                    Reserved1 = table.Column<string>(type: "text", nullable: true),
                    Reserved2 = table.Column<string>(type: "text", nullable: true),
                    Reserved3 = table.Column<string>(type: "text", nullable: true),
                    Reserved4 = table.Column<string>(type: "text", nullable: true),
                    Reserved5 = table.Column<string>(type: "text", nullable: true),
                    Reserved6 = table.Column<string>(type: "text", nullable: true),
                    Reserved7 = table.Column<string>(type: "text", nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentGroups", x => x.AssignmentGroupId);
                    table.ForeignKey(
                        name: "FK_AssignmentGroups_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserActivities",
                columns: table => new
                {
                    UserActivityId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssignmentId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    ActivityTypeEnum = table.Column<int>(type: "integer", nullable: false),
                    AssignmentStatusCode = table.Column<string>(type: "text", nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivities", x => x.UserActivityId);
                    table.ForeignKey(
                        name: "FK_UserActivities_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "AssignmentId");
                    table.ForeignKey(
                        name: "FK_UserActivities_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTerritories",
                columns: table => new
                {
                    UserTerritoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "character varying(50)", nullable: true),
                    TerritoryId = table.Column<string>(type: "character varying(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTerritories", x => x.UserTerritoryId);
                    table.ForeignKey(
                        name: "FK_UserTerritories_Territories_TerritoryId",
                        column: x => x.TerritoryId,
                        principalTable: "Territories",
                        principalColumn: "TerritoryId");
                    table.ForeignKey(
                        name: "FK_UserTerritories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_RoleId",
                table: "Accounts",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_AssignmentId",
                table: "Answers",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_SurveyId",
                table: "Answers",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_UserId",
                table: "Answers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentDetails_AssignmentId",
                table: "AssignmentDetails",
                column: "AssignmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentDetails_ContactId",
                table: "AssignmentDetails",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentDetails_UserId",
                table: "AssignmentDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentGroups_CreatedBy",
                table: "AssignmentGroups",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssignmentStatusCode",
                table: "Assignments",
                column: "AssignmentStatusCode");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_RefAssigmentCode",
                table: "Assignments",
                column: "RefAssigmentCode");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_CustomerId",
                table: "Contacts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDetails_CustomerId",
                table: "CustomerDetails",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_AssignmentId",
                table: "Invoices",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AssignmentId",
                table: "Orders",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AssignmentId",
                table: "Payments",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_AssignmentId",
                table: "UserActivities",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_CreatedBy",
                table: "UserActivities",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_AccountId",
                table: "UserLogins",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AccountId",
                table: "Users",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTerritories_TerritoryId",
                table: "UserTerritories",
                column: "TerritoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTerritories_UserId",
                table: "UserTerritories",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "AssignmentDetails");

            migrationBuilder.DropTable(
                name: "AssignmentGroups");

            migrationBuilder.DropTable(
                name: "CustomerContactAgents");

            migrationBuilder.DropTable(
                name: "CustomerDetails");

            migrationBuilder.DropTable(
                name: "DataProtectionKeys");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Otps");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "UserActivities");

            migrationBuilder.DropTable(
                name: "UserLocations");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserTerritories");

            migrationBuilder.DropTable(
                name: "Surveys");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "Territories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "AssignmentStatuses");

            migrationBuilder.DropTable(
                name: "RefAssigments");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
