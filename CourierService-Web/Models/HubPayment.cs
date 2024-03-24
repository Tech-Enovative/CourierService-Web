using System.ComponentModel.DataAnnotations.Schema;

namespace CourierService_Web.Models
{
    public class HubPayment
    {
        public string Id { get; set; }="HubPAY-" + Guid.NewGuid().ToString();
        public string? HubId { get; set; }
        public Hub? Hub { get; set; }

        public int? TotalAmount { get; set; }

        public int? AmountReceived { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;

        //calculate due amount
       public int? DueAmount { get;set; }

       public List<RiderPayment> RiderPayments { get; set; }
    }
}
