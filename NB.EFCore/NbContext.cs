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
                    Firstname = "Bobby",
                    Lastname = "Olsen",
                    Email = "bobby@olsen.as",
                    Username = "test@test.com",
                    Password = "test1234",
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

            modelBuilder.Entity<CustomerEntity>()
                .HasData(new CustomerEntity
                {
                    Id = 1,
                    Firstname = "Ole",
                    Lastname = "Bobbysen",
                    Address = "Testgade 69",
                    Zipcode = 6969,
                    City = "Testby",
                    Email = "test1@test.com",
                    Username = "test1@test.com",
                    Password = "test1234",
                    PhoneNumber = 42042069,
                });
            
            modelBuilder.Entity<CustomerEntity>()
                .HasData(new CustomerEntity
                {
                    Id = 2,
                    Firstname = "Ole",
                    Lastname = "Bobbysen",
                    Address = "Testgade 69",
                    Zipcode = 6969,
                    City = "Testby",
                    Email = "test2@test.com",
                    Username = "test2@test.com",
                    Password = "test1234",
                    PhoneNumber = 42042069,
                });
            
            modelBuilder.Entity<CustomerEntity>()
                .HasData(new CustomerEntity
                {
                    Id = 3,
                    Firstname = "Ole",
                    Lastname = "Bobbysen",
                    Address = "Testgade 69",
                    Zipcode = 6969,
                    City = "Testby",
                    Email = "test3@test.com",
                    Username = "test3@test.com",
                    Password = "test1234",
                    PhoneNumber = 42042069,
                });

            var numberOfCategories = 10;
            List<CategoryEntity> generatedCategories = new List<CategoryEntity>();

            for (int i = 1; i <= numberOfCategories; i++)
            {
                generatedCategories.Add(new CategoryEntity
                {
                    Id = i,
                    Title = "Kategori " + i,
                    Description = "Tester med seedet data",
                    PrettyDescriptor = "kategori-" + i
                });
            }

            modelBuilder.Entity<CategoryEntity>()
                .HasData(generatedCategories);

            List<PostEntity> generatedPosts = new List<PostEntity>();
            var nextId = 1;
            for (int i = 1; i <= numberOfCategories; i++)
            {
                for (int x = 1; x <= 12; x++)
                {
                    generatedPosts.Add(new PostEntity
                    {
                        Id = nextId,
                        AuthorId = 1,
                        CategoryId = i,
                        PrettyDescriptor = "post-nr-" + nextId,
                        Title = "Post " + x + " in Categori " + i,
                        Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras a orci sapien. Vivamus erat quam, lobortis id tempor ac, condimentum ut turpis. Nullam vitae fermentum dolor. Ut et justo quis lacus hendrerit vulputate. Nam scelerisque nibh sed nunc rutrum porta vel tincidunt ex. Vestibulum ac nibh commodo, condimentum felis id, convallis nisi. Donec id sapien a orci malesuada vulputate. Nunc eu ultrices elit. Aliquam eget ligula euismod enim volutpat aliquam eu quis quam. Cras iaculis scelerisque neque, eget interdum magna. Donec porta tincidunt massa, non sodales erat facilisis eu. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras a orci sapien. Vivamus erat quam, lobortis id tempor ac, condimentum ut turpis. Nullam vitae fermentum dolor. Ut et justo quis lacus hendrerit vulputate. Nam scelerisque nibh sed nunc rutrum porta vel tincidunt ex. Vestibulum ac nibh commodo, condimentum felis id, convallis nisi. Donec id sapien a orci malesuada vulputate. Nunc eu ultrices elit. Aliquam eget ligula euismod enim volutpat aliquam eu quis quam. Cras iaculis scelerisque neque, eget interdum magna. Donec porta tincidunt massa, non sodales erat facilisis eu. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras a orci sapien. Vivamus erat quam, lobortis id tempor ac, condimentum ut turpis. Nullam vitae fermentum dolor. Ut et justo quis lacus hendrerit vulputate. Nam scelerisque nibh sed nunc rutrum porta vel tincidunt ex. Vestibulum ac nibh commodo, condimentum felis id, convallis nisi. Donec id sapien a orci malesuada vulputate. Nunc eu ultrices elit. Aliquam eget ligula euismod enim volutpat aliquam eu quis quam. Cras iaculis scelerisque neque, eget interdum magna. Donec porta tincidunt massa, non sodales erat facilisis eu. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras a orci sapien. Vivamus erat quam, lobortis id tempor ac, condimentum ut turpis. Nullam vitae fermentum dolor. Ut et justo quis lacus hendrerit vulputate. Nam scelerisque nibh sed nunc rutrum porta vel tincidunt ex. Vestibulum ac nibh commodo, condimentum felis id, convallis nisi. Donec id sapien a orci malesuada vulputate. Nunc eu ultrices elit. Aliquam eget ligula euismod enim volutpat aliquam eu quis quam. Cras iaculis scelerisque neque, eget interdum magna. Donec porta tincidunt massa, non sodales erat facilisis eu.",
                        Date = DateTime.Now,
                        FeaturedImageUrl = "https://picsum.photos/800/300",
                        Paid = true,
                        Price = 10
                    });
                    nextId++;
                }
            }

            modelBuilder.Entity<PostEntity>()
                .HasData(generatedPosts);
        }

        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<PageEntity> Pages { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        
        public DbSet<UserEntity> Users { get; set; }
        
        public DbSet<PaymentEntity> Payments { get; set; }
    }
}