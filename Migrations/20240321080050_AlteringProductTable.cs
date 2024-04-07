using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eKirana.Migrations
{
    /// <inheritdoc />
    public partial class AlteringProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductPurchaseRates_ProductPurchaseRateId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductSaleRates_ProductSaleRateId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseDetails_ProductPurchaseRates_ProductPurchaseRateId",
                table: "PurchaseDetails");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseDetails_ProductPurchaseRateId",
                table: "PurchaseDetails");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductPurchaseRateId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductSaleRateId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductPurchaseRateId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductSaleRateId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductPurchaseRateId",
                table: "PurchaseDetails",
                newName: "ProductPurchaseRate");

            migrationBuilder.AddColumn<long>(
                name: "PurchaseById",
                table: "Purchases",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "ProductSaleRates",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "ProductPurchaseRates",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_PurchaseById",
                table: "Purchases",
                column: "PurchaseById");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSaleRates_ProductId",
                table: "ProductSaleRates",
                column: "ProductId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSaleRates_Products_ProductId",
                table: "ProductSaleRates",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Admins_PurchaseById",
                table: "Purchases",
                column: "PurchaseById",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPurchaseRates_Products_ProductId",
                table: "ProductPurchaseRates");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSaleRates_Products_ProductId",
                table: "ProductSaleRates");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Admins_PurchaseById",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_PurchaseById",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_ProductSaleRates_ProductId",
                table: "ProductSaleRates");

            migrationBuilder.DropIndex(
                name: "IX_ProductPurchaseRates_ProductId",
                table: "ProductPurchaseRates");

            migrationBuilder.DropColumn(
                name: "PurchaseById",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductSaleRates");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductPurchaseRates");

            migrationBuilder.RenameColumn(
                name: "ProductPurchaseRate",
                table: "PurchaseDetails",
                newName: "ProductPurchaseRateId");

            migrationBuilder.AddColumn<long>(
                name: "ProductPurchaseRateId",
                table: "Products",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProductSaleRateId",
                table: "Products",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetails_ProductPurchaseRateId",
                table: "PurchaseDetails",
                column: "ProductPurchaseRateId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductPurchaseRateId",
                table: "Products",
                column: "ProductPurchaseRateId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductSaleRateId",
                table: "Products",
                column: "ProductSaleRateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductPurchaseRates_ProductPurchaseRateId",
                table: "Products",
                column: "ProductPurchaseRateId",
                principalTable: "ProductPurchaseRates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductSaleRates_ProductSaleRateId",
                table: "Products",
                column: "ProductSaleRateId",
                principalTable: "ProductSaleRates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseDetails_ProductPurchaseRates_ProductPurchaseRateId",
                table: "PurchaseDetails",
                column: "ProductPurchaseRateId",
                principalTable: "ProductPurchaseRates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
