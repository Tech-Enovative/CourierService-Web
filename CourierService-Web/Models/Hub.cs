using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourierService_Web.Models
{
    public class Hub
    {
        [Key]
        public string Id { get; set; } = "H-" + Guid.NewGuid().ToString().Substring(0, 4);
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Area is required.")]
        public string Area { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^(\+88)?01[0-9]{9}$", ErrorMessage = "Please enter a valid phone number.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public int Status { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? CreatedBy { get; set; } 

        [ForeignKey("AdminId")]
        public string? AdminId { get; set; }
        public Admin Admin { get; set; }
        
        public List<Rider>? Riders { get; set; }

        public List<Parcel>? parcels { get; set; }

        public List<DeliveredParcel>? DeliveredParcels { get; set; }

        public List<ExchangeParcel>? ExchangeParcels { get; set; }

        public List<ReturnParcel>? ReturnParcels { get; set; }
        public List<Merchant>? Merchants { get; set; }

    }
}
