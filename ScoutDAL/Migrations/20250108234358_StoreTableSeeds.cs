using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ScoutDAL.Migrations
{
    /// <inheritdoc />
    public partial class StoreTableSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "Id", "City", "Name", "PhoneNumber", "PostalCode", "Street" },
                values: new object[,]
                {
                    { 1, "Tech City", "Tech Solution", "6669990000", "12121", "123 Tech St" },
                    { 2, "Vid City", "Vivid Books", "7779990000", "66666", "999 Vid St" },
                    { 3, "Lala land", "Readers Club", "1113335555", "99999", "999 Main St" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
