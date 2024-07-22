using JobPortal_New.Modeles.Entites;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace JobPortal_New.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Application> Applicationes { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Otp> Otps { get; set; }

        public DbSet<BlacklistedToken> BlacklistedTokens { get; set; }
        public DbSet<APIOptimize> aPIOptimizes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Application entity configuration
            //modelBuilder.Entity<Application>()
            //    .HasOne(a => a.Job)
            //    .WithMany(j => j.)
            //    .HasForeignKey(a => a.JobId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Job>()
            //    .HasOne(a => a.User)
            //    .WithMany(u => u.)
            //    .HasForeignKey(a => a.UserId)
            //    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
