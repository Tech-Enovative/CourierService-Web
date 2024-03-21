using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class NewMig : Migration
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
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacebookUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TradeLicense = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    HubId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Riders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Riders_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Complain",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MerchantId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complain", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Complain_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id");
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
                name: "ExchangeParcels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParcelId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExchangeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RiderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HubId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MerchantId = table.Column<string>(type: "nvarchar(450)", nullable: true)
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
                        name: "FK_ExchangeParcels_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
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
                    ParcelId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RiderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HubId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MerchantId = table.Column<string>(type: "nvarchar(450)", nullable: true)
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
                        name: "FK_ReturnParcels_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReturnParcels_Riders_RiderId",
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
                    ReturnId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DeliveryId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ExchangeId = table.Column<string>(type: "nvarchar(450)", nullable: true)
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
                        name: "FK_Parcels_ExchangeParcels_ExchangeId",
                        column: x => x.ExchangeId,
                        principalTable: "ExchangeParcels",
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
                        name: "FK_Parcels_ReturnParcels_ReturnId",
                        column: x => x.ReturnId,
                        principalTable: "ReturnParcels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Parcels_Riders_RiderId",
                        column: x => x.RiderId,
                        principalTable: "Riders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParcelId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Parcels_ParcelId",
                        column: x => x.ParcelId,
                        principalTable: "Parcels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "Email", "Name", "Password" },
                values: new object[] { "A-123", "flyerbd@gmail.com", "Admin", "1111" });

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "Id", "Area", "CompanyAddress", "CompanyName", "ContactNumber", "CreatedAt", "District", "Email", "FacebookUrl", "FullAddress", "HubId", "ImageUrl", "NID", "Name", "Password", "Tin", "TradeLicense", "Website" },
                values: new object[] { "M-123", "Mirpur", null, "Merchant Company", "01837730317", new DateTime(2024, 3, 18, 20, 21, 49, 585, DateTimeKind.Local).AddTicks(5695), null, "merchant@gmail.com", null, "Dhaka, Bangladesh", null, null, null, "Merchant", "1111", null, null, null });

            migrationBuilder.InsertData(
                table: "Hubs",
                columns: new[] { "Id", "Address", "AdminId", "Area", "CreatedAt", "CreatedBy", "District", "Email", "Name", "Password", "PhoneNumber", "Status" },
                values: new object[] { "H-123", "Dhaka, Bangladesh", "A-123", "Mirpur", new DateTime(2024, 3, 18, 20, 21, 49, 585, DateTimeKind.Local).AddTicks(5805), "Admin", "Dhaka", "hub@gmail.com", "Hub", "1111", "01837730317", 1 });

            migrationBuilder.InsertData(
                table: "Riders",
                columns: new[] { "Id", "Area", "ContactNumber", "CreatedAt", "District", "Email", "FullAddress", "HubId", "ImageUrl", "NID", "Name", "Password", "Salary", "State", "Status" },
                values: new object[] { "R-123", "Dhaka", "01837730317", new DateTime(2024, 3, 18, 20, 21, 49, 585, DateTimeKind.Local).AddTicks(5890), "Dhaka", "rider@gmail.com", "Dhaka, Bangladesh", "H-123", null, "0123456789", "Rider", "1111", 10000, "Available", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Complain_MerchantId",
                table: "Complain",
                column: "MerchantId");

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
                name: "IX_ExchangeParcels_MerchantId",
                table: "ExchangeParcels",
                column: "MerchantId");

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
                name: "IX_Parcels_ExchangeId",
                table: "Parcels",
                column: "ExchangeId",
                unique: true,
                filter: "[ExchangeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_HubId",
                table: "Parcels",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_MerchantId",
                table: "Parcels",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_ReturnId",
                table: "Parcels",
                column: "ReturnId",
                unique: true,
                filter: "[ReturnId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_RiderId",
                table: "Parcels",
                column: "RiderId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ParcelId",
                table: "Payments",
                column: "ParcelId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnParcels_HubId",
                table: "ReturnParcels",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnParcels_MerchantId",
                table: "ReturnParcels",
                column: "MerchantId");

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
                name: "Complain");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Parcels");

            migrationBuilder.DropTable(
                name: "DeliveredParcels");

            migrationBuilder.DropTable(
                name: "ExchangeParcels");

            migrationBuilder.DropTable(
                name: "ReturnParcels");

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
