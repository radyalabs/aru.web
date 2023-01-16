using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trisatech.MWorkforce.Domain.Migrations
{
    public partial class AlterTableManualReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "InvoiceValue",
                table: "SalesManualReports",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceValue",
                table: "SalesManualReports");
        }
    }
}
