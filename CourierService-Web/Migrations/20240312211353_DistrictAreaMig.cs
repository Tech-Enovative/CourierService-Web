using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class DistrictAreaMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Hubs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                columns: new[] { "Area", "CreatedAt", "District" },
                values: new object[] { "Mirpur", new DateTime(2024, 3, 13, 3, 13, 53, 57, DateTimeKind.Local).AddTicks(4598), "Dhaka" });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "District",
                table: "Hubs");

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                columns: new[] { "Area", "CreatedAt" },
                values: new object[] { "Dhaka", new DateTime(2024, 3, 13, 1, 27, 6, 732, DateTimeKind.Local).AddTicks(3915) });

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 13, 1, 27, 6, 732, DateTimeKind.Local).AddTicks(3824));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 13, 1, 27, 6, 732, DateTimeKind.Local).AddTicks(3983));
        }
    }
}
