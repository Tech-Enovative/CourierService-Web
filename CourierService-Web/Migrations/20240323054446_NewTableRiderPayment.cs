using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class NewTableRiderPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "riderPayments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParcelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RiderId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_riderPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_riderPayments_Parcels_ParcelId",
                        column: x => x.ParcelId,
                        principalTable: "Parcels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_riderPayments_Riders_RiderId",
                        column: x => x.RiderId,
                        principalTable: "Riders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 11, 44, 43, 957, DateTimeKind.Local).AddTicks(9805));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 11, 44, 43, 957, DateTimeKind.Local).AddTicks(9712));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 11, 44, 43, 957, DateTimeKind.Local).AddTicks(9867));

            migrationBuilder.CreateIndex(
                name: "IX_riderPayments_ParcelId",
                table: "riderPayments",
                column: "ParcelId");

            migrationBuilder.CreateIndex(
                name: "IX_riderPayments_RiderId",
                table: "riderPayments",
                column: "RiderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "riderPayments");

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 9, 51, 50, 933, DateTimeKind.Local).AddTicks(4679));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 9, 51, 50, 933, DateTimeKind.Local).AddTicks(4627));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 9, 51, 50, 933, DateTimeKind.Local).AddTicks(4710));
        }
    }
}
