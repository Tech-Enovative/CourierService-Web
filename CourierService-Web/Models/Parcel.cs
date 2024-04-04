using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourierService_Web.Models
{
    public class Parcel
    {
        [Key]
        public string Id { get; set; } = "D" + System.DateTime.Now.ToString("ddMMyyyy") + System.Guid.NewGuid().ToString().Substring(0, 3).ToUpper();

        [Required(ErrorMessage = "Receiver name is required")]
        public string ReceiverName { get; set; }

        [Required(ErrorMessage = "Receiver address is required")]
        public string ReceiverAddress { get; set; }

        [Required(ErrorMessage = "Receiver contact number is required")]
        [RegularExpression(@"^01[3-9]\d{8}$", ErrorMessage = "Invalid contact number")]
        public string ReceiverContactNumber { get; set; }

        

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? ReceiverEmail { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Product weight is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Product weight can't be below 0")]
        public decimal ProductWeight { get; set; }

        [Required(ErrorMessage = "Product price is required")]
        [Range(0,int.MaxValue,ErrorMessage ="Product price can't be below 0")]
        public int ProductPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Product quantity must be at least 1")]

        public int? ProductQuantity { get; set; }

        [Required(ErrorMessage = "Delivery charge is required")]
        [Range(0,1000,ErrorMessage ="Delivery charge range is 0 to 1000")]
        public int DeliveryCharge { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = "Pending";
        public DateTime? PickupRequestDate { get; set; } 
        public DateTime? DispatchDate  { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? PickedUpAt { get; set; }
        public DateTime? InHubAt { get; set; }
        public DateTime? DeliveryRiderAssignedAt { get; set; }
        public DateTime? OnTheWayAt { get; set; }
        public DateTime? PaymentInHubAt { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? PaymentStatus { get; set; }
        public string? PaymentInHand { get; set; }
       
        public string? PickupLocation { get; set; }
        public string DeliveryType { get; set; }
        public int TotalPrice { get; set; }

        public int COD { get; set; }
        
        [ForeignKey("MerchantId")]
        public string? MerchantId { get; set; }
        public Merchant? Merchant { get; set; }
        [ForeignKey("RiderId")]
        public string? RiderId { get; set; }
        public Rider? Rider { get; set; }

        [ForeignKey("HubId")]
        public string? HubId { get; set; }

        public Hub? Hub { get; set; }

        [ForeignKey("ReturnId")]
       public string? ReturnId { get; set; }
       public ReturnParcel? ReturnParcel { get; set; }

        [ForeignKey("DeliveryId")]
        public string? DeliveryId { get; set; }
        public DeliveredParcel? DeliveryParcel { get; set; }

        [ForeignKey("ExchangeId")]
        public string? ExchangeId { get; set; }
        public ExchangeParcel? ExchangeParcel { get; set; }

        public List<Payment> Payments { get; set; } = new List<Payment>();

        // Method to add payment
        public void AddPayment(int amount)
        {
            Payments.Add(new Payment { Amount = amount });
        }

        public List<RequestPermission>? Notifications { get; set; }

        public List<RiderPayment>? riderPayments { get; set; }

        [ForeignKey("StoreId")]
        public string? StoreId { get; set; }

        public Store? Store { get; set; }

        [ForeignKey("DistrictId")]
        public string? DistrictId { get; set; }
        public District? District { get; set; }

        [ForeignKey("ZoneId")]
        public string? ZoneId { get; set; }
        public Zone? Zone { get; set; }

        [ForeignKey("AreaId")]
        public string? AreaId { get; set; }
        public Area? Area { get; set; }

        public DateTime? UpdatedAt { get; set; }


    }
}
