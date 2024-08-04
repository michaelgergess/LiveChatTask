using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiveChatTaskMVC.Migrations
{
    /// <inheritdoc />
    public partial class CreateBaseRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert roles into AspNetRoles table
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[,]
                {
                    { "1", "User", "USER", "d8f3c1f7-2b3c-4e5d-8a1e-8d3a7b9c2c3a" },
                    { "2", "Admin", "ADMIN", "a7d8c1b3-2d4e-5f6a-9b1c-8f2a7b8c9d4a" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove roles from AspNetRoles table
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");
        }
    }
}
