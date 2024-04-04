using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourierService_Web.Models
{
    public class District
    {
        [Key]
        public string Id { get; set; } = "DI-" + Guid.NewGuid().ToString().Substring(0, 5);

        [Required(ErrorMessage = "District name is required.")]
        public string Name { get; set; }

        public List<Zone>? Zones { get; set; }

        public List<Area>? Areas { get; set; }

        public List<Hub>? Hubs { get; set; }

        public List<Store>? Stores { get; set; }

        public List<Parcel>? Parcels { get; set; }
    }
}
