using Microsoft.EntityFrameworkCore;
using NB.EFCore.Entities;

namespace NB.EFCore
{
    public class NbContext : DbContext
    {
        public NbContext(DbContextOptions<NbContext> options) : base(options)
        {
            
        }
        
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<SubscriptionEntity> Subscriptions { get; set; }
        //public DbSet<PostEntity> Posts { get; set; }
        
        public DbSet<UserEntity> Users { get; set; }
        
        public DbSet<CategoryEntity> Category { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<CustomerEntity>()
            //    .HasData(new CustomerEntity
            //    {
            //        Id = 1,
            //        Firstname = "Bobby",
            //        Lastname = "Bobsen",
            //        Email = "bobbysen@bob.dk",
            //        PhoneNumber = 12345678,
            //        Username = "Bobby",
            //        Password = "bob123"
            //    });
        }
    }
}