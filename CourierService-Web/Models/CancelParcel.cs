namespace CourierService_Web.Models
{
    public class CancelParcel
    {
        public string Id { get; set; } = "C-" + Guid.NewGuid().ToString().Substring(0, 4);
        public Parcel ParcelId { get; set; }
        public DateTime CancelDate { get; set; } = DateTime.Now;

        public Rider RiderId { get; set; }

        public Hub HubId { get; set; }
    }
}
