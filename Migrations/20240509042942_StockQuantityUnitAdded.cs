using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eKirana.Migrations
{
    /// <inheritdoc />
    public partial class StockQuantityUnitAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "QuantityMovement",
                table: "StockQuantityHistories",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "UnitId",
                table: "StockQuantityHistories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_StockQuantityHistories_UnitId",
                table: "StockQuantityHistories",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockQuantityHistories_Units_UnitId",
                table: "StockQuantityHistories",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockQuantityHistories_Units_UnitId",
                table: "StockQuantityHistories");

            migrationBuilder.DropIndex(
                name: "IX_StockQuantityHistories_UnitId",
                table: "StockQuantityHistories");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "StockQuantityHistories");

            migrationBuilder.AlterColumn<long>(
                name: "QuantityMovement",
                table: "StockQuantityHistories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
