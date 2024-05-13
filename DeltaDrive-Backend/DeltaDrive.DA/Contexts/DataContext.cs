using DeltaDrive.DA.Contracts.Model;
using Microsoft.EntityFrameworkCore;

namespace DeltaDrive.DA.Contexts
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        // Database Sets:
        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleBooking> VehicleBookings { get; set; }
        public DbSet<VehicleBookingRating> VehicleBookingRatings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // FK Example:
            //modelBuilder.Entity<User>()
            //    .HasOne(user => user.Role)
            //    .WithMany()
            //    .HasForeignKey(user => user.RoleId);

            modelBuilder.Entity<User>()
                .HasIndex(user => user.Email)
                .IsUnique();

            modelBuilder.Entity<VehicleBooking>()
                .HasOne(booking => booking.User)
                .WithMany()
                .HasForeignKey(booking => booking.UserId);

            modelBuilder.Entity<VehicleBooking>()
                .HasOne(booking => booking.Vehicle)
                .WithMany()
                .HasForeignKey(booking => booking.VehicleId);

            modelBuilder.Entity<VehicleBooking>(entity =>
            {
                entity.OwnsOne(e => e.StartLocation, sa =>
                {
                    sa.Property(p => p.Longitude).HasColumnName("StartLongitude");
                    sa.Property(p => p.Latitude).HasColumnName("StartLatitude");
                });

                entity.OwnsOne(e => e.EndLocation, ea =>
                {
                    ea.Property(p => p.Longitude).HasColumnName("EndLongitude");
                    ea.Property(p => p.Latitude).HasColumnName("EndLatitude");
                });
            });

            modelBuilder.Entity<VehicleBooking>()
                .HasOne(booking => booking.Rating)
                .WithMany()
                .HasForeignKey(booking => booking.RatingId);

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.Property(e => e.Location).HasColumnType("geometry");
            });
        }
    }
}
