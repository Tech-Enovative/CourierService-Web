using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class NullableHubId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Riders_Hubs_HubId",
                table: "Riders");

            migrationBuilder.AlterColumn<string>(
                name: "HubId",
                table: "Riders",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 11, 22, 26, 51, 906, DateTimeKind.Local).AddTicks(4475));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 11, 22, 26, 51, 906, DateTimeKind.Local).AddTicks(4516));

            migrationBuilder.AddForeignKey(
                name: "FK_Riders_Hubs_HubId",
                table: "Riders",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Riders_Hubs_HubId",
                table: "Riders");

            migrationBuilder.AlterColumn<string>(
                name: "HubId",
                table: "Riders",
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
                value: new DateTime(2024, 3, 11, 22, 2, 35, 577, DateTimeKind.Local).AddTicks(3986));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 11, 22, 2, 35, 577, DateTimeKind.Local).AddTicks(4051));

            migrationBuilder.AddForeignKey(
                name: "FK_Riders_Hubs_HubId",
                table: "Riders",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
