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
                .HasOne(booking => booking.Driver)
                .WithMany()
                .HasForeignKey(booking => booking.DriverId);

            modelBuilder.Entity<VehicleBooking>(entity =>
            {
                entity.OwnsOne(e => e.StartLocation, sa =>
                {
                    sa.Property(p => p.Latitude).HasColumnName("StartLatitude");
                    sa.Property(p => p.Longitude).HasColumnName("StartLongitude");
                });

                entity.OwnsOne(e => e.EndLocation, ea =>
                {
                    ea.Property(p => p.Latitude).HasColumnName("EndLatitude");
                    ea.Property(p => p.Longitude).HasColumnName("EndLongitude");
                });
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                //entity.OwnsOne(e => e.Location, lo =>
                //{
                //    lo.Property(p => p.X).HasColumnName("Latitude");
                //    lo.Property(p => p.Y).HasColumnName("Longitude");
                //});
                entity.Property(e => e.Location).HasColumnType("geometry");
            });
        }
    }
}
