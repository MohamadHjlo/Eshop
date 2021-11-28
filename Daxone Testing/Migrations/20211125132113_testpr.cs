using Microsoft.EntityFrameworkCore.Migrations;

namespace Daxone_Testing.Migrations
{
    public partial class testpr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FavoriteToProducts_ProductId",
                table: "FavoriteToProducts");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteToProducts_ProductId",
                table: "FavoriteToProducts",
                column: "ProductId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FavoriteToProducts_ProductId",
                table: "FavoriteToProducts");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteToProducts_ProductId",
                table: "FavoriteToProducts",
                column: "ProductId");
        }
    }
}
