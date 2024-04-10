using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eKirana.Migrations
{
    /// <inheritdoc />
    public partial class ProductQuantityUnitRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductStockQuantities");

            migrationBuilder.CreateTable(
                name: "ProductQuantityUnitRates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Stock_Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    IsBaseUnit = table.Column<bool>(type: "bit", nullable: false),
                    Ratio = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductQuantityUnitRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductQuantityUnitRates_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductQuantityUnitRates_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductQuantityUnitRates_ProductId",
                table: "ProductQuantityUnitRates",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductQuantityUnitRates_UnitId",
                table: "ProductQuantityUnitRates",
                column: "UnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductQuantityUnitRates");

            migrationBuilder.CreateTable(
                name: "ProductStockQuantities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    Stock_Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStockQuantities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductStockQuantities_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductStockQuantities_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductStockQuantities_ProductId",
                table: "ProductStockQuantities",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStockQuantities_UnitId",
                table: "ProductStockQuantities",
                column: "UnitId");
        }
    }
}
