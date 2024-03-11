using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class NewDatabaseWithNewModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hubs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hubs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hubs_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Merchants",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacebookUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfirmPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TradeLicense = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HubId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Merchants_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Riders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salary = table.Column<int>(type: "int", nullable: false),
                    NID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HubId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Riders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Riders_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveredParcels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParcelId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RiderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HubId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MerchantId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveredParcels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveredParcels_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeliveredParcels_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeliveredParcels_Riders_RiderId",
                        column: x => x.RiderId,
                        principalTable: "Riders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Parcels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReceiverName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductPrice = table.Column<int>(type: "int", nullable: false),
                    ProductQuantity = table.Column<int>(type: "int", nullable: true),
                    DeliveryCharge = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PickupRequestDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DispatchDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentInHand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PickupLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrice = table.Column<int>(type: "int", nullable: false),
                    MerchantId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RiderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HubId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DeliveryId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parcels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parcels_DeliveredParcels_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "DeliveredParcels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Parcels_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Parcels_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Parcels_Riders_RiderId",
                        column: x => x.RiderId,
                        principalTable: "Riders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExchangeParcels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParcelId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ExchangeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RiderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HubId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeParcels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangeParcels_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExchangeParcels_Parcels_ParcelId",
                        column: x => x.ParcelId,
                        principalTable: "Parcels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExchangeParcels_Riders_RiderId",
                        column: x => x.RiderId,
                        principalTable: "Riders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReturnParcels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParcelId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RiderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HubId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnParcels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnParcels_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReturnParcels_Parcels_ParcelId",
                        column: x => x.ParcelId,
                        principalTable: "Parcels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReturnParcels_Riders_RiderId",
                        column: x => x.RiderId,
                        principalTable: "Riders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveredParcels_HubId",
                table: "DeliveredParcels",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveredParcels_MerchantId",
                table: "DeliveredParcels",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveredParcels_RiderId",
                table: "DeliveredParcels",
                column: "RiderId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeParcels_HubId",
                table: "ExchangeParcels",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeParcels_ParcelId",
                table: "ExchangeParcels",
                column: "ParcelId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeParcels_RiderId",
                table: "ExchangeParcels",
                column: "RiderId");

            migrationBuilder.CreateIndex(
                name: "IX_Hubs_AdminId",
                table: "Hubs",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Merchants_HubId",
                table: "Merchants",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_DeliveryId",
                table: "Parcels",
                column: "DeliveryId",
                unique: true,
                filter: "[DeliveryId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_HubId",
                table: "Parcels",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_MerchantId",
                table: "Parcels",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_RiderId",
                table: "Parcels",
                column: "RiderId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnParcels_HubId",
                table: "ReturnParcels",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnParcels_ParcelId",
                table: "ReturnParcels",
                column: "ParcelId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnParcels_RiderId",
                table: "ReturnParcels",
                column: "RiderId");

            migrationBuilder.CreateIndex(
                name: "IX_Riders_HubId",
                table: "Riders",
                column: "HubId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "ExchangeParcels");

            migrationBuilder.DropTable(
                name: "ReturnParcels");

            migrationBuilder.DropTable(
                name: "Parcels");

            migrationBuilder.DropTable(
                name: "DeliveredParcels");

            migrationBuilder.DropTable(
                name: "Merchants");

            migrationBuilder.DropTable(
                name: "Riders");

            migrationBuilder.DropTable(
                name: "Hubs");

            migrationBuilder.DropTable(
                name: "Admins");
        }
    }
}
