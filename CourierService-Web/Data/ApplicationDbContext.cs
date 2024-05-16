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
        public DbSet<Payment> Payments { get; set; }

        public DbSet<RequestPermission> NotificationsPermission { get; set; }

        public DbSet<RiderPayment> riderPayments { get; set; }

        public DbSet<HubPayment> HubPayments { get; set; }
        public DbSet<MerchantPayment> MerchantPayments { get; set; }  
        
        public DbSet<Area> Areas { get; set; }
        public DbSet<Zone> Zone { get; set; }
        public DbSet<District> District { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<AccessGroup> AccessGroups { get; set; }
        public DbSet<AccessController> AccessControllers { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }


            //relationship between parcel and delivered parcel
            modelBuilder.Entity<DeliveredParcel>()
                 .HasOne(d => d.Parcel)
                 .WithOne(p => p.DeliveryParcel)
                 .HasForeignKey<Parcel>(d => d.DeliveryId);

            //relationship between parcel and return parcel
            modelBuilder.Entity<ReturnParcel>()
                .HasOne(r => r.Parcel)
                .WithOne(p => p.ReturnParcel)
                .HasForeignKey<Parcel>(r => r.ReturnId);

            //relationship between parcel and exchange parcel
            modelBuilder.Entity<ExchangeParcel>()
                .HasOne(e => e.Parcel)
                .WithOne(p => p.ExchangeParcel)
                .HasForeignKey<Parcel>(Parcel => Parcel.ExchangeId);

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
                .WithMany(h => h.Parcels)
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

            //relationship between payment and parcel
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Parcel)
                .WithMany(p => p.Payments)
                .HasForeignKey(p => p.ParcelId);

            //relationship between parcel and notification
            modelBuilder.Entity<RequestPermission>()
                .HasOne(p => p.Parcel)
                .WithMany(p => p.Notifications)
                .HasForeignKey(p => p.ParcelId);

            //relationship between rider and rider payment
            modelBuilder.Entity<RiderPayment>()
                .HasOne(r => r.Rider)
                .WithMany(r => r.riderPayments)
                .HasForeignKey(r => r.RiderId);

            //relationship between parcel and rider payment
            modelBuilder.Entity<RiderPayment>()
                .HasOne(p => p.Parcel)
                .WithMany(p => p.riderPayments)
                .HasForeignKey(p => p.ParcelId);

            //realtionship between merchantPayment and HubPayment
            modelBuilder.Entity<MerchantPayment>()
                .HasOne(m => m.HubPayment)
                .WithMany(h => h.MerchantPayments)
                .HasForeignKey(m => m.HubPaymentId);

            //relationship between merchant and merchant payment
            modelBuilder.Entity<MerchantPayment>()
                .HasOne(m => m.Merchant)
                .WithMany(m => m.MerchantPayments)
                .HasForeignKey(m => m.MerchantId);

            //relationship between hub and district
            modelBuilder.Entity<Hub>()
                .HasOne(h => h.District)
                .WithMany(d => d.Hubs)
                .HasForeignKey(h => h.DistrictId);

            //relationship between hub and area
            modelBuilder.Entity<Area>()
                .HasOne(a => a.Hub)
                .WithMany(h => h.Areas)
                .HasForeignKey(a => a.HubId);

            //relationship between hub and zone
            modelBuilder.Entity<Zone>()
                .HasOne(z => z.Hub)
                .WithMany(h => h.Zones)
                .HasForeignKey(z => z.HubId);

            //relationship between zone and area
            modelBuilder.Entity<Area>()
                .HasOne(a => a.Zone)
                .WithMany(z => z.Areas)
                .HasForeignKey(a => a.ZoneId);

            //relationship between district and area
            modelBuilder.Entity<Area>()
                .HasOne(a => a.District)
                .WithMany(d => d.Areas)
                .HasForeignKey(a => a.DistrictId);

            //relationship between zone and district
            modelBuilder.Entity<Zone>()
                .HasOne(z => z.District)
                .WithMany(d => d.Zones)
                .HasForeignKey(z => z.DistrictId);

            //relationship between store and district
            modelBuilder.Entity<Store>()
                .HasOne(s => s.District)
                .WithMany(d => d.Stores)
                .HasForeignKey(s => s.DistrictId);

            //relationship between store and zone
            modelBuilder.Entity<Store>()
                .HasOne(s => s.Zone)
                .WithMany(z => z.Stores)
                .HasForeignKey(s => s.ZoneId);

            //relationship between store and area
            modelBuilder.Entity<Store>()
                .HasOne(s => s.Area)
                .WithMany(a => a.Stores)
                .HasForeignKey(s => s.AreaId);

            //relationship between store and hub
            modelBuilder.Entity<Store>()
                .HasOne(s => s.Hub)
                .WithMany(h => h.Stores)
                .HasForeignKey(s => s.HubId);

            //relationship between store and merchant
            modelBuilder.Entity<Store>()
                .HasOne(s => s.Merchant)
                .WithMany(m => m.Stores)
                .HasForeignKey(s => s.MerchantId);

            //relationship between store and parcel
            modelBuilder.Entity<Store>()
                .HasMany(s => s.Parcels)
                .WithOne(p => p.Store)
                .HasForeignKey(p => p.StoreId);

            //relationship between parcel and area
            modelBuilder.Entity<Parcel>()
                .HasOne(p => p.Area)
                .WithMany(a => a.Parcels)
                .HasForeignKey(p => p.AreaId);

            //relationship between parcel and zone
            modelBuilder.Entity<Parcel>()
                .HasOne(p => p.Zone)
                .WithMany(z => z.Parcels)
                .HasForeignKey(p => p.ZoneId);

            //relationship between parcel and district
            modelBuilder.Entity<Parcel>()
                .HasOne(p => p.District)
                .WithMany(d => d.Parcels)
                .HasForeignKey(p => p.DistrictId);

            //relationship between accessgroup and accesscontroller
            modelBuilder.Entity<AccessGroup>()
                .HasMany(ag => ag.AccessControllers)
                .WithOne(ac => ac.AccessGroup)
                .HasForeignKey(ac => ac.AccessGroupId);



            modelBuilder.Entity<Zone>()
        .HasMany(z => z.Areas)
        .WithOne(a => a.Zone)
        .OnDelete(DeleteBehavior.Cascade);

            



            //seed district data
            modelBuilder.Entity<District>().HasData(
                               new District
                               {
                    Id = "DIS-123",
                    Name = "Dhaka"
                });
           
            //seed zone data
            modelBuilder.Entity<Zone>().HasData(
                                                                            new Zone
                                                                            {
                    Id = "ZONE-123",
                    Name = "Dhaka",
                    DistrictId = "DIS-123",
                    HubId = "HUB-123"
                });
           

            //seed area data
            modelBuilder.Entity<Area>().HasData(
                                                             new Area
                                                             {
                    Id = "AREA-123",
                    Name = "Mirpur",
                    DistrictId = "DIS-123",
                    ZoneId = "ZONE-123",
                    HubId = "HUB-123"
                });

            //seed hub data
            modelBuilder.Entity<Hub>().HasData(
                                                             new Hub
                                                             {
                    Id = "HUB-123",
                    Name = "Mirpur Hub",
                    DistrictId = "DIS-123",
                    
                   
                });



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
            //modelBuilder.Entity<Merchant>().HasData(
            //                   new Merchant
            //                   {
            //                       Id = "M-123",
            //                       Name = "Merchant",
            //                       Email = "merchant@gmail.com",
            //                       Password = "1111",
            //                       ConfirmPassword = "1111",
            //                       ContactNumber = "01837730317",
            //                       CompanyName = "Merchant Company",
            //                       FullAddress = "Dhaka, Bangladesh",
            //                       Area = "Mirpur"




            //                   });

            //seed hub data
            //modelBuilder.Entity<Hub>().HasData(
            //                                  new Hub
            //                                  {
            //                                      Id = "H-123",
            //                                      Name = "Hub",
            //                                      Email = "hub@gmail.com",
            //                                      Password = "1111",
            //                                      PhoneNumber = "01837730317",
            //                                      Area = "Mirpur",
            //                                      Address = "Dhaka, Bangladesh",
            //                                      Status = 1,
            //                                      CreatedAt = DateTime.Now,
            //                                      CreatedBy = "Admin",
            //                                      AdminId = "A-123",
            //                                      District = "Dhaka"

            //                                  });

            //seed rider data
            //modelBuilder.Entity<Rider>().HasData(
            //                                                 new Rider
            //                                                 {
            //                                                     Id = "R-123",
            //                                                     Name = "Rider",
            //                                                     Email = "rider@gmail.com",
            //                                                     Password = "1111",
            //                                                     ContactNumber = "01837730317",
            //                                                     Area = "Dhaka",
            //                                                     Salary = 10000,
            //                                                     NID = "0123456789",
            //                                                     District = "Dhaka",
            //                                                     FullAddress = "Dhaka, Bangladesh",
            //                                                     HubId = "H-123"

            //                                                 });

        }

        
    }
}
