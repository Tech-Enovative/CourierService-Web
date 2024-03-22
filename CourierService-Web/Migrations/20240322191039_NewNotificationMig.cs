using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class NewNotificationMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "ParcelId",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ParcelId",
                table: "Notifications",
                column: "ParcelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Parcels_ParcelId",
                table: "Notifications",
                column: "ParcelId",
                principalTable: "Parcels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Parcels_ParcelId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ParcelId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ParcelId",
                table: "Notifications");

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 22, 22, 6, 19, 482, DateTimeKind.Local).AddTicks(4998));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 22, 22, 6, 19, 482, DateTimeKind.Local).AddTicks(4920));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 22, 22, 6, 19, 482, DateTimeKind.Local).AddTicks(5066));
        }
    }
}
