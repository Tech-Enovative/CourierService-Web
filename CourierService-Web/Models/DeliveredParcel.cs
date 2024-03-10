namespace CourierService_Web.Models
{
    public class DeliveredParcel
    {
        public string Id { get; set; } = "D-" + Guid.NewGuid().ToString().Substring(0, 4);
        public Parcel ParcelId { get; set; }
        public DateTime DeliveryDate { get; set; } = DateTime.Now;

        public Rider RiderId { get; set; }

        public Hub HubId { get; set; }
    }
}
