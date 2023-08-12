using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace DataAccess
{
    public class LoginSampleContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<UserRole>().HasQueryFilter(ur => !ur.User.IsDeleted);



            modelBuilder.Entity<Role>().HasIndex(r => r.Name);
            modelBuilder.Entity<User>().HasIndex(u => u.Login);
            modelBuilder.Entity<UserRole>().HasIndex(ur => ur.UserId);




            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

    }
}
