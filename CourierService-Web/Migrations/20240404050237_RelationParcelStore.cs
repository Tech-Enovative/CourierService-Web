using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class RelationParcelStore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StoreId",
                table: "Parcels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_StoreId",
                table: "Parcels",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_Stores_StoreId",
                table: "Parcels",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_Stores_StoreId",
                table: "Parcels");

            migrationBuilder.DropIndex(
                name: "IX_Parcels_StoreId",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Parcels");
        }
    }
}
