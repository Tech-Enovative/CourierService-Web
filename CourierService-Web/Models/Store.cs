using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourierService_Web.Models
{
    public class Store
    {
        public string Id { get; set; } = "S-"+Guid.NewGuid().ToString().Substring(0, 4).ToUpper();

        [Required(ErrorMessage = "Store name is required.")]
        
        public string Name { get; set; }
        [Required(ErrorMessage = "Contact is required.")]
        public string Contact { get; set; }

        public string? SecondaryContact { get; set; }

        [ForeignKey("DistrictId")]
        public string? DistrictId { get; set; }
        public District? District { get; set; }

        [ForeignKey("ZoneId")]
        public string? ZoneId { get; set; }
        public Zone? Zone { get; set; }

        [ForeignKey("AreaId")]

        public string? AreaId { get; set; }
        public Area? Area { get; set; }

        [ForeignKey("HubId")]
        public string? HubId { get; set; }
       
        public Hub? Hub { get; set; }

        [ForeignKey("MerchantId")]
        public string? MerchantId { get; set; }

        public Merchant? Merchant { get; set; }
        public string Address { get; set; }

        public List<Parcel>? Parcels { get; set; }

        public int? Status { get; set; } = 0;
    }
}
