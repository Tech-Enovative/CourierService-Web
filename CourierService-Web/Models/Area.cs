using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourierService_Web.Models
{
    public class Area
    {
        [Key]
        public string Id { get; set; } = "AREA-" + Guid.NewGuid().ToString().Substring(0, 5);

        [Required(ErrorMessage = "Area name is required.")]
        public string Name { get; set; }

        [ForeignKey("DistrictId")]
        public string DistrictId { get; set; }
        public District? District { get; set; }

        [ForeignKey("ZoneId")]
        public string ZoneId { get; set; }

        public Zone? Zone { get; set; }

        [ForeignKey("HubId")]
        public string HubId { get; set; }

        public Hub? Hub { get; set; }
    }
}
