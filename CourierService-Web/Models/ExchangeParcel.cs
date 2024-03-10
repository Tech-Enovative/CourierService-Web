namespace CourierService_Web.Models
{
    public class ExchangeParcel
    {
        public string Id { get; set; } = "E-" + Guid.NewGuid().ToString().Substring(0, 4);
        public Parcel ParcelId { get; set; }
        public DateTime ExchangeDate { get; set; } = DateTime.Now;

        public Rider RiderId { get; set; }

        public Hub HubId { get; set; }
    }
}
