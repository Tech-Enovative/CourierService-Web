using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class NewMigArea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Area",
                table: "Merchants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 13, 1, 27, 6, 732, DateTimeKind.Local).AddTicks(3915));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                columns: new[] { "Area", "CreatedAt" },
                values: new object[] { "Mirpur", new DateTime(2024, 3, 13, 1, 27, 6, 732, DateTimeKind.Local).AddTicks(3824) });

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 13, 1, 27, 6, 732, DateTimeKind.Local).AddTicks(3983));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Area",
                table: "Merchants",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 12, 20, 50, 55, 994, DateTimeKind.Local).AddTicks(2367));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                columns: new[] { "Area", "CreatedAt" },
                values: new object[] { null, new DateTime(2024, 3, 12, 20, 50, 55, 994, DateTimeKind.Local).AddTicks(2313) });

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 12, 20, 50, 55, 994, DateTimeKind.Local).AddTicks(2404));
        }
    }
}
