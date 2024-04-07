using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eKirana.Migrations
{
    /// <inheritdoc />
    public partial class LastTransactionAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "PurchaseDetails",
                newName: "NetAmount");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastTransaction",
                table: "Suppliers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastTransaction",
                table: "Suppliers");

            migrationBuilder.RenameColumn(
                name: "NetAmount",
                table: "PurchaseDetails",
                newName: "TotalAmount");
        }
    }
}
