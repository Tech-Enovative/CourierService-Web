using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class fresh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationsPermission_Merchants_MerchantId",
                table: "NotificationsPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationsPermission_Riders_RiderId",
                table: "NotificationsPermission");

            migrationBuilder.DropIndex(
                name: "IX_NotificationsPermission_MerchantId",
                table: "NotificationsPermission");

            migrationBuilder.DropIndex(
                name: "IX_NotificationsPermission_RiderId",
                table: "NotificationsPermission");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "NotificationsPermission");

            migrationBuilder.DropColumn(
                name: "RiderId",
                table: "NotificationsPermission");

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 2, 49, 1, 674, DateTimeKind.Local).AddTicks(1567));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 2, 49, 1, 674, DateTimeKind.Local).AddTicks(1518));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 2, 49, 1, 674, DateTimeKind.Local).AddTicks(1626));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MerchantId",
                table: "NotificationsPermission",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RiderId",
                table: "NotificationsPermission",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 2, 42, 1, 636, DateTimeKind.Local).AddTicks(5641));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 2, 42, 1, 636, DateTimeKind.Local).AddTicks(5579));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 2, 42, 1, 636, DateTimeKind.Local).AddTicks(5686));

            migrationBuilder.CreateIndex(
                name: "IX_NotificationsPermission_MerchantId",
                table: "NotificationsPermission",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationsPermission_RiderId",
                table: "NotificationsPermission",
                column: "RiderId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationsPermission_Merchants_MerchantId",
                table: "NotificationsPermission",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationsPermission_Riders_RiderId",
                table: "NotificationsPermission",
                column: "RiderId",
                principalTable: "Riders",
                principalColumn: "Id");
        }
    }
}
