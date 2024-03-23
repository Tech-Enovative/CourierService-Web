using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class NewModifiParcel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 14, 23, 35, 657, DateTimeKind.Local).AddTicks(5233));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 14, 23, 35, 657, DateTimeKind.Local).AddTicks(5181));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 14, 23, 35, 657, DateTimeKind.Local).AddTicks(5265));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 14, 16, 5, 188, DateTimeKind.Local).AddTicks(9656));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 14, 16, 5, 188, DateTimeKind.Local).AddTicks(9515));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 14, 16, 5, 188, DateTimeKind.Local).AddTicks(9734));
        }
    }
}
