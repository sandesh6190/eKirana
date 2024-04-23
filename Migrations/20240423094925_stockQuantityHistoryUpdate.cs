using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eKirana.Migrations
{
    /// <inheritdoc />
    public partial class stockQuantityHistoryUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "SaleDetails",
                newName: "NetAmount");

            migrationBuilder.AddColumn<DateTime>(
                name: "On_Date",
                table: "StockQuantityHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "Quantity",
                table: "SaleDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "On_Date",
                table: "StockQuantityHistories");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "SaleDetails");

            migrationBuilder.RenameColumn(
                name: "NetAmount",
                table: "SaleDetails",
                newName: "TotalAmount");
        }
    }
}
