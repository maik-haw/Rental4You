using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rental4You.Models;

namespace Rental4You.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<DeliveryImage> DeliveryImages { get; set; }
        public DbSet<Pickup> Pickups { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleCategory> VehicleCategories { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Reservation>()
                .HasOne(r => r.Pickup)
                .WithOne(p => p.Reservation)
                .HasForeignKey<Pickup>(r => r.ReservationId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<Reservation>()
                .HasOne(r => r.Delivery)
                .WithOne(p => p.Reservation)
                .HasForeignKey<Delivery>(r => r.ReservationId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}