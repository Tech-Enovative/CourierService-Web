using System.ComponentModel.DataAnnotations.Schema;

namespace CourierService_Web.Models
{
    public class Complain
    {
        public string Id { get; set; } = "COM-" + Guid.NewGuid().ToString().Substring(0, 4);
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey("MerchantId")]
        public string? MerchantId { get; set; }
        public Merchant? Merchant { get; set; }
    }

}
