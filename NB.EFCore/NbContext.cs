using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NB.EFCore.Entities;
using NyhedsBlog_Backend.Core.Models.Customer;

namespace NB.EFCore
{
    public class NbContext : DbContext
    {
        public NbContext(DbContextOptions<NbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CustomerEntity>()
                .HasMany<PaymentEntity>(c => c.Payments)
                .WithOne(p => p.Customer);
            
            modelBuilder.Entity<CategoryEntity>()
                .HasMany<PostEntity>(c => c.Posts)
                .WithOne(p => p.Category);

            modelBuilder.Entity<UserEntity>()
                .HasData(new UserEntity
                {
                    Id = 1,
                    Firstname = "Ulla",
                    Lastname = "Højgaard",
                    Email = "ulla@govarde.dk",
                    Username = "ulla@govarde.dk",
                    Password = "Alberte1212",
                    PhoneNumber = 42424242,
                    Role = 3
                });
            
            modelBuilder.Entity<UserEntity>()
                .HasData(new UserEntity
                {
                    Id = 2,
                    Firstname = "Christian",
                    Lastname = "Lindholm",
                    Email = "christian@mqservice.dk",
                    Username = "christian@mqservice.dk",
                    Password = "Christian1234",
                    PhoneNumber = 42424242,
                    Role = 3
                });
            
            modelBuilder.Entity<UserEntity>()
                .HasData(new UserEntity
                {
                    Id = 3,
                    Firstname = "Mads",
                    Lastname = "Qvistgaard",
                    Email = "mads@mqservice.dk",
                    Username = "mads@mqservice.dk",
                    Password = "Mads1234",
                    PhoneNumber = 42424242,
                    Role = 3
                });
            
            modelBuilder.Entity<PageEntity>()
                .HasData(new PageEntity
                {
                    Id = 1,
                    AuthorId = 1,
                    Title = "Forside",
                    Content = "Velkommen til forsiden.",
                    Date = DateTime.Now,
                    PrettyDescriptor = "frontpage"
                });
        }

        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<PageEntity> Pages { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        
        public DbSet<UserEntity> Users { get; set; }
        
        public DbSet<PaymentEntity> Payments { get; set; }
    }
}