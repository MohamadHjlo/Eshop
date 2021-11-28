using Microsoft.EntityFrameworkCore.Migrations;

namespace Daxone_Testing.Migrations
{
    public partial class ChangePrimaryKeyOfFavToProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteToProducts_UserFavorites_UserFavoritesId",
                table: "FavoriteToProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteToProducts",
                table: "FavoriteToProducts");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteToProducts_UserFavoritesId",
                table: "FavoriteToProducts");

            migrationBuilder.DropColumn(
                name: "FavoriteId",
                table: "FavoriteToProducts");

            migrationBuilder.AlterColumn<int>(
                name: "UserFavoritesId",
                table: "FavoriteToProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteToProducts",
                table: "FavoriteToProducts",
                columns: new[] { "UserFavoritesId", "ProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteToProducts_UserFavorites_UserFavoritesId",
                table: "FavoriteToProducts",
                column: "UserFavoritesId",
                principalTable: "UserFavorites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteToProducts_UserFavorites_UserFavoritesId",
                table: "FavoriteToProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteToProducts",
                table: "FavoriteToProducts");

            migrationBuilder.AlterColumn<int>(
                name: "UserFavoritesId",
                table: "FavoriteToProducts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "FavoriteId",
                table: "FavoriteToProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteToProducts",
                table: "FavoriteToProducts",
                columns: new[] { "FavoriteId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteToProducts_UserFavoritesId",
                table: "FavoriteToProducts",
                column: "UserFavoritesId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteToProducts_UserFavorites_UserFavoritesId",
                table: "FavoriteToProducts",
                column: "UserFavoritesId",
                principalTable: "UserFavorites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
