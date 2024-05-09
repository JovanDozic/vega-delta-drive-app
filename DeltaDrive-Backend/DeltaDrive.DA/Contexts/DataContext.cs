using DeltaDrive.DA.Contracts.Model;
using Microsoft.EntityFrameworkCore;

namespace DeltaDrive.DA.Contexts
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        // Database Sets:
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // FK Example:
            //modelBuilder.Entity<User>()
            //    .HasOne(user => user.Role)
            //    .WithMany()
            //    .HasForeignKey(user => user.RoleId);
        }
    }
}
