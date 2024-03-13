using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class newRelationExchangeReturnParcel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeParcels_Parcels_ParcelId",
                table: "ExchangeParcels");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnParcels_Parcels_ParcelId",
                table: "ReturnParcels");

            migrationBuilder.DropIndex(
                name: "IX_ReturnParcels_ParcelId",
                table: "ReturnParcels");

            migrationBuilder.DropIndex(
                name: "IX_ExchangeParcels_ParcelId",
                table: "ExchangeParcels");

            migrationBuilder.AlterColumn<string>(
                name: "ParcelId",
                table: "ReturnParcels",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExchangeId",
                table: "Parcels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReturnId",
                table: "Parcels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ParcelId",
                table: "ExchangeParcels",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_ExchangeId",
                table: "Parcels",
                column: "ExchangeId",
                unique: true,
                filter: "[ExchangeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_ReturnId",
                table: "Parcels",
                column: "ReturnId",
                unique: true,
                filter: "[ReturnId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_ExchangeParcels_ExchangeId",
                table: "Parcels",
                column: "ExchangeId",
                principalTable: "ExchangeParcels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_ReturnParcels_ReturnId",
                table: "Parcels",
                column: "ReturnId",
                principalTable: "ReturnParcels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_ExchangeParcels_ExchangeId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_ReturnParcels_ReturnId",
                table: "Parcels");

            migrationBuilder.DropIndex(
                name: "IX_Parcels_ExchangeId",
                table: "Parcels");

            migrationBuilder.DropIndex(
                name: "IX_Parcels_ReturnId",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "ExchangeId",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "ReturnId",
                table: "Parcels");

            migrationBuilder.AlterColumn<string>(
                name: "ParcelId",
                table: "ReturnParcels",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ParcelId",
                table: "ExchangeParcels",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 13, 3, 13, 53, 57, DateTimeKind.Local).AddTicks(4598));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 13, 3, 13, 53, 57, DateTimeKind.Local).AddTicks(4534));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 13, 3, 13, 53, 57, DateTimeKind.Local).AddTicks(4652));

            migrationBuilder.CreateIndex(
                name: "IX_ReturnParcels_ParcelId",
                table: "ReturnParcels",
                column: "ParcelId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeParcels_ParcelId",
                table: "ExchangeParcels",
                column: "ParcelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeParcels_Parcels_ParcelId",
                table: "ExchangeParcels",
                column: "ParcelId",
                principalTable: "Parcels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnParcels_Parcels_ParcelId",
                table: "ReturnParcels",
                column: "ParcelId",
                principalTable: "Parcels",
                principalColumn: "Id");
        }
    }
}
