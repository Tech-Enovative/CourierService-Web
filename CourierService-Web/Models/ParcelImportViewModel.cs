namespace CourierService_Web.Models
{
    public class ParcelImportViewModel
    {
        public string ReceiverName { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverContactNumber { get; set; }
        public string ReceiverEmail { get; set; }
        public string ProductName { get; set; }
        public decimal ProductWeight { get; set; }
        public int ProductPrice { get; set; }
        public int? ProductQuantity { get; set; }
        public string PickupLocation { get; set; }
        public string DeliveryType { get; set; }
        public int COD { get; set; }
    }
}
