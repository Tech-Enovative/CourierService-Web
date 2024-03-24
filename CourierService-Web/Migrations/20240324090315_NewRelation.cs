using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class NewRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HubPaymentId",
                table: "riderPayments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RiderPaymentId",
                table: "HubPayments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 24, 15, 3, 14, 400, DateTimeKind.Local).AddTicks(1247));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 24, 15, 3, 14, 400, DateTimeKind.Local).AddTicks(1197));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 24, 15, 3, 14, 400, DateTimeKind.Local).AddTicks(1288));

            migrationBuilder.CreateIndex(
                name: "IX_HubPayments_RiderPaymentId",
                table: "HubPayments",
                column: "RiderPaymentId",
                unique: true,
                filter: "[RiderPaymentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_HubPayments_riderPayments_RiderPaymentId",
                table: "HubPayments",
                column: "RiderPaymentId",
                principalTable: "riderPayments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HubPayments_riderPayments_RiderPaymentId",
                table: "HubPayments");

            migrationBuilder.DropIndex(
                name: "IX_HubPayments_RiderPaymentId",
                table: "HubPayments");

            migrationBuilder.DropColumn(
                name: "HubPaymentId",
                table: "riderPayments");

            migrationBuilder.DropColumn(
                name: "RiderPaymentId",
                table: "HubPayments");

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
    }
}
