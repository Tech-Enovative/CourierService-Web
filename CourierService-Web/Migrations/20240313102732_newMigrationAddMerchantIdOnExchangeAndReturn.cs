using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class newMigrationAddMerchantIdOnExchangeAndReturn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MerchantId",
                table: "ReturnParcels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MerchantId",
                table: "ExchangeParcels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 13, 16, 27, 31, 450, DateTimeKind.Local).AddTicks(3849));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 13, 16, 27, 31, 450, DateTimeKind.Local).AddTicks(3792));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 13, 16, 27, 31, 450, DateTimeKind.Local).AddTicks(3892));

            migrationBuilder.CreateIndex(
                name: "IX_ReturnParcels_MerchantId",
                table: "ReturnParcels",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeParcels_MerchantId",
                table: "ExchangeParcels",
                column: "MerchantId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeParcels_Merchants_MerchantId",
                table: "ExchangeParcels",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnParcels_Merchants_MerchantId",
                table: "ReturnParcels",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeParcels_Merchants_MerchantId",
                table: "ExchangeParcels");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnParcels_Merchants_MerchantId",
                table: "ReturnParcels");

            migrationBuilder.DropIndex(
                name: "IX_ReturnParcels_MerchantId",
                table: "ReturnParcels");

            migrationBuilder.DropIndex(
                name: "IX_ExchangeParcels_MerchantId",
                table: "ExchangeParcels");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "ReturnParcels");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "ExchangeParcels");

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 13, 16, 16, 40, 50, DateTimeKind.Local).AddTicks(2359));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 13, 16, 16, 40, 50, DateTimeKind.Local).AddTicks(2309));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 13, 16, 16, 40, 50, DateTimeKind.Local).AddTicks(2395));
        }
    }
}
