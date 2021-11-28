using Microsoft.EntityFrameworkCore.Migrations;

namespace Daxone_Testing.Migrations
{
    public partial class TestSetHasDataForRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "RankOfRole" },
                values: new object[,]
                {
                    { "cd36392a-fe3e-463b-b669-82aabf8a4539", "7c0630ed-45cb-4b62-8273-54f1b55a0c2a", "Owner", "OWNER", 1 },
                    { "e1abdd14-43ac-46ad-9295-767059570d8f", "144fd754-94c4-46b3-a4fa-f64964ecaef1", "Admin", "ADMIN", 2 },
                    { "c12746c6-fa6d-4816-864f-6a0d9418a2a1", "bb3582d8-fe6f-4beb-b01b-19eff1ff8f72", "Storekeeper", "STOREKEEPER", 3 },
                    { "855981ee-8190-44f4-ba69-df1e21c2635e", "6b5818e3-2a79-48ea-9d6e-98e7667a1002", "User", "USER", 4 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "855981ee-8190-44f4-ba69-df1e21c2635e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c12746c6-fa6d-4816-864f-6a0d9418a2a1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cd36392a-fe3e-463b-b669-82aabf8a4539");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e1abdd14-43ac-46ad-9295-767059570d8f");
        }
    }
}
