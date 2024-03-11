using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class SeedRiderData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 11, 22, 2, 35, 577, DateTimeKind.Local).AddTicks(3986));

            migrationBuilder.InsertData(
                table: "Riders",
                columns: new[] { "Id", "Area", "ContactNumber", "CreatedAt", "District", "Email", "FullAddress", "HubId", "ImageUrl", "NID", "Name", "Password", "Salary", "State", "Status" },
                values: new object[] { "R-123", "Dhaka", "01837730317", new DateTime(2024, 3, 11, 22, 2, 35, 577, DateTimeKind.Local).AddTicks(4051), "Dhaka", "rider@gmail.com", "Dhaka, Bangladesh", "H-123", null, "0123456789", "Rider", "1111", 10000, "Available", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123");

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 11, 21, 56, 24, 793, DateTimeKind.Local).AddTicks(2758));
        }
    }
}
