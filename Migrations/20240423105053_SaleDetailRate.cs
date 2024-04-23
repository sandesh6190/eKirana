using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eKirana.Migrations
{
    /// <inheritdoc />
    public partial class SaleDetailRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleDetails_ProductSaleRates_ProductSaleRateId",
                table: "SaleDetails");

            migrationBuilder.DropIndex(
                name: "IX_SaleDetails_ProductSaleRateId",
                table: "SaleDetails");

            migrationBuilder.DropColumn(
                name: "ProductSaleRateId",
                table: "SaleDetails");

            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                table: "SaleDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "SaleDetails");

            migrationBuilder.AddColumn<long>(
                name: "ProductSaleRateId",
                table: "SaleDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_ProductSaleRateId",
                table: "SaleDetails",
                column: "ProductSaleRateId");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleDetails_ProductSaleRates_ProductSaleRateId",
                table: "SaleDetails",
                column: "ProductSaleRateId",
                principalTable: "ProductSaleRates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
