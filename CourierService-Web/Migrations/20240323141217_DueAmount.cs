using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class DueAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HubPayments_Hubs_HubId",
                table: "HubPayments");

            migrationBuilder.AlterColumn<int>(
                name: "TotalAmount",
                table: "HubPayments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "HubId",
                table: "HubPayments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "AmountReceived",
                table: "HubPayments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DueAmount",
                table: "HubPayments",
                type: "int",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_HubPayments_Hubs_HubId",
                table: "HubPayments",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HubPayments_Hubs_HubId",
                table: "HubPayments");

            migrationBuilder.DropColumn(
                name: "DueAmount",
                table: "HubPayments");

            migrationBuilder.AlterColumn<int>(
                name: "TotalAmount",
                table: "HubPayments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HubId",
                table: "HubPayments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AmountReceived",
                table: "HubPayments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 20, 1, 45, 417, DateTimeKind.Local).AddTicks(1480));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 20, 1, 45, 417, DateTimeKind.Local).AddTicks(1404));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 20, 1, 45, 417, DateTimeKind.Local).AddTicks(1539));

            migrationBuilder.AddForeignKey(
                name: "FK_HubPayments_Hubs_HubId",
                table: "HubPayments",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
