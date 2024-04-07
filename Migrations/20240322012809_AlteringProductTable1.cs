using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eKirana.Migrations
{
    /// <inheritdoc />
    public partial class AlteringProductTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPurchaseRates_Products_ProductId",
                table: "ProductPurchaseRates");

            migrationBuilder.DropIndex(
                name: "IX_ProductPurchaseRates_ProductId",
                table: "ProductPurchaseRates");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductPurchaseRates");

            migrationBuilder.AddColumn<long>(
                name: "ProductPurchaseRateId",
                table: "Products",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductPurchaseRateId",
                table: "Products",
                column: "ProductPurchaseRateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductPurchaseRates_ProductPurchaseRateId",
                table: "Products",
                column: "ProductPurchaseRateId",
                principalTable: "ProductPurchaseRates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductPurchaseRates_ProductPurchaseRateId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductPurchaseRateId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductPurchaseRateId",
                table: "Products");

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "ProductPurchaseRates",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ProductPurchaseRates_ProductId",
                table: "ProductPurchaseRates",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPurchaseRates_Products_ProductId",
                table: "ProductPurchaseRates",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
