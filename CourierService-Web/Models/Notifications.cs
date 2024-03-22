namespace CourierService_Web.Models
{
    public class Notifications
    {
        public string Id { get; set; } = "N-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        public string Title { get; set; }
        public string Message { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
    }
}
