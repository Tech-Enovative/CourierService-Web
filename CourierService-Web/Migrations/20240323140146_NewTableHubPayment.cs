using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class NewTableHubPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HubDue",
                table: "riderPayments");

            migrationBuilder.DropColumn(
                name: "HubDueDate",
                table: "riderPayments");

            migrationBuilder.DropColumn(
                name: "HubReceivedAmount",
                table: "riderPayments");

            migrationBuilder.DropColumn(
                name: "HubReceivedDate",
                table: "riderPayments");

            migrationBuilder.CreateTable(
                name: "HubPayments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HubId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalAmount = table.Column<int>(type: "int", nullable: false),
                    AmountReceived = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HubPayments_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 20, 1, 45, 417, DateTimeKind.Local).AddTicks(1480));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 20, 1, 45, 417, DateTimeKind.Local).AddTicks(1404));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 20, 1, 45, 417, DateTimeKind.Local).AddTicks(1539));

            migrationBuilder.CreateIndex(
                name: "IX_HubPayments_HubId",
                table: "HubPayments",
                column: "HubId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HubPayments");

            migrationBuilder.AddColumn<int>(
                name: "HubDue",
                table: "riderPayments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HubDueDate",
                table: "riderPayments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HubReceivedAmount",
                table: "riderPayments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HubReceivedDate",
                table: "riderPayments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 14, 23, 35, 657, DateTimeKind.Local).AddTicks(5233));

            migrationBuilder.UpdateData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 14, 23, 35, 657, DateTimeKind.Local).AddTicks(5181));

            migrationBuilder.UpdateData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123",
                column: "CreatedAt",
                value: new DateTime(2024, 3, 23, 14, 23, 35, 657, DateTimeKind.Local).AddTicks(5265));
        }
    }
}
