using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class newseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Zone",
                columns: new[] { "Id", "Name" },
                values: new object[] { "ZONE-123", "Dhaka" });

            migrationBuilder.InsertData(
                table: "District",
                columns: new[] { "Id", "Name", "ZoneId" },
                values: new object[] { "DIS-123", "Dhaka", "ZONE-123" });

            migrationBuilder.InsertData(
                table: "Hubs",
                columns: new[] { "Id", "DistrictId", "Name", "ZoneId" },
                values: new object[] { "HUB-123", "DIS-123", "Mirpur Hub", "ZONE-123" });

            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "DistrictId", "HubId", "Name", "ZoneId" },
                values: new object[] { "AREA-123", "DIS-123", "HUB-123", "Mirpur", "ZONE-123" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: "AREA-123");

            migrationBuilder.DeleteData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "HUB-123");

            migrationBuilder.DeleteData(
                table: "District",
                keyColumn: "Id",
                keyValue: "DIS-123");

            migrationBuilder.DeleteData(
                table: "Zone",
                keyColumn: "Id",
                keyValue: "ZONE-123");
        }
    }
}
