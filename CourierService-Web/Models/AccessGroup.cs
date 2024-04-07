namespace CourierService_Web.Models
{
    public class AccessGroup
    {
        public string? Id { get; set; }="G-" + Guid.NewGuid().ToString().Substring(0, 4);
        public string? Name { get; set; }

        public List<AccessController>? AccessControllers { get; set; }
    }
}
