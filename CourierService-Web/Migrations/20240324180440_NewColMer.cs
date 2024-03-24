using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class NewColMer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HubPayments_riderPayments_RiderPaymentId",
                table: "HubPayments");

            migrationBuilder.DropIndex(
                name: "IX_HubPayments_RiderPaymentId",
                table: "HubPayments");

            migrationBuilder.DropColumn(
                name: "Due",
                table: "riderPayments");

            migrationBuilder.DropColumn(
                name: "HubReceived",
                table: "riderPayments");

            migrationBuilder.DropColumn(
                name: "RiderPaymentId",
                table: "HubPayments");

            migrationBuilder.AlterColumn<string>(
                name: "HubPaymentId",
                table: "riderPayments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "MerchantPayments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MerchantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalAmount = table.Column<int>(type: "int", nullable: true),
                    AmountPaid = table.Column<int>(type: "int", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueAmount = table.Column<int>(type: "int", nullable: true),
                    HubPaymentId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MerchantPayments_HubPayments_HubPaymentId",
                        column: x => x.HubPaymentId,
                        principalTable: "HubPayments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MerchantPayments_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 0, 4, 39, 436, DateTimeKind.Local).AddTicks(5371));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 0, 4, 39, 436, DateTimeKind.Local).AddTicks(5313));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 25, 0, 4, 39, 436, DateTimeKind.Local).AddTicks(5412));

            migrationBuilder.CreateIndex(
                name: "IX_riderPayments_HubPaymentId",
                table: "riderPayments",
                column: "HubPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantPayments_HubPaymentId",
                table: "MerchantPayments",
                column: "HubPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantPayments_MerchantId",
                table: "MerchantPayments",
                column: "MerchantId");

            migrationBuilder.AddForeignKey(
                name: "FK_riderPayments_HubPayments_HubPaymentId",
                table: "riderPayments",
                column: "HubPaymentId",
                principalTable: "HubPayments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_riderPayments_HubPayments_HubPaymentId",
                table: "riderPayments");

            migrationBuilder.DropTable(
                name: "MerchantPayments");

            migrationBuilder.DropIndex(
                name: "IX_riderPayments_HubPaymentId",
                table: "riderPayments");

            migrationBuilder.AlterColumn<string>(
                name: "HubPaymentId",
                table: "riderPayments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Due",
                table: "riderPayments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HubReceived",
                table: "riderPayments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RiderPaymentId",
                table: "HubPayments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 24, 15, 3, 14, 400, DateTimeKind.Local).AddTicks(1247));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 24, 15, 3, 14, 400, DateTimeKind.Local).AddTicks(1197));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 24, 15, 3, 14, 400, DateTimeKind.Local).AddTicks(1288));

            migrationBuilder.CreateIndex(
                name: "IX_HubPayments_RiderPaymentId",
                table: "HubPayments",
                column: "RiderPaymentId",
                unique: true,
                filter: "[RiderPaymentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_HubPayments_riderPayments_RiderPaymentId",
                table: "HubPayments",
                column: "RiderPaymentId",
                principalTable: "riderPayments",
                principalColumn: "Id");
        }
    }
}
