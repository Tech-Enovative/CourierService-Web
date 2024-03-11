using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierService_Web.Migrations
{
    /// <inheritdoc />
    public partial class SeedMerchantData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "Id", "Area", "CompanyAddress", "CompanyName", "ConfirmPassword", "ContactNumber", "District", "Email", "FacebookUrl", "FullAddress", "HubId", "ImageUrl", "NID", "Name", "Password", "Tin", "TradeLicense", "Website" },
                values: new object[] { "M-123", null, null, "Merchant Company", "1111", "01837730317", null, "merchant@gmail.com", null, "Dhaka, Bangladesh", null, null, null, "Merchant", "1111", null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "Id",
                keyValue: "M-123");
        }
    }
}
