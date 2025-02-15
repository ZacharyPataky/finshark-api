using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FinShark.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedRole2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8f8b8060-5d4a-44bc-84f1-5b21684a2bcd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fd586c27-6006-4725-aeb1-ba0d670715cb");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "25622371-0be0-48f0-aaf6-a40661b14606", null, "Admin", "ADMIN" },
                    { "423c81a5-f153-46fe-a4fc-3456024e640d", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "25622371-0be0-48f0-aaf6-a40661b14606");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "423c81a5-f153-46fe-a4fc-3456024e640d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8f8b8060-5d4a-44bc-84f1-5b21684a2bcd", null, "User", "USER" },
                    { "fd586c27-6006-4725-aeb1-ba0d670715cb", null, "Admin", "ADMIN" }
                });
        }
    }
}
