using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eKirana.Migrations
{
    /// <inheritdoc />
    public partial class StockQuantityHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPurchaseRates_Products_ProductId",
                table: "ProductPurchaseRates");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPurchaseRates_Units_UnitId",
                table: "ProductPurchaseRates");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "ProductSaleRates",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<long>(
                name: "Stock_Quantity",
                table: "ProductQuantityUnitRates",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "UnitId",
                table: "ProductPurchaseRates",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "ProductPurchaseRates",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "StockQuantityHistories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductQuantityUnitRateId = table.Column<long>(type: "bigint", nullable: false),
                    QuantityMovement = table.Column<long>(type: "bigint", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockQuantityHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockQuantityHistories_ProductQuantityUnitRates_ProductQuantityUnitRateId",
                        column: x => x.ProductQuantityUnitRateId,
                        principalTable: "ProductQuantityUnitRates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockQuantityHistories_ProductQuantityUnitRateId",
                table: "StockQuantityHistories",
                column: "ProductQuantityUnitRateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPurchaseRates_Products_ProductId",
                table: "ProductPurchaseRates",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPurchaseRates_Units_UnitId",
                table: "ProductPurchaseRates",
                column: "UnitId",
                principalTable: "Units",
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
                name: "FK_ProductPurchaseRates_Units_UnitId",
                table: "ProductPurchaseRates");

            migrationBuilder.DropTable(
                name: "StockQuantityHistories");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "ProductSaleRates",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Stock_Quantity",
                table: "ProductQuantityUnitRates",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "UnitId",
                table: "ProductPurchaseRates",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPurchaseRates_Units_UnitId",
                table: "ProductPurchaseRates",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id");
        }
    }
}
