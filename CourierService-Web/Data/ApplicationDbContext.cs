using CourierService_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CourierService_Web.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //Dbsets
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Rider> Riders { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Hub> Hubs { get; set; }
        public DbSet<Parcel> Parcels { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<CancelParcel> CancelParcels { get; set; }
        public DbSet<DeliveredParcel> DeliveredParcels { get; set; }
        public DbSet<ExchangeParcel> ExchangeParcels { get; set; }
        public DbSet<ReturnParcel> ReturnParcels { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


           //relationship between parcel and delivered parcel
           modelBuilder.Entity<DeliveredParcel>()
                .HasOne(d => d.ParcelId)
                .WithOne(p => p.DeliveryParcel)
                .HasForeignKey<DeliveredParcel>(d => d.ParcelId);

            //relationship between parcel and return parcel
            modelBuilder.Entity<ReturnParcel>()
                .HasOne(r => r.ParcelId)
                .WithMany(p => p.ReturnParcel)
                .HasForeignKey(q => q.ParcelId);

            //relationship between parcel and exchange parcel
            modelBuilder.Entity<ExchangeParcel>()
                .HasOne(e => e.ParcelId)
                .WithMany(p => p.ExchangeParcel)
                .HasForeignKey(q => q.ParcelId);

            //relationship between parcel and cancel parcel
            modelBuilder.Entity<CancelParcel>()
                 .HasOne(c => c.ParcelId) 
                 .WithOne(p => p.CancelParcel)
                 .HasForeignKey<CancelParcel>(c => c.ParcelId); 



        }

        
    }
}
