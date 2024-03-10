using System.ComponentModel.DataAnnotations.Schema;

namespace CourierService_Web.Models
{
    public class DeliveredParcel
    {
        public string Id { get; set; } = "D-" + Guid.NewGuid().ToString().Substring(0, 4);
        [ForeignKey("ParcelId")]
        public string? ParcelId { get; set; }
        public Parcel Parcel { get; set; }
        public DateTime DeliveryDate { get; set; } = DateTime.Now;

        [ForeignKey("RiderId")]
        public string? RiderId { get; set; }
        public Rider Rider { get; set; }

        [ForeignKey("HubId")]
        public string? HubId { get; set; }
        public Hub Hub { get; set; }

        [ForeignKey("MerchantId")]
        public string? MerchantId { get; set; }
        public Merchant Merchant { get; set; }
    }
}
