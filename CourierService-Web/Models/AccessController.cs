using System.ComponentModel.DataAnnotations.Schema;

namespace CourierService_Web.Models
{
    public class AccessController
    {
        public string? Id { get; set; }="AC-" + Guid.NewGuid().ToString().Substring(0, 4);
        public string? AccessName { get; set; }
        public int? Status { get; set; } = 1;

        [ForeignKey("AccessGroupId")]
        public string? AccessGroupId { get; set; }
        public AccessGroup AccessGroup { get; set; }
    }
}
