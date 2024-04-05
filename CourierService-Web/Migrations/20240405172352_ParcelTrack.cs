using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class ParcelTrack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InHubAt",
                table: "Parcels",
                newName: "ReturnedAssignedPickupAgentAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "AtTheSortingHubAt",
                table: "Parcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DamagedAt",
                table: "Parcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryFailedAt",
                table: "Parcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryOnHoldAt",
                table: "Parcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OnTheWayToLastMileHubAt",
                table: "Parcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OnTheWayToSortingHubAt",
                table: "Parcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ParcelAtTheHubReceivedAt",
                table: "Parcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PickupCancelledAt",
                table: "Parcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PickupFailedAt",
                table: "Parcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PickupOnHoldAt",
                table: "Parcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceivedAtLastMileHubAt",
                table: "Parcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnCreateFirstMileHubAt",
                table: "Parcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnOnTheWayToFirstMileHubAt",
                table: "Parcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnOnTheWayToMerchantAt",
                table: "Parcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnOnTheWayToSortingHubAt",
                table: "Parcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnReceivedByFirstMileHubAt",
                table: "Parcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnReceivedByMerchantAt",
                table: "Parcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnReceivedBySortingHubAt",
                table: "Parcels",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AtTheSortingHubAt",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "DamagedAt",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "DeliveryFailedAt",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "DeliveryOnHoldAt",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "OnTheWayToLastMileHubAt",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "OnTheWayToSortingHubAt",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "ParcelAtTheHubReceivedAt",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "PickupCancelledAt",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "PickupFailedAt",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "PickupOnHoldAt",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "ReceivedAtLastMileHubAt",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "ReturnCreateFirstMileHubAt",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "ReturnOnTheWayToFirstMileHubAt",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "ReturnOnTheWayToMerchantAt",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "ReturnOnTheWayToSortingHubAt",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "ReturnReceivedByFirstMileHubAt",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "ReturnReceivedByMerchantAt",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "ReturnReceivedBySortingHubAt",
                table: "Parcels");

            migrationBuilder.RenameColumn(
                name: "ReturnedAssignedPickupAgentAt",
                table: "Parcels",
                newName: "InHubAt");
        }
    }
}
