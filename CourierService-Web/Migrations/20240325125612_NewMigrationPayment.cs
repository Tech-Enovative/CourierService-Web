using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class NewMigrationPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MerchantPayments_Merchants_MerchantId",
                table: "MerchantPayments");

            migrationBuilder.AlterColumn<string>(
                name: "MerchantId",
                table: "MerchantPayments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 18, 56, 9, 922, DateTimeKind.Local).AddTicks(8460));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 18, 56, 9, 922, DateTimeKind.Local).AddTicks(8391));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 18, 56, 9, 922, DateTimeKind.Local).AddTicks(8494));

            migrationBuilder.AddForeignKey(
                name: "FK_MerchantPayments_Merchants_MerchantId",
                table: "MerchantPayments",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MerchantPayments_Merchants_MerchantId",
                table: "MerchantPayments");

            migrationBuilder.AlterColumn<string>(
                name: "MerchantId",
                table: "MerchantPayments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 0, 4, 39, 436, DateTimeKind.Local).AddTicks(5371));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 0, 4, 39, 436, DateTimeKind.Local).AddTicks(5313));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 0, 4, 39, 436, DateTimeKind.Local).AddTicks(5412));

            migrationBuilder.AddForeignKey(
                name: "FK_MerchantPayments_Merchants_MerchantId",
                table: "MerchantPayments",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
