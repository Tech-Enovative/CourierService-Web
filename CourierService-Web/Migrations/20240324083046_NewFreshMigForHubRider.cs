using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class NewFreshMigForHubRider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Due",
                table: "riderPayments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HubReceived",
                table: "riderPayments",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 24, 14, 30, 42, 921, DateTimeKind.Local).AddTicks(3943));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 24, 14, 30, 42, 921, DateTimeKind.Local).AddTicks(3842));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 24, 14, 30, 42, 921, DateTimeKind.Local).AddTicks(4017));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Due",
                table: "riderPayments");

            migrationBuilder.DropColumn(
                name: "HubReceived",
                table: "riderPayments");

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 20, 12, 16, 982, DateTimeKind.Local).AddTicks(5176));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 20, 12, 16, 982, DateTimeKind.Local).AddTicks(5120));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 20, 12, 16, 982, DateTimeKind.Local).AddTicks(5210));
        }
    }
}
