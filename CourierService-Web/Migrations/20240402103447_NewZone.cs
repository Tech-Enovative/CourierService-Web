using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class NewZone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hubs_Zone_ZoneId",
                table: "Hubs");

            migrationBuilder.DropIndex(
                name: "IX_Hubs_ZoneId",
                table: "Hubs");

            migrationBuilder.DropColumn(
                name: "ZoneId",
                table: "Hubs");

            migrationBuilder.AddColumn<string>(
                name: "HubId",
                table: "Zone",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Zone",
                keyColumn: "Id",
                keyValue: "ZONE-123",
                column: "HubId",
                value: "HUB-123");

            migrationBuilder.CreateIndex(
                name: "IX_Zone_HubId",
                table: "Zone",
                column: "HubId");

            migrationBuilder.AddForeignKey(
                name: "FK_Zone_Hubs_HubId",
                table: "Zone",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zone_Hubs_HubId",
                table: "Zone");

            migrationBuilder.DropIndex(
                name: "IX_Zone_HubId",
                table: "Zone");

            migrationBuilder.DropColumn(
                name: "HubId",
                table: "Zone");

            migrationBuilder.AddColumn<string>(
                name: "ZoneId",
                table: "Hubs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "HUB-123",
                column: "ZoneId",
                value: "ZONE-123");

            migrationBuilder.CreateIndex(
                name: "IX_Hubs_ZoneId",
                table: "Hubs",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hubs_Zone_ZoneId",
                table: "Hubs",
                column: "ZoneId",
                principalTable: "Zone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
