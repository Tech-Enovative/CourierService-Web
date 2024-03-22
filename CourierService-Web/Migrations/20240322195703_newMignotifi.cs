using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class newMignotifi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MerchantId",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestedPrice",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RiderId",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 1, 57, 2, 471, DateTimeKind.Local).AddTicks(548));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 1, 57, 2, 471, DateTimeKind.Local).AddTicks(491));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 1, 57, 2, 471, DateTimeKind.Local).AddTicks(593));

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_MerchantId",
                table: "Notifications",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RiderId",
                table: "Notifications",
                column: "RiderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Merchants_MerchantId",
                table: "Notifications",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Riders_RiderId",
                table: "Notifications",
                column: "RiderId",
                principalTable: "Riders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Merchants_MerchantId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Riders_RiderId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_MerchantId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_RiderId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "RequestedPrice",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "RiderId",
                table: "Notifications");

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 1, 10, 37, 190, DateTimeKind.Local).AddTicks(487));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 1, 10, 37, 190, DateTimeKind.Local).AddTicks(422));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 1, 10, 37, 190, DateTimeKind.Local).AddTicks(536));
        }
    }
}
