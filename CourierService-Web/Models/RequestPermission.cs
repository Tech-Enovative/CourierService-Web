namespace CourierService_Web.Models
{
    public class RequestPermission
    {
        public string Id { get; set; } = "N-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        public string Title { get; set; }
        public string Message { get; set; }
        public int RequestedPrice { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public DateTime Date { get; set; }
        public string ParcelId { get; set; }  

        public Parcel Parcel { get; set; }

        public int Status { get; set; } = 0;

    }
}
