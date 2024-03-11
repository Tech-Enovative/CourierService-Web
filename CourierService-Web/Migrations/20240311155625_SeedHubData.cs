using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class SeedHubData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Hubs",
                columns: new[] { "Id", "Address", "AdminId", "Area", "CreatedAt", "CreatedBy", "Email", "Name", "Password", "PhoneNumber", "Status" },
                values: new object[] { "H-123", "Dhaka, Bangladesh", "A-123", "Dhaka", new DateTime(2024, 3, 11, 21, 56, 24, 793, DateTimeKind.Local).AddTicks(2758), "Admin", "hub@gmail.com", "Hub", "1111", "01837730317", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123");
        }
    }
}
