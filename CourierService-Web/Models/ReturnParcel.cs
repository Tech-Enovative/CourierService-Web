using System.ComponentModel.DataAnnotations.Schema;

namespace CourierService_Web.Models
{
    public class ReturnParcel
    {
        public string Id { get; set; } = "R" + System.DateTime.Now.ToString("ddMMyyyy") + System.Guid.NewGuid().ToString().Substring(0, 3).ToUpper();

        public string? ParcelId { get; set; }
        public Parcel? Parcel { get; set; }
        public DateTime ReturnDate { get; set; } = DateTime.Now;

        [ForeignKey("RiderId")]
        public string? RiderId { get; set; }
        public Rider Rider { get; set; }

        [ForeignKey("HubId")]
        public string? HubId { get; set; }
        public Hub Hub { get; set; }

        [ForeignKey("MerchantId")]
        public string? MerchantId { get; set; }
        public Merchant? Merchant { get; set; }

    }
}
