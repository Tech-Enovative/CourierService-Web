namespace CourierService_Web.Models
{
    public class ParcelImportViewModel
    {
        public string Store { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverContactNumber { get; set; }
        public string District { get; set; }
        public string Zone { get; set; }
        public string Area { get; set; }
        public string ProductName { get; set; }
        public decimal ProductWeight { get; set; }
        //public string? DeliveryType { get; set; }
        public int ProductPrice { get; set; }
        public int? ProductQuantity { get; set; }
        public string? Hub { get; set; }
    }
}
