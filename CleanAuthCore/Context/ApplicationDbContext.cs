using CleanAuthCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanAuthCore.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, UserName = "admin", PasswordHash = "12345", Role = "Admin"},
                new User { Id = 2, UserName = "samet", PasswordHash = "12345", Role = "User"}
                );
        }
    }
}
