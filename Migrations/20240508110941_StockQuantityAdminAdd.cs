using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eKirana.Migrations
{
    /// <inheritdoc />
    public partial class StockQuantityAdminAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AdminId",
                table: "StockQuantityHistories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_StockQuantityHistories_AdminId",
                table: "StockQuantityHistories",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockQuantityHistories_Admins_AdminId",
                table: "StockQuantityHistories",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockQuantityHistories_Admins_AdminId",
                table: "StockQuantityHistories");

            migrationBuilder.DropIndex(
                name: "IX_StockQuantityHistories_AdminId",
                table: "StockQuantityHistories");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "StockQuantityHistories");
        }
    }
}
