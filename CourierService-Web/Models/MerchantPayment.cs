using System.ComponentModel.DataAnnotations.Schema;

namespace CourierService_Web.Models
{
    public class MerchantPayment
    {
        public string Id { get; set; } = "MerchantPAY-" + Guid.NewGuid().ToString();
        public string MerchantId { get; set; }
        public Merchant? Merchant { get; set; }

        public int? TotalAmount { get; set; }
        public int? AmountPaid { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;

        // Calculate due amount
        public int? DueAmount { get; set; }

        [ForeignKey("HubPaymentId")]
        public string? HubPaymentId { get; set; }

       public HubPayment? HubPayment { get; set; }
    }
}
