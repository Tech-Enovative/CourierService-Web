using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class NewDis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_District_Zone_ZoneId",
                table: "District");

            migrationBuilder.DropIndex(
                name: "IX_District_ZoneId",
                table: "District");

            migrationBuilder.DropColumn(
                name: "ZoneId",
                table: "District");

            migrationBuilder.AddColumn<string>(
                name: "DistrictId",
                table: "Zone",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Zone",
                keyColumn: "Id",
                keyValue: "ZONE-123",
                column: "DistrictId",
                value: "DIS-123");

            migrationBuilder.CreateIndex(
                name: "IX_Zone_DistrictId",
                table: "Zone",
                column: "DistrictId");

            migrationBuilder.AddForeignKey(
                name: "FK_Zone_District_DistrictId",
                table: "Zone",
                column: "DistrictId",
                principalTable: "District",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zone_District_DistrictId",
                table: "Zone");

            migrationBuilder.DropIndex(
                name: "IX_Zone_DistrictId",
                table: "Zone");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "Zone");

            migrationBuilder.AddColumn<string>(
                name: "ZoneId",
                table: "District",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "District",
                keyColumn: "Id",
                keyValue: "DIS-123",
                column: "ZoneId",
                value: "ZONE-123");

            migrationBuilder.CreateIndex(
                name: "IX_District_ZoneId",
                table: "District",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_District_Zone_ZoneId",
                table: "District",
                column: "ZoneId",
                principalTable: "Zone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
