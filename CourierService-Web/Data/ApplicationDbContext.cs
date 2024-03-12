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

        public DbSet<Complain> Complain { get; set; }  





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

            //relation between merchant and complain
            modelBuilder.Entity<Complain>()
                .HasOne(m => m.Merchant)
                .WithMany(m => m.complains)
                .HasForeignKey(m => m.MerchantId);


            //seed admin data
            modelBuilder.Entity<Admin>().HasData(
                               new Admin
                               {
                                   Id = "A-123",
                                   Name = "Admin",
                                   Email = "flyerbd@gmail.com",
                                   Password = "1111"

                               });

            //seed merchant data
            modelBuilder.Entity<Merchant>().HasData(
                               new Merchant
                               {
                                   Id = "M-123",
                                   Name = "Merchant",
                                   Email = "merchant@gmail.com",
                                   Password = "1111",
                                   ConfirmPassword = "1111",
                                   ContactNumber = "01837730317",
                                   CompanyName = "Merchant Company",
                                   FullAddress = "Dhaka, Bangladesh",

                                   


                               });

            //seed hub data
            modelBuilder.Entity<Hub>().HasData(
                                              new Hub
                                              {
                                                  Id = "H-123",
                                                  Name = "Hub",
                                                  Email = "hub@gmail.com",
                                                  Password = "1111",
                                                  PhoneNumber = "01837730317",
                                                  Area = "Dhaka",
                                                  Address = "Dhaka, Bangladesh",
                                                  Status = 1,
                                                  CreatedAt = DateTime.Now,
                                                  CreatedBy = "Admin",
                                                  AdminId = "A-123"

                                              });

            //seed rider data
            modelBuilder.Entity<Rider>().HasData(
                                                             new Rider
                                                             {
                                                                 Id = "R-123",
                                                                 Name = "Rider",
                                                                 Email = "rider@gmail.com",
                                                                 Password = "1111",
                                                                 ContactNumber = "01837730317",
                                                                 Area = "Dhaka",
                                                                 Salary = 10000,
                                                                 NID = "0123456789",
                                                                 District = "Dhaka",
                                                                 FullAddress = "Dhaka, Bangladesh",
                                                                 HubId = "H-123"

                                                             });

        }

        
    }
}
