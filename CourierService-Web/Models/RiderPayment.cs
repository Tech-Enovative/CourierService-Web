using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourierService_Web.Models
{
    public class RiderPayment
    {
        [Key]
        public string Id { get; set; } = "RPAY-" + Guid.NewGuid().ToString().Substring(0, 4);

        [Required]
        public int Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [ForeignKey("ParcelId")]
        public string ParcelId { get; set; }

        public Parcel Parcel { get; set; }

        [ForeignKey("RiderId")]
        public string RiderId { get; set; }

        public Rider Rider { get; set; }
    }
}
