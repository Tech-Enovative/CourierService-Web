namespace CourierService_Web.Models
{
    public class ReturnParcel
    {
        public string Id { get; set; } = "R-" + Guid.NewGuid().ToString().Substring(0, 4);
        public Parcel ParcelId { get; set; }
        public DateTime ReturnDate { get; set; } = DateTime.Now;

        public Rider RiderId { get; set; }

        public Hub HubId { get; set; }

    }
}
