using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class ParcelModelUpdateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CancelDate",
                table: "Parcels",
                newName: "PickedUpAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryRiderAssignedAt",
                table: "Parcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InHubAt",
                table: "Parcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OnTheWayAt",
                table: "Parcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentInHubAt",
                table: "Parcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 30, 22, 5, 12, 430, DateTimeKind.Local).AddTicks(750));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 30, 22, 5, 12, 430, DateTimeKind.Local).AddTicks(699));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 30, 22, 5, 12, 430, DateTimeKind.Local).AddTicks(787));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryRiderAssignedAt",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "InHubAt",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "OnTheWayAt",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "PaymentInHubAt",
                table: "Parcels");

            migrationBuilder.RenameColumn(
                name: "PickedUpAt",
                table: "Parcels",
                newName: "CancelDate");

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 29, 22, 12, 35, 878, DateTimeKind.Local).AddTicks(781));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 29, 22, 12, 35, 878, DateTimeKind.Local).AddTicks(730));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 29, 22, 12, 35, 878, DateTimeKind.Local).AddTicks(814));
        }
    }
}
