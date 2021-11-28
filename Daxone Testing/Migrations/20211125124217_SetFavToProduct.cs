using Microsoft.EntityFrameworkCore.Migrations;

namespace Daxone_Testing.Migrations
{
    public partial class SetFavToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_UserFavorites_UserFavoritesId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UserFavoritesId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "UserFavorites");

            migrationBuilder.DropColumn(
                name: "UserFavoritesId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "FavoriteToProducts",
                columns: table => new
                {
                    FavoriteId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UserFavoritesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteToProducts", x => new { x.FavoriteId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_FavoriteToProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteToProducts_UserFavorites_UserFavoritesId",
                        column: x => x.UserFavoritesId,
                        principalTable: "UserFavorites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteToProducts_ProductId",
                table: "FavoriteToProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteToProducts_UserFavoritesId",
                table: "FavoriteToProducts",
                column: "UserFavoritesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteToProducts");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "UserFavorites",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserFavoritesId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserFavoritesId",
                table: "Products",
                column: "UserFavoritesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UserFavorites_UserFavoritesId",
                table: "Products",
                column: "UserFavoritesId",
                principalTable: "UserFavorites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
