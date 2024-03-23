using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class NewMigHub : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HubDue",
                table: "riderPayments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HubReceivedAmount",
                table: "riderPayments",
                type: "int",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HubDue",
                table: "riderPayments");

            migrationBuilder.DropColumn(
                name: "HubReceivedAmount",
                table: "riderPayments");

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 11, 44, 43, 957, DateTimeKind.Local).AddTicks(9805));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 11, 44, 43, 957, DateTimeKind.Local).AddTicks(9712));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 11, 44, 43, 957, DateTimeKind.Local).AddTicks(9867));
        }
    }
}
