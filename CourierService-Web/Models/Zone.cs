using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourierService_Web.Models
{
    public class Zone
    {
        [Key]
        public string Id { get; set; } = "Z-" + Guid.NewGuid().ToString().Substring(0, 5);

        [Required(ErrorMessage = "Zone name is required.")]
        public string Name { get; set; }

        public List<Area>? Areas { get; set; }

        [ForeignKey("DistrictId")]
        public string? DistrictId { get; set; }

        public District? District { get; set; }

        [ForeignKey("HubId")]

        public string? HubId { get; set; }
        public Hub? Hub { get; set; }
       
    }
}
