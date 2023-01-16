using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trisatech.MWorkforce.Domain.Migrations
{
    public partial class SalesManualReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesManualReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReportDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SalesCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SalesName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CustomerCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CustomerName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Cash = table.Column<decimal>(type: "numeric", nullable: false),
                    Giro = table.Column<decimal>(type: "numeric", nullable: false),
                    Transfer = table.Column<decimal>(type: "numeric", nullable: false),
                    Total = table.Column<decimal>(type: "numeric", nullable: false),
                    Jenis = table.Column<int>(type: "integer", nullable: false),
                    Nominal = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<byte>(type: "smallint", nullable: false),
                    IsDeleted = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesManualReports", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesManualReports");
        }
    }
}
