using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class NewHub : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hubs_Admins_AdminId",
                table: "Hubs");

            migrationBuilder.DropIndex(
                name: "IX_Hubs_AdminId",
                table: "Hubs");

            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123");

            migrationBuilder.DeleteData(
                table: "Riders",
                keyColumn: "Id",
                keyValue: "R-123");

            migrationBuilder.DeleteData(
                table: "Hubs",
                keyColumn: "Id",
                keyValue: "H-123");

            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: "A-123");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Hubs");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Hubs");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "Hubs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Hubs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Hubs");

            migrationBuilder.DropColumn(
                name: "District",
                table: "Hubs");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Hubs");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Hubs");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Hubs");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Hubs");

            migrationBuilder.AddColumn<string>(
                name: "DistrictId",
                table: "Hubs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ZoneId",
                table: "Hubs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Zone",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zone", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "District",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_District", x => x.Id);
                    table.ForeignKey(
                        name: "FK_District_Zone_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DistrictId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HubId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Areas_District_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "District",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Areas_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Areas_Zone_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hubs_DistrictId",
                table: "Hubs",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Hubs_ZoneId",
                table: "Hubs",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_DistrictId",
                table: "Areas",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_HubId",
                table: "Areas",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_ZoneId",
                table: "Areas",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_District_ZoneId",
                table: "District",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hubs_District_DistrictId",
                table: "Hubs",
                column: "DistrictId",
                principalTable: "District",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Hubs_Zone_ZoneId",
                table: "Hubs",
                column: "ZoneId",
                principalTable: "Zone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hubs_District_DistrictId",
                table: "Hubs");

            migrationBuilder.DropForeignKey(
                name: "FK_Hubs_Zone_ZoneId",
                table: "Hubs");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "District");

            migrationBuilder.DropTable(
                name: "Zone");

            migrationBuilder.DropIndex(
                name: "IX_Hubs_DistrictId",
                table: "Hubs");

            migrationBuilder.DropIndex(
                name: "IX_Hubs_ZoneId",
                table: "Hubs");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "Hubs");

            migrationBuilder.DropColumn(
                name: "ZoneId",
                table: "Hubs");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Hubs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Hubs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "Hubs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Hubs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Hubs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Hubs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Hubs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Hubs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Hubs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Hubs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "Email", "Name", "Password" },
                values: new object[] { "A-123", "flyerbd@gmail.com", "Admin", "1111" });

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "Id", "Area", "CompanyAddress", "CompanyName", "ContactNumber", "CreatedAt", "District", "Email", "FacebookUrl", "FullAddress", "HubId", "ImageUrl", "NID", "Name", "Password", "Tin", "TradeLicense", "Website" },
                values: new object[] { "M-123", "Mirpur", null, "Merchant Company", "01837730317", new DateTime(2024, 3, 30, 22, 5, 12, 430, DateTimeKind.Local).AddTicks(699), null, "merchant@gmail.com", null, "Dhaka, Bangladesh", null, null, null, "Merchant", "1111", null, null, null });

            migrationBuilder.InsertData(
                table: "Hubs",
                columns: new[] { "Id", "Address", "AdminId", "Area", "CreatedAt", "CreatedBy", "District", "Email", "Name", "Password", "PhoneNumber", "Status" },
                values: new object[] { "H-123", "Dhaka, Bangladesh", "A-123", "Mirpur", new DateTime(2024, 3, 30, 22, 5, 12, 430, DateTimeKind.Local).AddTicks(750), "Admin", "Dhaka", "hub@gmail.com", "Hub", "1111", "01837730317", 1 });

            migrationBuilder.InsertData(
                table: "Riders",
                columns: new[] { "Id", "Area", "ContactNumber", "CreatedAt", "District", "Email", "FullAddress", "HubId", "ImageUrl", "NID", "Name", "Password", "Salary", "State", "Status" },
                values: new object[] { "R-123", "Dhaka", "01837730317", new DateTime(2024, 3, 30, 22, 5, 12, 430, DateTimeKind.Local).AddTicks(787), "Dhaka", "rider@gmail.com", "Dhaka, Bangladesh", "H-123", null, "0123456789", "Rider", "1111", 10000, "Available", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Hubs_AdminId",
                table: "Hubs",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hubs_Admins_AdminId",
                table: "Hubs",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id");
        }
    }
}
