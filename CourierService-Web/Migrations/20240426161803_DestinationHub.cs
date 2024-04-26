using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class DestinationHub : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessControllers_AccessGroups_AccessGroupId",
                table: "AccessControllers");

            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Zone_ZoneId",
                table: "Areas");

            migrationBuilder.DropForeignKey(
                name: "FK_Complain_Merchants_MerchantId",
                table: "Complain");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveredParcels_Hubs_HubId",
                table: "DeliveredParcels");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveredParcels_Merchants_MerchantId",
                table: "DeliveredParcels");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveredParcels_Riders_RiderId",
                table: "DeliveredParcels");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeParcels_Hubs_HubId",
                table: "ExchangeParcels");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeParcels_Merchants_MerchantId",
                table: "ExchangeParcels");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeParcels_Riders_RiderId",
                table: "ExchangeParcels");

            migrationBuilder.DropForeignKey(
                name: "FK_HubPayments_Hubs_HubId",
                table: "HubPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_Hubs_District_DistrictId",
                table: "Hubs");

            migrationBuilder.DropForeignKey(
                name: "FK_MerchantPayments_HubPayments_HubPaymentId",
                table: "MerchantPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_MerchantPayments_Merchants_MerchantId",
                table: "MerchantPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_Merchants_Hubs_HubId",
                table: "Merchants");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationsPermission_Parcels_ParcelId",
                table: "NotificationsPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_Areas_AreaId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_DeliveredParcels_DeliveryId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_District_DistrictId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_ExchangeParcels_ExchangeId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_Hubs_HubId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_Merchants_MerchantId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_ReturnParcels_ReturnId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_Riders_RiderId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_Stores_StoreId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_Zone_ZoneId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Parcels_ParcelId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnParcels_Hubs_HubId",
                table: "ReturnParcels");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnParcels_Merchants_MerchantId",
                table: "ReturnParcels");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnParcels_Riders_RiderId",
                table: "ReturnParcels");

            migrationBuilder.DropForeignKey(
                name: "FK_riderPayments_HubPayments_HubPaymentId",
                table: "riderPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_riderPayments_Parcels_ParcelId",
                table: "riderPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_riderPayments_Riders_RiderId",
                table: "riderPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_Riders_Hubs_HubId",
                table: "Riders");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Areas_AreaId",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_District_DistrictId",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Hubs_HubId",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Merchants_MerchantId",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Zone_ZoneId",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_Zone_Hubs_HubId",
                table: "Zone");

            migrationBuilder.AddColumn<string>(
                name: "DestinationHubId",
                table: "Parcels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AccessControllers_AccessGroups_AccessGroupId",
                table: "AccessControllers",
                column: "AccessGroupId",
                principalTable: "AccessGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Zone_ZoneId",
                table: "Areas",
                column: "ZoneId",
                principalTable: "Zone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Complain_Merchants_MerchantId",
                table: "Complain",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveredParcels_Hubs_HubId",
                table: "DeliveredParcels",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveredParcels_Merchants_MerchantId",
                table: "DeliveredParcels",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveredParcels_Riders_RiderId",
                table: "DeliveredParcels",
                column: "RiderId",
                principalTable: "Riders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeParcels_Hubs_HubId",
                table: "ExchangeParcels",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeParcels_Merchants_MerchantId",
                table: "ExchangeParcels",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeParcels_Riders_RiderId",
                table: "ExchangeParcels",
                column: "RiderId",
                principalTable: "Riders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HubPayments_Hubs_HubId",
                table: "HubPayments",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Hubs_District_DistrictId",
                table: "Hubs",
                column: "DistrictId",
                principalTable: "District",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MerchantPayments_HubPayments_HubPaymentId",
                table: "MerchantPayments",
                column: "HubPaymentId",
                principalTable: "HubPayments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MerchantPayments_Merchants_MerchantId",
                table: "MerchantPayments",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Merchants_Hubs_HubId",
                table: "Merchants",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationsPermission_Parcels_ParcelId",
                table: "NotificationsPermission",
                column: "ParcelId",
                principalTable: "Parcels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_Areas_AreaId",
                table: "Parcels",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_DeliveredParcels_DeliveryId",
                table: "Parcels",
                column: "DeliveryId",
                principalTable: "DeliveredParcels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_District_DistrictId",
                table: "Parcels",
                column: "DistrictId",
                principalTable: "District",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_ExchangeParcels_ExchangeId",
                table: "Parcels",
                column: "ExchangeId",
                principalTable: "ExchangeParcels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_Hubs_HubId",
                table: "Parcels",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_Merchants_MerchantId",
                table: "Parcels",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_ReturnParcels_ReturnId",
                table: "Parcels",
                column: "ReturnId",
                principalTable: "ReturnParcels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_Riders_RiderId",
                table: "Parcels",
                column: "RiderId",
                principalTable: "Riders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_Stores_StoreId",
                table: "Parcels",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_Zone_ZoneId",
                table: "Parcels",
                column: "ZoneId",
                principalTable: "Zone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Parcels_ParcelId",
                table: "Payments",
                column: "ParcelId",
                principalTable: "Parcels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnParcels_Hubs_HubId",
                table: "ReturnParcels",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnParcels_Merchants_MerchantId",
                table: "ReturnParcels",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnParcels_Riders_RiderId",
                table: "ReturnParcels",
                column: "RiderId",
                principalTable: "Riders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_riderPayments_HubPayments_HubPaymentId",
                table: "riderPayments",
                column: "HubPaymentId",
                principalTable: "HubPayments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_riderPayments_Parcels_ParcelId",
                table: "riderPayments",
                column: "ParcelId",
                principalTable: "Parcels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_riderPayments_Riders_RiderId",
                table: "riderPayments",
                column: "RiderId",
                principalTable: "Riders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Riders_Hubs_HubId",
                table: "Riders",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Areas_AreaId",
                table: "Stores",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_District_DistrictId",
                table: "Stores",
                column: "DistrictId",
                principalTable: "District",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Hubs_HubId",
                table: "Stores",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Merchants_MerchantId",
                table: "Stores",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Zone_ZoneId",
                table: "Stores",
                column: "ZoneId",
                principalTable: "Zone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Zone_Hubs_HubId",
                table: "Zone",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessControllers_AccessGroups_AccessGroupId",
                table: "AccessControllers");

            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Zone_ZoneId",
                table: "Areas");

            migrationBuilder.DropForeignKey(
                name: "FK_Complain_Merchants_MerchantId",
                table: "Complain");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveredParcels_Hubs_HubId",
                table: "DeliveredParcels");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveredParcels_Merchants_MerchantId",
                table: "DeliveredParcels");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveredParcels_Riders_RiderId",
                table: "DeliveredParcels");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeParcels_Hubs_HubId",
                table: "ExchangeParcels");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeParcels_Merchants_MerchantId",
                table: "ExchangeParcels");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeParcels_Riders_RiderId",
                table: "ExchangeParcels");

            migrationBuilder.DropForeignKey(
                name: "FK_HubPayments_Hubs_HubId",
                table: "HubPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_Hubs_District_DistrictId",
                table: "Hubs");

            migrationBuilder.DropForeignKey(
                name: "FK_MerchantPayments_HubPayments_HubPaymentId",
                table: "MerchantPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_MerchantPayments_Merchants_MerchantId",
                table: "MerchantPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_Merchants_Hubs_HubId",
                table: "Merchants");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationsPermission_Parcels_ParcelId",
                table: "NotificationsPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_Areas_AreaId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_DeliveredParcels_DeliveryId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_District_DistrictId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_ExchangeParcels_ExchangeId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_Hubs_HubId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_Merchants_MerchantId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_ReturnParcels_ReturnId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_Riders_RiderId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_Stores_StoreId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_Zone_ZoneId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Parcels_ParcelId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnParcels_Hubs_HubId",
                table: "ReturnParcels");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnParcels_Merchants_MerchantId",
                table: "ReturnParcels");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnParcels_Riders_RiderId",
                table: "ReturnParcels");

            migrationBuilder.DropForeignKey(
                name: "FK_riderPayments_HubPayments_HubPaymentId",
                table: "riderPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_riderPayments_Parcels_ParcelId",
                table: "riderPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_riderPayments_Riders_RiderId",
                table: "riderPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_Riders_Hubs_HubId",
                table: "Riders");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Areas_AreaId",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_District_DistrictId",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Hubs_HubId",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Merchants_MerchantId",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Zone_ZoneId",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_Zone_Hubs_HubId",
                table: "Zone");

            migrationBuilder.DropColumn(
                name: "DestinationHubId",
                table: "Parcels");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessControllers_AccessGroups_AccessGroupId",
                table: "AccessControllers",
                column: "AccessGroupId",
                principalTable: "AccessGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Zone_ZoneId",
                table: "Areas",
                column: "ZoneId",
                principalTable: "Zone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Complain_Merchants_MerchantId",
                table: "Complain",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveredParcels_Hubs_HubId",
                table: "DeliveredParcels",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveredParcels_Merchants_MerchantId",
                table: "DeliveredParcels",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveredParcels_Riders_RiderId",
                table: "DeliveredParcels",
                column: "RiderId",
                principalTable: "Riders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeParcels_Hubs_HubId",
                table: "ExchangeParcels",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeParcels_Merchants_MerchantId",
                table: "ExchangeParcels",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeParcels_Riders_RiderId",
                table: "ExchangeParcels",
                column: "RiderId",
                principalTable: "Riders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HubPayments_Hubs_HubId",
                table: "HubPayments",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Hubs_District_DistrictId",
                table: "Hubs",
                column: "DistrictId",
                principalTable: "District",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MerchantPayments_HubPayments_HubPaymentId",
                table: "MerchantPayments",
                column: "HubPaymentId",
                principalTable: "HubPayments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MerchantPayments_Merchants_MerchantId",
                table: "MerchantPayments",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Merchants_Hubs_HubId",
                table: "Merchants",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationsPermission_Parcels_ParcelId",
                table: "NotificationsPermission",
                column: "ParcelId",
                principalTable: "Parcels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_Areas_AreaId",
                table: "Parcels",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_DeliveredParcels_DeliveryId",
                table: "Parcels",
                column: "DeliveryId",
                principalTable: "DeliveredParcels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_District_DistrictId",
                table: "Parcels",
                column: "DistrictId",
                principalTable: "District",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_ExchangeParcels_ExchangeId",
                table: "Parcels",
                column: "ExchangeId",
                principalTable: "ExchangeParcels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_Hubs_HubId",
                table: "Parcels",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_Merchants_MerchantId",
                table: "Parcels",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_ReturnParcels_ReturnId",
                table: "Parcels",
                column: "ReturnId",
                principalTable: "ReturnParcels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_Riders_RiderId",
                table: "Parcels",
                column: "RiderId",
                principalTable: "Riders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_Stores_StoreId",
                table: "Parcels",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_Zone_ZoneId",
                table: "Parcels",
                column: "ZoneId",
                principalTable: "Zone",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Parcels_ParcelId",
                table: "Payments",
                column: "ParcelId",
                principalTable: "Parcels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnParcels_Hubs_HubId",
                table: "ReturnParcels",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnParcels_Merchants_MerchantId",
                table: "ReturnParcels",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnParcels_Riders_RiderId",
                table: "ReturnParcels",
                column: "RiderId",
                principalTable: "Riders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_riderPayments_HubPayments_HubPaymentId",
                table: "riderPayments",
                column: "HubPaymentId",
                principalTable: "HubPayments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_riderPayments_Parcels_ParcelId",
                table: "riderPayments",
                column: "ParcelId",
                principalTable: "Parcels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_riderPayments_Riders_RiderId",
                table: "riderPayments",
                column: "RiderId",
                principalTable: "Riders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Riders_Hubs_HubId",
                table: "Riders",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Areas_AreaId",
                table: "Stores",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_District_DistrictId",
                table: "Stores",
                column: "DistrictId",
                principalTable: "District",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Hubs_HubId",
                table: "Stores",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Merchants_MerchantId",
                table: "Stores",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Zone_ZoneId",
                table: "Stores",
                column: "ZoneId",
                principalTable: "Zone",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Zone_Hubs_HubId",
                table: "Zone",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "Id");
        }
    }
}
