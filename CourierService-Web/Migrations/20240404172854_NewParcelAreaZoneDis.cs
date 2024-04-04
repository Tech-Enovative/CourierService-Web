using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class NewParcelAreaZoneDis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AreaId",
                table: "Parcels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistrictId",
                table: "Parcels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZoneId",
                table: "Parcels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_AreaId",
                table: "Parcels",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_DistrictId",
                table: "Parcels",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_ZoneId",
                table: "Parcels",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_Areas_AreaId",
                table: "Parcels",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_District_DistrictId",
                table: "Parcels",
                column: "DistrictId",
                principalTable: "District",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_Zone_ZoneId",
                table: "Parcels",
                column: "ZoneId",
                principalTable: "Zone",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_Areas_AreaId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_District_DistrictId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_Zone_ZoneId",
                table: "Parcels");

            migrationBuilder.DropIndex(
                name: "IX_Parcels_AreaId",
                table: "Parcels");

            migrationBuilder.DropIndex(
                name: "IX_Parcels_DistrictId",
                table: "Parcels");

            migrationBuilder.DropIndex(
                name: "IX_Parcels_ZoneId",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "ZoneId",
                table: "Parcels");
        }
    }
}
