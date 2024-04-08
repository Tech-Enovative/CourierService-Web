using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class MerchantNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "InDhakaEmergencyDeliveryCharge",
                table: "Merchants",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "InsideDhakaDeliveryCharge",
                table: "Merchants",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OutsideDhakaDeliveryCharge",
                table: "Merchants",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "P2PDeliveryCharge",
                table: "Merchants",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SubDhakaDeliveryCharge",
                table: "Merchants",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InDhakaEmergencyDeliveryCharge",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "InsideDhakaDeliveryCharge",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "OutsideDhakaDeliveryCharge",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "P2PDeliveryCharge",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "SubDhakaDeliveryCharge",
                table: "Merchants");
        }
    }
}
