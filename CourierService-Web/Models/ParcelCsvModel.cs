namespace CourierService_Web.Models
{
    public class ParcelCsvModel
    {
        public string ReceiverName { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverContactNumber { get; set; }
        public string ReceiverEmail { get; set; }
        public string ProductName { get; set; }
        public decimal ProductWeight { get; set; }
        public int ProductPrice { get; set; }
        public int? ProductQuantity { get; set; }
        public int DeliveryCharge { get; set; }
        public string Status { get; set; }
        public DateTime? PickupRequestDate { get; set; }
        public DateTime? DispatchDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? CancelDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentInHand { get; set; }
        public string PickupLocation { get; set; }
        public string DeliveryType { get; set; }
        public int TotalPrice { get; set; }
        public int COD { get; set; }
        public string MerchantId { get; set; }
        public string RiderId { get; set; }
        public string HubId { get; set; }
        public string ReturnId { get; set; }
        public string DeliveryId { get; set; }
        public string ExchangeId { get; set; }
    }
}
