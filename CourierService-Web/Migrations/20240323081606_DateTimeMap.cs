using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class DateTimeMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "HubDueDate",
                table: "riderPayments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HubReceivedDate",
                table: "riderPayments",
                type: "datetime2",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HubDueDate",
                table: "riderPayments");

            migrationBuilder.DropColumn(
                name: "HubReceivedDate",
                table: "riderPayments");

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 14, 9, 53, 688, DateTimeKind.Local).AddTicks(3721));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 14, 9, 53, 688, DateTimeKind.Local).AddTicks(3660));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 14, 9, 53, 688, DateTimeKind.Local).AddTicks(3774));
        }
    }
}
