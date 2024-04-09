using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eKirana.Migrations
{
    /// <inheritdoc />
    public partial class qum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPurchaseRates_Products_ProductId",
                table: "ProductPurchaseRates");

            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "ProductPurchaseRates",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPurchaseRates_Products_ProductId",
                table: "ProductPurchaseRates",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPurchaseRates_Products_ProductId",
                table: "ProductPurchaseRates");

            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "ProductPurchaseRates",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

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
