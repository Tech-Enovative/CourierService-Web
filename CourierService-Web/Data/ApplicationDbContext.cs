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
        //public DbSet<CancelParcel> CancelParcels { get; set; }
        public DbSet<DeliveredParcel> DeliveredParcels { get; set; }
        public DbSet<ExchangeParcel> ExchangeParcels { get; set; }
        public DbSet<ReturnParcel> ReturnParcels { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            //relationship between parcel and delivered parcel
            modelBuilder.Entity<DeliveredParcel>()
                 .HasOne(d => d.Parcel)
                 .WithOne(p => p.DeliveryParcel)
                 .HasForeignKey<Parcel>(d => d.DeliveryId);

            //relationship between parcel and return parcel
            modelBuilder.Entity<ReturnParcel>()
                .HasOne(r => r.Parcel)
                .WithMany(p => p.ReturnParcel)
                .HasForeignKey(q => q.ParcelId);

            //relationship between parcel and exchange parcel
            modelBuilder.Entity<ExchangeParcel>()
                .HasOne(e => e.Parcel)
                .WithMany(p => p.ExchangeParcel)
                .HasForeignKey(q => q.ParcelId);

            //relationship between parcel and cancel parcel
            //modelBuilder.Entity<CancelParcel>()
            //     .HasOne(c => c.ParcelId) 
            //     .WithOne(p => p.CancelParcel)
            //     .HasForeignKey<CancelParcel>(c => c.ParcelId); 

            //relationship between rider and delivered parcel
            modelBuilder.Entity<DeliveredParcel>()
                .HasOne(d => d.Rider)
                .WithMany(r => r.DeliveredParcels)
                .HasForeignKey(d => d.RiderId);

            //relationship between rider and return parcel
            modelBuilder.Entity<ReturnParcel>()
                .HasOne(r => r.Rider)
                .WithMany(r => r.ReturnParcels)
                .HasForeignKey(r => r.RiderId);

            //relationship between rider and exchange parcel
            modelBuilder.Entity<ExchangeParcel>()
                .HasOne(e => e.Rider)
                .WithMany(r => r.ExchangeParcels)
                .HasForeignKey(e => e.RiderId);

            //relationship between hub and parcel
            modelBuilder.Entity<Parcel>()
                .HasOne(p => p.Hub)
                .WithMany(h => h.parcels)
                .HasForeignKey(p => p.HubId);

            //relationship between hub and delivered parcel
            modelBuilder.Entity<DeliveredParcel>()
                .HasOne(d => d.Hub)
                .WithMany(h => h.DeliveredParcels)
                .HasForeignKey(d => d.HubId);

            //relationship between hub and return parcel
            modelBuilder.Entity<ReturnParcel>()
                .HasOne(r => r.Hub)
                .WithMany(h => h.ReturnParcels)
                .HasForeignKey(r => r.HubId);

            //relationship between hub and exchange parcel
            modelBuilder.Entity<ExchangeParcel>()
                .HasOne(e => e.Hub)
                .WithMany(h => h.ExchangeParcels)
                .HasForeignKey(e => e.HubId);

            //relationship between rider and hub
            modelBuilder.Entity<Rider>()
                .HasOne(r => r.Hub)
                .WithMany(h => h.Riders)
                .HasForeignKey(r => r.HubId);

            //relationship between merchant and parcel
            modelBuilder.Entity<Parcel>()
                .HasOne(p => p.Merchant)
                .WithMany(m => m.Parcels)
                .HasForeignKey(p => p.MerchantId);

            //relationship between merchant and delivered parcel
            modelBuilder.Entity<DeliveredParcel>()
                .HasOne(d => d.Merchant)
                .WithMany(m => m.DeliveredParcels)
                .HasForeignKey(d => d.MerchantId);

        }

        
    }
}
