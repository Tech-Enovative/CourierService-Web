using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourierService_Web.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [ForeignKey("ParcelId")]
        public string ParcelId { get; set; }

        public Parcel Parcel { get; set; }
    }
}
