using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourierService_Web.Models
{
    public class Hub
    {
        [Key]
        public string Id { get; set; } = "H-" + Guid.NewGuid().ToString().Substring(0, 5);

        [Required(ErrorMessage = "Hub name is required.")]
        public string Name { get; set; }

        [ForeignKey("DistrictId")]
        public string DistrictId { get; set; }
        public District? District { get; set; }

        public List<Zone>? Zones { get; set; }

        public List<Area>? Areas { get; set; }

        public List<Rider>? Riders { get; set; }

        public List<Parcel>? Parcels { get; set; }

        public List<DeliveredParcel>? DeliveredParcels { get; set; }

        public List<ExchangeParcel>? ExchangeParcels { get; set; }

        public List<ReturnParcel>? ReturnParcels { get; set; }

        public List<Merchant>? Merchants { get; set; }

        public List<Store>? Stores { get; set; }
    }
}
